using UnityEngine;
using System.Collections;
using System.Linq;

public class BaseEnemyBehaviour : MonoBehaviour
{
    public Collision collisionComponent;
    public Health health;
    public SpriteRenderer sprRenderer;

    void Start()
    {
        this.StartCoroutine(CollideOnFriendlyBullets());
    }

    void Update()
    {

    }

    IEnumerator CollideOnFriendlyBullets()
    {
        while (this.enabled)
        {
            var layers = new[] { LayerMask.NameToLayer("FriendlyBullets") };

            var collisions = this.collisionComponent.GetAllCollisions();
            
            var collision = collisions.FirstOrDefault(c => layers.Contains(c.Collision.collider.gameObject.layer));
            if (collision != null)
            {
                // get the bullet type to calculate exact amount of damage
                StartCoroutine(TakeDamage(collision));
            }
            yield return 1;
        }
    }

    IEnumerator TakeDamage(CollisionWithDirection col)
    {
        GameObject.Destroy(col.Collision.collider.gameObject);
        this.health.Hurt(1);

        if (this.health.GetCurrentHealth() <= 0)
        {
            StartCoroutine(Die());
            yield break;
        }
            
        for (int i = 0; i < 3; i++)
        {
            this.sprRenderer.color = Color.black;
            yield return 1;
            this.sprRenderer.color = Color.white;
            yield return 1;
        }
    }

    IEnumerator Die()
    {
        GameObject.Destroy(this.gameObject);
        yield break;
    }
}
