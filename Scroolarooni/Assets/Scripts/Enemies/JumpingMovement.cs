using UnityEngine;
using System.Collections;
using System.Linq;

public class JumpingMovement : EnemyMovement
{
    public float Strength;
    Vector3 currentVelocity;
    public Collision collisionComponent;
    public Gravity gravityComponent;
    public BaseEnemyBehaviour enemyBehaviour;

    private bool OnGround;
    private const string ObstacleLayer = "Obstacle";
    float yAxis = 0;

    void Update()
    {
        if (enemyBehaviour.IsStunned())
        {
            yAxis = 0;
            return;
        }
        // THIS SHOULD BE ON BASE ENEMY BEHAVIOUR
        var layers = new[] { LayerMask.NameToLayer(ObstacleLayer) };

        var collisions = this.collisionComponent.GetAllCollisions().Where(c => layers.Contains(c.Collision.collider.gameObject.layer));
        if (collisions.Any(c => c.Direction == Vector2.down))
        {
            OnGround= true;
            gravityComponent.Reset();
        }
        else
        {
            OnGround= false;
        }

        if (OnGround)
        {
            yAxis = 5;
        }

        Vector2 axis = new Vector2(0, Strength);
        Vector3 finalSpeed = axis * yAxis * 10f;
        Vector3 resultingSpeed = Vector3.SmoothDamp(this.transform.position, this.transform.position + finalSpeed, ref currentVelocity, 0.45f) - this.transform.position;
        movement.Push(resultingSpeed);

        yAxis = Mathf.Max(0f, yAxis - 0.1f);
    }
}
