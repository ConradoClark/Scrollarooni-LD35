using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class Collision : MonoBehaviour
{
    public Movement movement;
    public RectTransform rect;
    void Start()
    {

    }

    Vector2 raycastDistance = Vector2.one;
    public LayerMask layerMask;
    public float RaycastScale = 1f;

    CollisionWithDirection topCollision, bottomCollision, leftCollision, rightCollision;

    public CollisionWithDirection[] GetAllCollisions()
    {
        return new[] { topCollision, bottomCollision, leftCollision, rightCollision }
           .Where(c=>c!=null)
           .ToArray();
    }

    void Update()
    {
        topCollision = bottomCollision = leftCollision = rightCollision = null;
        this.raycastDistance = movement.currentSpeed;
        TrySolvePossibleCollision(Vector2.down, new Vector2(0,-16f), CheckBottomCollision);
        TrySolvePossibleCollision(Vector2.up, new Vector2(0,11.5f), CheckTopCollision);
    }

    private void CheckBottomCollision(RaycastHit2D hit)
    {
        var clampYMin = hit.point.y + rect.sizeDelta.y / 2;
        movement.ClampYMin(clampYMin);
        if (Mathf.Abs(clampYMin-rect.position.y) < 0.1f)
        {
            bottomCollision = new CollisionWithDirection(Vector2.down,hit);
        }
    }

    private void CheckTopCollision(RaycastHit2D hit)
    {
        var clampYMax = hit.point.y - rect.sizeDelta.y / 2;
        movement.ClampYMax(clampYMax);
        if (Mathf.Abs(clampYMax - rect.position.y) < 0.1f)
        {
            topCollision = new CollisionWithDirection(Vector2.up, hit);
        }
    }

    private void CheckLeftCollision(RaycastHit2D hit)
    {
        if (hit.distance < 0.1f)
        {
            leftCollision = new CollisionWithDirection(Vector2.left, hit);
        }
    }

    private void CheckRightCollision(RaycastHit2D hit)
    {
        if (hit.distance < 0.1f)
        {
            rightCollision = new CollisionWithDirection(Vector2.right, hit);
        }
    }

    private bool TrySolvePossibleCollision(Vector2 direction, Vector2 offset, Action<RaycastHit2D> onDetectableCollision)
    {
        Vector2 position = new Vector2(rect.position.x, rect.position.y);

        var debugRect = new Rect(position + offset, rect.sizeDelta * RaycastScale);
        var hits = Physics2D.BoxCastAll(position + offset, rect.sizeDelta * RaycastScale + offset, 0, direction, raycastDistance.magnitude, layerMask);
        var closestHit = hits.OrderBy(hit => hit.distance).FirstOrDefault();
        bool result = closestHit != default(RaycastHit2D);
        if (result)
        {
            onDetectableCollision(closestHit);
        }
        return result;
    }
}
