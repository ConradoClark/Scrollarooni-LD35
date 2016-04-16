using UnityEngine;
using System.Collections;

public class Soldier : MorphBase
{
    public Gravity gravityComponent;

    public override void MorphInto()
    {
        this.gravityComponent.Reset();
        base.MorphInto();
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }
}
