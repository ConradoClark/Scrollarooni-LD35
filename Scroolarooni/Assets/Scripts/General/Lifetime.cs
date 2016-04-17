using UnityEngine;
using System.Collections;

public class Lifetime : MonoBehaviour
{
    public float Duration;
    void Start()
    {
        this.StartCoroutine(LiveAndDie());
    }

    IEnumerator LiveAndDie()
    {
        yield return new WaitForSeconds(this.Duration);
        GameObject.Destroy(this.gameObject);
    }
}
