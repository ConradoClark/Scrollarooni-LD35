using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class ChangeAllChildrenColors : ColorChangeBehaviour
{
    public SpriteRenderer[] extraSpriteRenderers;
    bool changing;
    bool stop = false;
    Coroutine changingCoroutine;
    public Color currentColor;

    void Start()
    {

    }

    public override void ChangeColors(Color color)
    {
        if (changing)
        {
            stop = true;
        }
        changingCoroutine = StartCoroutine(StartChangingColors(color));
    }

    IEnumerator StartChangingColors(Color color)
    {
        while (this.stop)
        {
            yield return 1;
        }

        changing = true;
        if (extraSpriteRenderers == null) extraSpriteRenderers = new SpriteRenderer[0];
        SpriteRenderer[] renderers = this.GetComponentsInChildren<SpriteRenderer>().Concat(extraSpriteRenderers).ToArray();

        Dictionary<SpriteRenderer,ColorFlicker> effects = new Dictionary<SpriteRenderer, ColorFlicker>();
        ColorFlicker[] extraEffects = this.GetComponentsInChildren<ColorFlicker>().Concat(this.GetComponents<ColorFlicker>()).ToArray();

        foreach (var renderer in renderers)
        {
            if (effects.ContainsKey(renderer)) continue;
            var flicker = renderer.gameObject.AddComponent<ColorFlicker>();
            flicker.sprRenderer = renderer;
            flicker.magnitude = 0.2f;
            effects[renderer] = flicker;
        }

        float timeScale = 0f;
        do
        {
            if (this.stop) break;
            foreach (var renderer in renderers)
            {
                renderer.color = Color.Lerp(this.currentColor, color, timeScale);
                effects[renderer].ChangeOriginalColor(renderer.color);
            }
            timeScale += Mathf.Pow(Time.smoothDeltaTime, 2)*100f;
            yield return 1;
            yield return 1;
        }
        while (timeScale < 1.0);

        foreach (var flicker in effects.Values)
        {
            GameObject.Destroy(flicker);
        }

        foreach (var flicker in extraEffects)
        {
            flicker.ChangeOriginalColor(color);
        }

        foreach (var renderer in renderers)
        {
            renderer.color = color;
        }

        currentColor = color;
        stop = false;
        changing = false;
        yield break;
    }
}
