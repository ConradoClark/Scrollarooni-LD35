using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Ship : MorphBase
{
    public ShipWeapon weapon;
    public Movement movement;
    public FourWayMovement fourWayMovement;
    public Collision collisionComponent;
    public Queue<Coroutine> waitingForAvailability;
    public bool blinking;
    public SpriteRenderer sprRenderer;
    public float collisionPushStrength;
    //put this elsewhere
    private const string ObstacleLayer = "Obstacle";

    public override void MorphInto()
    {
        base.MorphInto();
        waitingForAvailability = new Queue<Coroutine>();
        this.StartAllCoroutines();
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }

    private void StartAllCoroutines()
    {
        this.StartCoroutine(FiringControls());
        this.StartCoroutine(CollideOnObstacles());
    }

    IEnumerator CollideOnObstacles()
    {
        while (this.enabled)
        {
            var collision = this.collisionComponent.GetAllCollisions().FirstOrDefault(c => c.Collision.collider.gameObject.layer == LayerMask.NameToLayer(ObstacleLayer));
            if (collision!=null)
            {
                if (!blinking)
                {
                    StartCoroutine(Blink());
                    StartCoroutine(Push(collision.Direction));
                }
            }
            yield return 1; //brb
        }

        if (waitingForAvailability.Count == 0)
        {
            this.waitingForAvailability.Enqueue(StartCoroutine(WaitForAvailability()));
        }
    }

    IEnumerator Push(Vector2 direction)
    {
        var currentPosition = this.movement.transform.position;
        var pushDirection = -new Vector3(direction.x, direction.y).normalized;
        var push = new Vector3(direction.x, direction.y).normalized * collisionPushStrength;
        var desiredPosition = this.movement.transform.position + push;
        float amountMoved = 0f;

        Vector3 velocity = Vector3.zero;
        fourWayMovement.Reset();

        while (amountMoved < push.magnitude)
        {
            Vector3 pushMovement = (Vector3.SmoothDamp(currentPosition, desiredPosition, ref velocity, 0.15f) - currentPosition);
            var pushMovementCorrect = new Vector3(-pushMovement.x * Mathf.Abs(pushDirection.x), -pushMovement.y * Mathf.Abs(pushDirection.y));
            this.movement.Push(pushMovementCorrect);
            amountMoved += pushMovementCorrect.magnitude;
            yield return 1;
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
                weapon.Fire();
                yield return new WaitForSeconds(weapon.firingCooldown);
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
        StartAllCoroutines();
    }
}
