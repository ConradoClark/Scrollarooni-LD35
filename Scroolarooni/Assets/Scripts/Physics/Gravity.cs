using UnityEngine;
using System.Collections;

public class Gravity : MonoBehaviour
{
    public Vector2 gravitySpeed;
    public Movement movement;
    void Start()
    {

    }

    Vector3 currentVelocity;

    void Update()
    {
        Vector3 finalSpeed = new Vector3(gravitySpeed.x, gravitySpeed.y) * 10f;
        Vector3 resultingSpeed = Vector3.SmoothDamp(this.transform.position, this.transform.position + finalSpeed, ref currentVelocity, 0.55f) - this.transform.position;
        movement.Increase(resultingSpeed);
    }

    public void Reset()
    {
        this.currentVelocity = Vector3.zero;
    }
}
