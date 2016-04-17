using UnityEngine;
using System.Collections;

public class TiledSprite : MonoBehaviour
{
    public RectTransform transform;
    public Sprite sprite;
    public Material material;
    public Color color;

    void Start()
    {
        int fitSpritesX = Mathf.CeilToInt(transform.sizeDelta.x / sprite.bounds.size.x)+1;
        int fitSpritesY = Mathf.CeilToInt(transform.sizeDelta.y / sprite.bounds.size.y)+1;
        for (int i = 0; i < fitSpritesY; i++)
        {
            for (int j = 0; j < fitSpritesX; j++)
            {
                GameObject render = new GameObject(string.Format("Renderer X{0}-Y{1}", i, j));
                SpriteRenderer renderer = render.AddComponent<SpriteRenderer>();
                renderer.sprite = this.sprite;
                renderer.material = this.material;
                renderer.color = this.color;

                render.transform.SetParent(this.transform, false);
                render.transform.localPosition = new Vector3(j * sprite.bounds.size.x - (sprite.bounds.size.x / 2) - (transform.sizeDelta.x/2), i * sprite.bounds.size.y - (sprite.bounds.size.y/2) - (transform.sizeDelta.y / 2));
            }
        }
    }

    void Update()
    {

    }
}
