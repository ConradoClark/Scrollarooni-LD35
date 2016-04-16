using UnityEngine;
using System.Collections;

public class ShipWeapon : MonoBehaviour
{
    public GameObject projectile1;
    public GameObject projectile2;
    public void Fire()
    {
        var dot1 = GameObject.Instantiate(projectile1);
        var dot2 = GameObject.Instantiate(projectile2);

        BaseBullet bullet1 = dot1.GetComponent<BaseBullet>();
        BaseBullet bullet2 = dot2.GetComponent<BaseBullet>();

        dot1.transform.SetParent(this.transform, false);
        dot2.transform.SetParent(this.transform, false);
        dot1.transform.localPosition = bullet1.positionInSlot1;
        dot2.transform.localPosition = bullet2.positionInSlot2;
        dot1.transform.SetParent(null);
        dot2.transform.SetParent(null);
    }
}
