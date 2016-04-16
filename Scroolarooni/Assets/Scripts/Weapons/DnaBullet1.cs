using UnityEngine;
using System.Collections;

public class DnaBullet1 : BaseBullet
{
    void Start()
    {

    }

    float frame;
    void Update()
    {
        this.transform.position += new Vector3(10, Mathf.Sin(frame)*2 , 0);
        this.frame += Mathf.PI/20f;
        
    }
}
