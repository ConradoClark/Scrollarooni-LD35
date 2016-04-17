using UnityEngine;
using System.Collections;

public class ScaleFadeOut : MonoBehaviour
{
    public float timeToFadeOut;

    void Start()
    {
        this.StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        var initialScale = this.transform.localScale;
        while (elapsedTime < timeToFadeOut)
        {
            this.transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, elapsedTime * elapsedTime / timeToFadeOut);
            elapsedTime += Time.deltaTime;
            yield return 1;
        }
    }
}
