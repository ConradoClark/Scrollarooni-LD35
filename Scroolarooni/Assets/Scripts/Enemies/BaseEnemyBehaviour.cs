using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class BaseEnemyBehaviour : MonoBehaviour
{
    public Collision collisionComponent;
    public Health health;
    public SpriteRenderer sprRenderer;
    public GameObject deathPoof;
    public Movement movement;
    public int PointsEarned;
    public Score Score;

    bool blinking;
    bool stunned;
    bool dead;
    Queue<Coroutine> stuns;

    void Start()
    {
        this.StartCoroutine(CollideOnFriendlyBullets());
        this.stuns = new Queue<Coroutine>();
        if (this.Score == null)
        {
            var gm = GameObject.Find("Score");
            if (gm != null)
            {
                this.Score = gm.GetComponent<Score>();
            }
        }
    }

    void Update()
    {

    }

    IEnumerator CollideOnFriendlyBullets()
    {
        while (this.enabled)
        {
            var layers = new[] { LayerMask.NameToLayer("FriendlyBullets") };

            var collisions = this.collisionComponent.GetAllCollisions(true);

            var collision = collisions.FirstOrDefault(c => layers.Contains(c.Collision.collider.gameObject.layer));
            if (collision != null)
            {
                // get the bullet type to calculate exact amount of damage
                StartCoroutine(TakeDamage(collision));
            }
            yield return 1;
        }
    }

    IEnumerator TakeDamage(CollisionWithDirection col)
    {
        //GameObject.Destroy(col.Collision.collider.gameObject);
        uint dmgAmount = 1;
        var damage = new[] { col.Collision.collider.gameObject.GetComponent<Damage>(),
            col.Collision.collider.gameObject.GetComponentInParent<Damage>(),
            col.Collision.collider.gameObject.GetComponentInChildren<Damage>() }.Where(dmg => dmg != null).FirstOrDefault();

        if (damage != null)
        {
            dmgAmount = (uint)damage.GetDamage();
        }

        if (dmgAmount > 0)
        {
            Dictionary<string, object> extraParams = new Dictionary<string, object>();
            var beamComponent = col.Collision.collider.gameObject.GetComponent<Beam>();
            if (beamComponent != null)
            {
                extraParams.Add("Direction(Beam)", beamComponent.Direction);
            }

            BulletEffect[] effects = col.Collision.collider.GetComponents<BulletEffect>();
            foreach (var effect in effects)
            {
                effect.Run(this.gameObject, extraParams);
            }

            this.health.Hurt(dmgAmount);
            Debug.Log("Enemy Hit!");

            if (this.health.GetCurrentHealth() <= 0)
            {
                StartCoroutine(Die());
                yield break;
            }

            if (!blinking)
            {
                StartCoroutine(Blink());
            }
        }
    }

    public void Stun(float seconds)
    {
        this.stuns.Enqueue(StartCoroutine(StunEnemy(seconds)));
    }

    IEnumerator StunEnemy(float seconds)
    {
        while (this.stuns.Count > 1)
        {
            yield return true;
        }

        this.stunned = true;
        var gravity = this.GetComponent<Gravity>();
        if (gravity != null)
        {
            gravity.Reset();
            gravity.Scale(0.5f);
        }
        yield return new WaitForSeconds(seconds);
        this.stuns.Dequeue();

        if (this.stuns.Count == 0)
        {
            gravity.Reset();
        }
        this.stunned = false;
    }

    public bool IsStunned()
    {
        return this.stunned;
    }

    public bool IsDead()
    {
        return this.dead;
    }

    IEnumerator Blink()
    {
        blinking = true;
        for (int i = 0; i < 3; i++)
        {
            this.sprRenderer.color = new Color(1, 1, 1, 0f);
            yield return 1;
            this.sprRenderer.color = Color.white;
            yield return 1;
        }
        blinking = false;
    }

    IEnumerator Die()
    {
        if (dead) yield break;
        dead = true;

        if (this.Score != null)
        {
            this.Score.Increase((uint)this.PointsEarned);
        }

        if (this.movement != null)
        {
            this.movement.Kill();
        }

        if (this.deathPoof != null)
        {
            GameObject deathParticle = Instantiate(this.deathPoof);
            deathParticle.transform.position = this.gameObject.transform.position;
        }

        var scaleFadeOut =  this.gameObject.AddComponent<ScaleFadeOut>();
        scaleFadeOut.timeToFadeOut = 0.5f;

        GameObject.Destroy(this.gameObject, 0.5f);
        yield break;
    }
}
