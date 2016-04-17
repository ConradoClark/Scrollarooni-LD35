using UnityEngine;
using System.Collections;

public class BaseCollectible : MonoBehaviour
{
    public virtual void Collect()
    {
        GameObject.Destroy(this.gameObject);
    }
}
