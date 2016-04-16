using UnityEngine;
using System.Collections;

public class Ship : MorphBase
{
    public ShipWeapon weapon;
    public override void MorphInto()
    {
        base.MorphInto();
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }

    private void Update()
    {
        //temp
        if (Input.GetKeyDown(KeyCode.Space))
        {
            weapon.Fire();
        }
    }
}
