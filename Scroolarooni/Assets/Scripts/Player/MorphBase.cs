using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MorphBase : MonoBehaviour
{
    public virtual void MorphInto()
    {
        this.gameObject.SetActive(true);
    }

    public virtual void Deactivate()
    {
        this.gameObject.SetActive(false);
    }
}
