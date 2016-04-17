using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour
{
    public TextComponent textComponent;
    public int CurrentScore { get; private set; }
    public int Digits;

    private void Start()
    {
        this.SetScore();
    }

    private void SetScore()
    {
        textComponent.Text = this.CurrentScore.ToString().PadLeft(7, '0');
    }

    public void Increase(uint amount)
    {
        this.CurrentScore += (int) amount;
        this.SetScore();
    }
}
