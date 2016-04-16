using UnityEngine;

public class FourWayMovement : MonoBehaviour
{
    public Vector2 maxSpeed;
    public Movement movement;
    Vector3 currentVelocity;

    void FixedUpdate()
    {
        Vector2 axis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        Vector3 finalSpeed = new Vector3(axis.x * maxSpeed.x, axis.y * maxSpeed.y) * 10f;
        Vector3 resultingSpeed = Vector3.SmoothDamp(this.transform.position, this.transform.position+finalSpeed, ref currentVelocity, 0.75f) - this.transform.position;
        movement.Increase(resultingSpeed);
    }
}
