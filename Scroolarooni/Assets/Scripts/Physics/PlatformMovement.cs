using UnityEngine;
using System.Collections;

public class PlatformMovement : MonoBehaviour
{
    public SpriteRenderer sprRenderer;
    public float maxSpeed;
    public Movement movement;
    Vector3 currentVelocity;
    Vector3 currentVelocityY;
    float yAxis;
    public AnimManager animManager;

    public bool OnGround { get; set; }
    private bool blockFlip;

    public bool IsFlipped()
    {
        return this.sprRenderer.flipX;
    }

    public void BlockFlip()
    {
        blockFlip = true;
    }

    public void UnblockFlip()
    {
        blockFlip = false;
    }

    void Update()
    {
        if (Input.GetAxisRaw("Vertical") > 0 && this.OnGround)
        {
            yAxis = 5;
            animManager.QueueAnimationWaitToFinish("Soldier_Jumping");
        }

        Vector2 axis = new Vector2(Input.GetAxisRaw("Horizontal"), 0f).normalized;

        if (axis == Vector2.zero) currentVelocity = Vector3.zero;

        if (!blockFlip)
        {
            if (axis.x < 0)
            {
                sprRenderer.flipX = true;
            }
            else if (axis.x > 0)
            {
                sprRenderer.flipX = false;
            }
        }

        Vector3 finalSpeed = new Vector3(axis.x * maxSpeed, 0) * 10f;

        if (finalSpeed.magnitude > 0)
        {
            animManager.QueueAnimation("Soldier_Walking");
        }
        else
        {
            animManager.QueueAnimation("Soldier_Standing");
        }

        Vector3 resultingSpeed = Vector3.SmoothDamp(this.transform.position, this.transform.position + finalSpeed, ref currentVelocity, 0.45f) - this.transform.position;
        movement.Push(resultingSpeed);

        Vector3 finalSpeedY = new Vector3(0, yAxis) * 10f;
        Vector3 resultingSpeedY = Vector3.SmoothDamp(this.transform.position, this.transform.position + finalSpeedY, ref currentVelocityY, 0.10f) - this.transform.position;
        movement.Push(resultingSpeedY);

        yAxis = Mathf.Max(0f, yAxis - 0.1f);
    }
}
