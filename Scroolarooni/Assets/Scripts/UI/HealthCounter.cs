using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthCounter : MonoBehaviour
{
    public Health Health;
    public Material Material;
    public Color Color;
    public Sprite EmptySprite;
    public Sprite FullSprite;
    public Vector2 Offset;
    public int orderingLayer;

    int currentHealth;
    int currentMaximumHealth;    

    private List<SpriteRenderer> healthBars;

    private void Start()
    {
        healthBars = new List<SpriteRenderer>();

        this.currentHealth = this.Health.GetCurrentHealth();
        this.currentMaximumHealth = this.Health.MaxHealth;
        this.CreateHearts();
    }

    private void CreateHearts()
    {
        for (int i = 0; i < currentMaximumHealth; i++)
        {
            GameObject healthBarTick = new GameObject("tick " + i);
            SpriteRenderer sprRenderer = healthBarTick.AddComponent<SpriteRenderer>();
            sprRenderer.sprite = this.FullSprite;
            sprRenderer.material = this.Material;
            sprRenderer.color = this.Color;
            sprRenderer.sortingOrder = orderingLayer;
            healthBarTick.transform.SetParent(this.transform, false);
            healthBarTick.transform.localPosition = new Vector3( i * (this.FullSprite.bounds.size.x+ Offset.x), Offset.y);
            healthBars.Add(sprRenderer);
        }
    }

    private void Update()
    {
        int realHealth = this.Health.GetCurrentHealth();
        if (this.currentHealth == realHealth) return;

        this.currentHealth = realHealth;
        bool full=false;
        for (int i = this.currentMaximumHealth-1; i >= 0; i--)
        {
            if (i+1 == this.currentHealth) full = true;
            healthBars[i].sprite = full ? FullSprite : EmptySprite;
            healthBars[i].color = full ? this.Color : Color.white;
        }

    }
}
