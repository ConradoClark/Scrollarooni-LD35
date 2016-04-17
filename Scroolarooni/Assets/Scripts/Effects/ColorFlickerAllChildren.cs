using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorFlickerAllChildren : MonoBehaviour
{
    public float magnitude;
    public Dictionary<SpriteRenderer,Color> sprRenderers;

    void Start()
    {
        sprRenderers = new Dictionary<SpriteRenderer, Color>();
    }

    void Update()
    {
        var collectedSpriteRenderers = this.GetComponentsInChildren<SpriteRenderer>();
        foreach(var sprRenderer in collectedSpriteRenderers)
        {
            if (!sprRenderers.ContainsKey(sprRenderer))
            {
                sprRenderers[sprRenderer] = sprRenderer.color;
            }
        }

        foreach(var kvp in sprRenderers)
        {
            var sprRenderer = kvp.Key;
            sprRenderer.color = kvp.Value + kvp.Value * new Color(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * magnitude;
        }
    }
}
