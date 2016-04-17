using UnityEngine;
using System.Collections;

public class Death : MonoBehaviour
{
    void Start()
    {

    }

    public void Die()
    {
        //simple at first
        GameObject.Destroy(this.gameObject);
    }
}
