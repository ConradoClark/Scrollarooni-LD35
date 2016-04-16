using UnityEngine;
using System.Collections;

public class PlatformMovement : MonoBehaviour
{
    public float maxSpeed;
    public Movement movement;
    Vector3 currentVelocity;

    void Update()
    {
        Vector2 axis = new Vector2(Input.GetAxisRaw("Horizontal"), 0f).normalized;
        Vector3 finalSpeed = new Vector3(axis.x * maxSpeed, 0) * 10f;
        Vector3 resultingSpeed = Vector3.SmoothDamp(this.transform.position, this.transform.position + finalSpeed, ref currentVelocity, 0.45f) - this.transform.position;
        movement.Push(resultingSpeed);
    }


}
