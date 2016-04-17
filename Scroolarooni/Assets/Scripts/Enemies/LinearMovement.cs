using UnityEngine;
using System.Collections;

public class LinearMovement : EnemyMovement
{
    public Vector2 Direction;
    public float Speed;    
    Vector3 currentVelocity;

    void Update()
    {
        Vector2 axis = this.Direction.normalized;
        Vector3 finalSpeed = axis * Speed * 10f;
        Vector3 resultingSpeed = Vector3.SmoothDamp(this.transform.position, this.transform.position + finalSpeed, ref currentVelocity, 0.45f) - this.transform.position;
        movement.Push(resultingSpeed);
    }
}
