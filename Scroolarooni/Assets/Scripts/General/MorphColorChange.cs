using UnityEngine;
using System.Collections;

public class MorphColorChange : MonoBehaviour
{
    public ColorChangeBehaviour change;
    public Color colorWhenShip;
    public Color colorWhenSoldier;
    public Morph morph;
    public string gameObjectName;

    private void Start()
    {
        if (morph == null)
        {
            morph = GameObject.Find(gameObjectName).GetComponent<Morph>();
        }
        morph.OnMorph += this.Change;
    }

    public void Change(MorphType type)
    {
        switch (type)
        {
            case MorphType.Ship:
                if (change != null)
                {
                    change.ChangeColors(colorWhenShip);
                }
                break;
            case MorphType.Soldier:
                if (change != null)
                {
                    change.ChangeColors(colorWhenSoldier);
                }
                break;
            case MorphType.Submarine:
                if (change != null)
                {

                }
                //change.ChangeColors(colorWhenShip);
                break;
        }
    }
}
