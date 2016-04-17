using UnityEngine;
using System.Collections;

public class DestroyWhenOutOfBounds : MonoBehaviour
{
    public Camera cam;
    public string cameraName;
    public Vector2 screenSize;

    void Start()
    {
        if (this.cam == null)
        {
            this.cam = GameObject.Find(cameraName).GetComponent<Camera>();
        }
    }

    void Update()
    {
        bool leavingX = (Mathf.Abs(this.transform.position.x) - Mathf.Abs(cam.transform.position.x) > 1.5 * screenSize.x);
        bool leavingY = (Mathf.Abs(this.transform.position.y) - Mathf.Abs(cam.transform.position.y) > 1.5 * screenSize.y);

        if (leavingX || leavingY)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
