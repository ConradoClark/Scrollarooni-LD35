using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Soldier : MorphBase
{
    public Gravity gravityComponent;
    public Collision[] collisionComponents;
    public PlatformMovement platformMovement;
    public SoldierWeapon soldierWeapon;
    public Queue<Coroutine> waitingForAvailability;
    public AnimManager animManager;
    public Health health;
    public SpriteRenderer sprRenderer;

    private const string ObstacleLayer = "Obstacle";
    private const string Enemies = "Enemies";
    private const string CollectiblesLayer = "Collectibles";
    private bool blinking;

    private void Start()
    {
        StartAllCoroutines();
        this.waitingForAvailability = new Queue<Coroutine>();
    }

    public override void MorphInto()
    {
        this.gravityComponent.Reset();
        base.MorphInto();        
        this.StartAllCoroutines();
        this.sprRenderer.enabled = true;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        this.DestroyAllBeams();
        this.platformMovement.UnblockFlip();
    }

    private void DestroyAllBeams()
    {
        var beams = this.GetComponentsInChildren<Beam>();
        foreach(var beam in beams)
        {
            GameObject.Destroy(beam.gameObject);
        }
    }

    private void Update()
    {
        var layers = new[] { LayerMask.NameToLayer(ObstacleLayer) };
        var layersEnemy = new[] { LayerMask.NameToLayer(Enemies) };

        var collisions = this.collisionComponents.SelectMany(c => c.GetAllCollisions())
            .Where(c => c.Collision.collider!=null)
            .Where(c => layers.Contains(c.Collision.collider.gameObject.layer));

        if (collisions.Any(c=>c.Direction == Vector2.down))
        {
            platformMovement.OnGround = true;
            gravityComponent.Reset();
        }
        else
        {
            platformMovement.OnGround = false;
            if (!animManager.animator.GetCurrentAnimatorClipInfo(0).Any(clipInfo => clipInfo.clip.name == "Soldier_Jumping"))
            {
                animManager.QueueAnimation("Soldier_Falling");
            }
        }

        if (collisions.Any(c => c.Direction == Vector2.up))
        {
            platformMovement.CancelJump();
            //gravityComponent.Reset();
        }

        var enemyCollisions = this.collisionComponents.SelectMany(c => c.GetAllCollisions())
            .Where(c => c.Collision.collider!=null)
            .Where(c => layersEnemy.Contains(c.Collision.collider.gameObject.layer));

        if (enemyCollisions.Any())
        {
            if (!blinking)
            {
                this.health.Hurt(1);
                StartCoroutine(Blink());
            }
        }

        var collectiblesCollisions = this.collisionComponents.SelectMany(c => c.GetAllCollisions()).Where(c => c.Collision.collider.gameObject.layer == LayerMask.NameToLayer(CollectiblesLayer));
        foreach (var collision in collectiblesCollisions)
        {
            BaseCollectible collectible = collision.Collision.collider.gameObject.GetComponent<BaseCollectible>();
            collectible.Collect();
        }
    }

    IEnumerator Blink()
    {
        blinking = true;
        for (int i = 0; i < 6; i++)
        {
            sprRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sprRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        blinking = false;
    }

    IEnumerator FiringControls()
    {
        while (this.enabled)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                soldierWeapon.StartCharging();
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                soldierWeapon.Release();
            }
            yield return 1;
        }
        if (waitingForAvailability.Count == 0)
        {
            this.waitingForAvailability.Enqueue(StartCoroutine(WaitForAvailability()));
        }
    }

    IEnumerator WaitForAvailability()
    {
        while (!this.enabled)
        {
            yield return 1;
        }
        this.waitingForAvailability.Dequeue();
        StartAllCoroutines();
    }

    private void StartAllCoroutines()
    {
        this.StartCoroutine(FiringControls());
    }
}
