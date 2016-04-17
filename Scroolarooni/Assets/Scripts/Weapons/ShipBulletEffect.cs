using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipBulletEffect : BulletEffect
{
    public override void Run(GameObject affected, Dictionary<string, object> extraParams)
    {
        base.Run(affected, extraParams);

        var scaleFadeOut = this.gameObject.AddComponent<ScaleFadeOut>();
        scaleFadeOut.timeToFadeOut = 0.5f;

        GameObject.Destroy(this.gameObject, 0.5f);
    }
}
