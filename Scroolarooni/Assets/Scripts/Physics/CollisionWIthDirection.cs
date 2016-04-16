using UnityEngine;

public class CollisionWithDirection
{
    public CollisionWithDirection(Vector2 direction, RaycastHit2D collision)
    {
        this.Direction = direction;
        this.Collision = collision;
    }
    public Vector2 Direction;
    public RaycastHit2D Collision;
}
