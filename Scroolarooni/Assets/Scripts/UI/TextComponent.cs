using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class TextComponent : MonoBehaviour
{
    public RectTransform rectTransform;
    public string Text;
    public string Font;
    public Material material;
    public int orderingLayer;

    private string innerText;
    private Dictionary<string, Sprite> fontSprites;
    private List<SpriteRenderer> glyphs;

    private void Start()
    {
        var allGlyphs = Resources.LoadAll<Sprite>("Sprites/" + Font);
        this.fontSprites = Translate(allGlyphs);
        this.glyphs = new List<SpriteRenderer>();
    }

    private void Update()
    {
        if (Text != innerText)
        {
            innerText = Text;
            RenderText();
        }
    }

    private void RenderText()
    {
        var firstChar = this.fontSprites.Values.FirstOrDefault();
        if (firstChar == null) return;

        int columns = Mathf.FloorToInt(Mathf.Max(1f, rectTransform.sizeDelta.x / firstChar.bounds.size.x));
        int rows = Mathf.FloorToInt(Mathf.Max(1f, rectTransform.sizeDelta.y / firstChar.bounds.size.y));

        for (int i = 0; i < glyphs.Count; i++)
        {
            GameObject.Destroy(glyphs[i].gameObject);
        }
        glyphs.Clear();

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                int currentIndex = c + r / columns;
                if (currentIndex >= innerText.Length) return; // text finished

                string currentChar = innerText[currentIndex].ToString().ToUpper();
                GameObject character = new GameObject(string.Format("(x{0},y{1}) - {2}", c, r, currentChar));
                SpriteRenderer charRenderer = character.AddComponent<SpriteRenderer>();
                charRenderer.sprite = fontSprites[currentChar];
                charRenderer.material = material;
                charRenderer.sortingOrder = orderingLayer;
                character.transform.SetParent(this.transform, false);
                character.transform.localPosition = new Vector3(c * charRenderer.sprite.bounds.size.x, r * charRenderer.sprite.bounds.size.y);
                glyphs.Add(charRenderer);
            }
        }
    }

    private Dictionary<string, Sprite> Translate(Sprite[] sprites)
    {
        Dictionary<string, Sprite> dict = new Dictionary<string, Sprite>();
        for (int i = 0; i < sprites.Length; i++)
        {
            Sprite spr = sprites[i];
            if (spr.name.StartsWith("Num") || spr.name.StartsWith("Sym"))
            {
                dict[ConvertSymbolToChar(spr.name).ToString()] = spr;
            }
            dict[spr.name] = spr;
        }
        return dict;
    }

    private char ConvertSymbolToChar(string symbol)
    {
        if (symbol.StartsWith("Num_")) return symbol.LastOrDefault();

        switch (symbol)
        {
            // add others
            case "Sym_Exclamation": return '!';
            case "Sym_Slash": return '/';
            case "Sym_Space": return ' ';
            default: return ' ';
        }
    }
}
