using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    private Vector3 currentSpeed;
    private Vector2 clampX;
    private Vector2 clampY;
    private bool hasClampX;
    private bool hasClampY;

    void Start()
    {

    }

    void Update()
    {

    }

    public void Increase(Vector3 position)
    {
        this.currentSpeed += position;
    }

    void LateUpdate()
    {
        var desiredPosition = this.transform.position + currentSpeed;
        var x = hasClampX ? Mathf.Clamp(desiredPosition.x, clampX.x, clampX.y) : desiredPosition.x;
        var y = desiredPosition.y;
        if (hasClampY)
        {
            y = hasClampY ? Mathf.Clamp(desiredPosition.y, clampY.x, clampY.y) : desiredPosition.y;
        }

        Debug.Log(x + " | " + y);
        this.transform.position = new Vector3(x, y);

        Reset();
    }

    void Reset()
    {
        currentSpeed = Vector3.zero;
        clampX = Vector2.zero;
        clampY = Vector2.zero;
        hasClampX = false;
        hasClampY = false;
    }

    public void ClampYMin(float yMin)
    {
        this.hasClampY = true;
        clampY.x = yMin;
    }
}
