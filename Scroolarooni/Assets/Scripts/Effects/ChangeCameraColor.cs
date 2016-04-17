using UnityEngine;
using System.Collections;

public class ChangeCameraColor : ColorChangeBehaviour
{
    public Camera cam;
    bool changing;
    Coroutine changingCoroutine;
    private Color currentColor;

    void Start()
    {
        this.currentColor = cam.backgroundColor;  
    }

    public override void ChangeColors(Color color)
    {
        if (changing)
        {
            StopCoroutine(changingCoroutine);
        }
        changingCoroutine = StartCoroutine(StartChangingColors(color));
    }

    IEnumerator StartChangingColors(Color color)
    {
        changing = true;

        float timeScale = 0f;
        do
        {
            cam.backgroundColor = Color.Lerp(this.currentColor, color, timeScale);
            timeScale += Mathf.Pow(Time.smoothDeltaTime, 2) * 100f;
            yield return 1;
            yield return 1;
        }
        while (timeScale < 1.0);

        currentColor = color;
        changing = false;
        yield break;
    }

}
