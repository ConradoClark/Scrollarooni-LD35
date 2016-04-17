using UnityEngine;
using System.Collections;

public class FollowScrollMovement : MonoBehaviour
{
    public float speedScale;
    public string mainCameraName;
    public Movement movement;

    private Autoscrolling autoScroller;
    private Vector3 currentVelocity;

    void Start()
    {
        var cam = GameObject.Find(mainCameraName);
        if (cam == null) throw new System.Exception("didn't find camera!");
        this.autoScroller = cam.GetComponent<Autoscrolling>();
    }

    void Update()
    {
        Vector3 finalSpeed = autoScroller.GetCurrentScrollSpeed() * speedScale;
        Vector3 resultingSpeed = Vector3.SmoothDamp(this.transform.position, this.transform.position + finalSpeed, ref currentVelocity, autoScroller.GetDampTime(), finalSpeed.magnitude) - this.transform.position;
        movement.Push(resultingSpeed);
    }
}
