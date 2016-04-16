using UnityEngine;
using System.Collections;

public class Morph : MonoBehaviour
{
    //temp
    int morphStatus = 0;
    private Soldier soldierMorph;
    private Ship shipMorph;

    void Start()
    {
        this.soldierMorph = this.GetComponentInChildren<Soldier>(true);
        this.shipMorph = this.GetComponentInChildren<Ship>(true);
        this.MorphIntoShip();
    }

    void Update()
    {
        // Remove, just for testing
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (morphStatus == 0)
            {
                //remove this too
                this.MorphIntoSoldier();
                morphStatus = 1;
            }
            else
            {
                this.MorphIntoShip();
                morphStatus = 0;
            }
        }
    }

    void MorphIntoSoldier()
    {        
        shipMorph.Deactivate();
        soldierMorph.MorphInto();
    }

    void MorphIntoShip()
    {
        soldierMorph.Deactivate();
        shipMorph.MorphInto();        
    }
}
