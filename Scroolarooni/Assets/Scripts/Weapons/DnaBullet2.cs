using UnityEngine;
using System.Collections;

public class DnaBullet2 : BaseBullet
{
    void Start()
    {

    }

    float frame;
    void Update()
    {
        this.transform.position += new Vector3(10, Mathf.Cos(frame) * 2, 0);
        this.frame += Mathf.PI / 10f;

    }
}
