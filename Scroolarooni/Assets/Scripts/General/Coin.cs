using UnityEngine;
using System.Collections;

public class Coin : BaseCollectible
{
    public CoinsCount coinsCount;
    public string coinsCountName;

    private void Start()
    {
        if (coinsCount == null)
        {
            coinsCount = GameObject.Find(coinsCountName).GetComponent<CoinsCount>();
        }        
    }

    public override void Collect()
    {
        if (coinsCount != null)
        {
            coinsCount.Collect();
        }
        base.Collect();
    }
}
