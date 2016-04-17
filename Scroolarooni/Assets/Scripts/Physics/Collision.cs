using UnityEngine;
using System.Linq;
using System;

public class Collision : MonoBehaviour
{
    public Movement movement;
    public RectTransform rect;

    private const string ObstacleLayer = "Obstacle";

    void Start()
    {

    }

    Vector2 raycastDistance = Vector2.one;
    public LayerMask layerMask;
    public float boxCastScale;
    public Vector2 collisionBoxSize;

    CollisionWithDirection topCollision, bottomCollision, leftCollision, rightCollision;

    public CollisionWithDirection[] GetAllCollisions()
    {
        return new[] { topCollision, bottomCollision, leftCollision, rightCollision }
           .Where(c => c != null)
           .ToArray();
    }

    void Update()
    {
        topCollision = bottomCollision = leftCollision = rightCollision = null;
        this.raycastDistance = movement.currentSpeed;
        TrySolvePossibleCollision(Vector2.down, Vector2.zero, CheckBottomCollision);
        TrySolvePossibleCollision(Vector2.up, Vector2.zero, CheckTopCollision);
        TrySolvePossibleCollision(Vector2.right, Vector2.zero, CheckRightCollision);
        TrySolvePossibleCollision(Vector2.left, Vector2.zero, CheckLeftCollision);
    }

    private void CheckBottomCollision(RaycastHit2D hit)
    {
        if (hit.normal != Vector2.up) return;

        var clampYMin = hit.point.y + collisionBoxSize.y / 2;
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer(ObstacleLayer))
        {
            movement.ClampYMin(clampYMin);
        }
        if (hit.distance < raycastDistance.magnitude)
        {
            bottomCollision = new CollisionWithDirection(Vector2.down, hit);
        }
    }

    private void CheckTopCollision(RaycastHit2D hit)
    {
        if (hit.normal != Vector2.down) return;

        var clampYMax = hit.point.y - collisionBoxSize.y / 2;
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer(ObstacleLayer))
        {
            movement.ClampYMax(clampYMax);
        }
        if (hit.distance < raycastDistance.magnitude)
        {
            topCollision = new CollisionWithDirection(Vector2.up, hit);
        }
    }

    private void CheckLeftCollision(RaycastHit2D hit)
    {
        if (hit.normal != Vector2.right) return;

        var clampXMin = hit.point.x + collisionBoxSize.x / 2;
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer(ObstacleLayer))
        {
            movement.ClampXMin(clampXMin);
        }
        if (hit.distance < raycastDistance.magnitude)
        {
            leftCollision = new CollisionWithDirection(Vector2.left, hit);
        }
    }

    private void CheckRightCollision(RaycastHit2D hit)
    {
        if (hit.normal != Vector2.left) return;

        var clampXMax = hit.point.x - collisionBoxSize.x / 2;
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer(ObstacleLayer))
        {
            movement.ClampXMax(clampXMax);
        }
        if (hit.distance < raycastDistance.magnitude)
        //if (Mathf.Abs(clampXMax - rect.position.x) < 0.1f || clampXMax < rect.position.x)
        {
            rightCollision = new CollisionWithDirection(Vector2.right, hit);
        }
    }

    private bool TrySolvePossibleCollision(Vector2 direction, Vector2 offset, Action<RaycastHit2D> onDetectableCollision)
    {

        Vector2 position = new Vector2(rect.position.x - collisionBoxSize.x / 2 * boxCastScale * Mathf.Abs(direction.y), rect.position.y - collisionBoxSize.y / 2 * boxCastScale * Mathf.Abs(direction.x));

        var boxPosition = new Vector2(position.x + offset.x + direction.x * collisionBoxSize.x / 2,
                                      position.y + offset.y + direction.y * collisionBoxSize.y / 2);

        Rect rect2 = new Rect(boxPosition, new Vector3(Mathf.Abs(direction.x * raycastDistance.x) + Mathf.Abs(direction.y) * collisionBoxSize.x * boxCastScale,
                                                       Mathf.Abs(direction.y * raycastDistance.y) + Mathf.Abs(direction.x) * collisionBoxSize.y * boxCastScale));
        DebugRect(rect2);

        //+ direction.y * collisionBoxSize.x  * boxCastScale
        // direction.x*collisionBoxSize.x
        // boxCastSize.x * direction.x
        // 
        var hits = Physics2D.BoxCastAll(rect2.position, rect2.size, 0, direction, 0f, layerMask);
        var closestHit = hits.OrderByDescending(hit => hit.normal == -direction).ThenBy(hit => hit.distance).FirstOrDefault();
        bool result = closestHit != default(RaycastHit2D);
        if (result)
        {
            onDetectableCollision(closestHit);
        }
        //return result;
        return false;
    }

    void DebugRect(Rect rect)
    {
        Debug.DrawLine(rect.min, new Vector3(rect.max.x, rect.min.y));
        Debug.DrawLine(rect.min, new Vector3(rect.min.x, rect.max.y));
        Debug.DrawLine(new Vector3(rect.max.x, rect.min.y), new Vector3(rect.max.x, rect.max.y));
        Debug.DrawLine(new Vector3(rect.min.x, rect.max.y), new Vector3(rect.max.x, rect.max.y));
    }
}
