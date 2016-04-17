using UnityEngine;
using System.Collections;

public class CoinsCount : MonoBehaviour
{
    public int coinsInLevel;
    private int collectedCoins;
    public TextComponent textComponent;

    void Start()
    {
        SetText();
    }

    public void Collect()
    {
        collectedCoins++;
        SetText();
    }

    private void SetText()
    {
        this.textComponent.Text = collectedCoins.ToString().PadLeft(2, '0') + "/" + coinsInLevel.ToString().PadLeft(2, '0');
    }
}
