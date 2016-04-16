using UnityEngine;
using System.Collections;

public class NormalBullet : BaseBullet
{
    void Update()
    {
        //temp
        this.transform.position += new Vector3(10, 0, 0);
    }
}
