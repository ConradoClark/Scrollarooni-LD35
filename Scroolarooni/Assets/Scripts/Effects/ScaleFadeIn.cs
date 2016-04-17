using UnityEngine;
using System.Collections;

public class ScaleFadeIn : MonoBehaviour
{
    public float timeToFadeIn;
    private Vector3 scale;

    void Start()
    {
        this.scale = this.transform.localScale;
        this.StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < timeToFadeIn)
        {
            this.transform.localScale = Vector3.Lerp(Vector3.zero, scale, elapsedTime * elapsedTime / timeToFadeIn);
            elapsedTime += Time.deltaTime;
            yield return 1;
        }
    }
}
