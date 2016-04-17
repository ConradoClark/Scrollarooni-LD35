using UnityEngine;
using System.Collections;

public class RandomZRotation : MonoBehaviour
{
    void Start()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, Random.value * 360);
    }
}
