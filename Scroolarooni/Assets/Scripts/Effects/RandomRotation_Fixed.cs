using UnityEngine;
using System.Collections;

public class RandomRotation : MonoBehaviour
{

    void Start()
    {
        this.transform.localRotation = Quaternion.Euler(0, 0, Random.value*360);
    }
}
