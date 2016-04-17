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

    private const string ObstacleLayer = "Obstacle";

    private void Start()
    {
        StartAllCoroutines();
    }

    public override void MorphInto()
    {
        this.gravityComponent.Reset();
        base.MorphInto();
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }

    private void Update()
    {
        var layers = new[] { LayerMask.NameToLayer(ObstacleLayer) };

        var collisions = this.collisionComponents.SelectMany(c => c.GetAllCollisions()).Where(c => layers.Contains(c.Collision.collider.gameObject.layer));
        if (collisions.Any(c=>c.Direction == Vector2.down))
        {
            platformMovement.OnGround = true;
            gravityComponent.Reset();
        }
        else
        {
            platformMovement.OnGround = false;
        }
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
