using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

public class Collision : MonoBehaviour
{
    public Movement movement;
    public RectTransform rect;

    private const string ObstacleLayer = "Obstacle";

    void Start()
    {
        overlapping = new List<RaycastHit2D>();
    }

    Vector2 raycastDistance = Vector2.one;
    public LayerMask layerMask;
    public float boxCastScale;
    public Vector2 collisionBoxSize;
    public Vector2 offset;

    CollisionWithDirection topCollision, bottomCollision, leftCollision, rightCollision;
    List<RaycastHit2D> overlapping;

    public RaycastHit2D[] GetOverlappingBoundaries()
    {
        if (this.overlapping == null)
        {
            this.overlapping = new List<RaycastHit2D>();
        }

        return this.overlapping.ToArray();
    }

    public CollisionWithDirection[] GetAllCollisions(bool includeOverlapping = false)
    {
        var collisions = new[] { topCollision, bottomCollision, leftCollision, rightCollision }
           .Where(c => c != null);

        var additionalCollisions = new CollisionWithDirection[0];

        if (includeOverlapping)
        {
            additionalCollisions = this.GetOverlappingBoundaries().Select(b => new CollisionWithDirection(Vector2.zero, b)).ToArray();

            //if (additionalCollisions.Any())
            //{
            //    Debug.Break();
            //}
        }

        return collisions.Concat(additionalCollisions).ToArray();
    }

    void Update()
    {
        overlapping.Clear();
        topCollision = bottomCollision = leftCollision = rightCollision = null;
        this.raycastDistance = movement.currentSpeed;
        TrySolvePossibleCollision(Vector2.down, offset, CheckBottomCollision);
        TrySolvePossibleCollision(Vector2.up, offset, CheckTopCollision);
        TrySolvePossibleCollision(Vector2.right, offset, CheckRightCollision);
        TrySolvePossibleCollision(Vector2.left, offset, CheckLeftCollision);
    }

    private void CheckBottomCollision(RaycastHit2D hit)
    {
        if (hit.normal != Vector2.up) return;

        var clampYMin = hit.point.y + collisionBoxSize.y / 2 - offset.y;
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

        var clampYMax = hit.point.y - collisionBoxSize.y / 2 - offset.y;
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

        var clampXMin = hit.point.x + collisionBoxSize.x / 2 - offset.x;
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

        var clampXMax = hit.point.x - collisionBoxSize.x / 2 - offset.x;
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

        //scrap this boxcast, meh
        //var hits = Physics2D.BoxCastAll(rect2.position, rect2.size, 0, direction, 0f, layerMask);
        var halfway = new Vector2(Mathf.Abs(direction.y) * rect2.size.x / 2, Mathf.Abs(direction.x) * rect2.size.y / 2);

        // triple raycast ftw
        var hits = Physics2D.RaycastAll(rect2.position, direction, Mathf.Abs(direction.x * rect2.size.x + direction.y * rect2.size.x), layerMask);
        var hits2 = Physics2D.RaycastAll(rect2.position + halfway, direction, Mathf.Abs(direction.x * rect2.size.x + direction.y * rect2.size.x), layerMask);
        var hits3 = Physics2D.RaycastAll(rect2.position + halfway*2, direction, Mathf.Abs(direction.x * rect2.size.x + direction.y * rect2.size.x), layerMask);

        var allHits = hits.Concat(hits2).Concat(hits3);

        var closestHit = allHits.OrderByDescending(hit => hit.normal == -direction).ThenBy(hit => hit.distance).FirstOrDefault();
        bool result = closestHit != default(RaycastHit2D);
        if (result)
        {
            if (closestHit.collider.bounds.Intersects(new Bounds(rect2.position, rect2.size)))
            {
                overlapping.Add(closestHit);
            }
            onDetectableCollision(closestHit);
        }
        return result;
    }

    void DebugRect(Rect rect)
    {
        Debug.DrawLine(rect.min, new Vector3(rect.max.x, rect.min.y));
        Debug.DrawLine(rect.min, new Vector3(rect.min.x, rect.max.y));
        Debug.DrawLine(new Vector3(rect.max.x, rect.min.y), new Vector3(rect.max.x, rect.max.y));
        Debug.DrawLine(new Vector3(rect.min.x, rect.max.y), new Vector3(rect.max.x, rect.max.y));
    }
}
