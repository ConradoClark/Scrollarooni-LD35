using UnityEngine;
using System.Collections;

public class ColorFlicker : MonoBehaviour
{
    public float magnitude;
    public SpriteRenderer sprRenderer;

    Color originalColor;
    void Start()
    {
        this.originalColor = sprRenderer.color;
    }

    void Update()
    {
        sprRenderer.color = this.originalColor + this.originalColor * new Color(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * magnitude;
    }

    public void ChangeOriginalColor(Color color)
    {
        this.originalColor = color;
    }
}
