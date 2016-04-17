using UnityEngine;
using System.Collections;

public class Gravity : MonoBehaviour
{
    public Vector2 gravitySpeed;
    public Movement movement;
    private float scale = 1.0f;

    void Start()
    {

    }

    Vector3 currentVelocity;
    float elapsedTime = 0f;
    void Update()
    {
        Vector3 finalSpeed = new Vector3(gravitySpeed.x, gravitySpeed.y) * this.scale;
        //Vector3 resultingSpeed = Vector3.SmoothDamp(this.transform.position, this.transform.position + finalSpeed , ref currentVelocity, 0.35f) - this.transform.position;
        Vector3 resultingSpeed = Vector3.Lerp(this.transform.position, this.transform.position + finalSpeed, Mathf.Pow(elapsedTime,1.25f));
        //currentVelocity += finalSpeed;
        elapsedTime += Time.deltaTime;

        movement.Push(resultingSpeed-this.transform.position);
    }

    public void Reset()
    {
        //this.currentVelocity = Vector3.zero;
        elapsedTime = 0.01f;
        this.scale = 1.0f;
    }

    public void Scale(float factor)
    {
        this.scale = factor;
    }
}
