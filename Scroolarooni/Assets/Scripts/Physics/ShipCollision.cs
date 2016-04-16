using UnityEngine;
using System.Collections;
using System.Linq;

public class ShipCollision : MonoBehaviour
{
    public Movement movement;
    public RectTransform rect;
    void Start()
    {

    }

    float raycastDistance = 128f;
    public LayerMask layerMask;
    void Update()
    {
        // TO DO (OTHER COLLISIONS)
        Vector2 position = new Vector2(rect.position.x, rect.position.y);
        //up
        //var hitsUp = Physics2D.BoxCastAll(position, rect.sizeDelta, 0, Vector2.up, raycastDistance, layerMask);
        ////down
        var hitsDown = Physics2D.BoxCastAll(position, rect.sizeDelta, 0, Vector2.down, raycastDistance, layerMask);
        var hitsDownCloser = hitsDown.OrderBy(hit => hit.distance).FirstOrDefault();
        if (hitsDownCloser != default(RaycastHit2D))
        {
            movement.ClampYMin(hitsDownCloser.point.y + rect.sizeDelta.y/2);
        }
        ////left
        //var hitsLeft = Physics2D.BoxCastAll(position, rect.sizeDelta, 0, Vector2.left, raycastDistance, layerMask);
        ////right
        //var hitsRight = Physics2D.BoxCastAll(position, rect.sizeDelta, 0, Vector2.right, raycastDistance, layerMask);
        //DrawCollisionBox();
    }


    void DrawCollisionBox()
    {
        var realPos = rect.position - new Vector3(rect.sizeDelta.x / 2f, rect.sizeDelta.y / 2);
        // down
        Debug.DrawLine(realPos, realPos + new Vector3(rect.sizeDelta.x, 0), Color.blue);
        Debug.DrawLine(realPos, realPos + new Vector3(0, -raycastDistance), Color.blue);
        Debug.DrawLine(realPos + new Vector3(rect.sizeDelta.x, 0), realPos + new Vector3(rect.sizeDelta.x, -raycastDistance), Color.blue);
        Debug.DrawLine(realPos + new Vector3(0, -raycastDistance), realPos + new Vector3(rect.sizeDelta.x, -raycastDistance), Color.blue);
    }
}
