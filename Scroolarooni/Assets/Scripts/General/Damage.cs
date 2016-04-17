using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour
{
    public uint InitialAmount;
    private uint amount;
    public float TickCooldown;
    uint currentAmount;
    bool damage;

    void Start()
    {
        this.currentAmount = this.InitialAmount;
        this.StartCoroutine(SetDamageAmount());
    }

    IEnumerator SetDamageAmount()
    {
        while (this.enabled)
        {
            this.amount = currentAmount;
            yield return 1;
            
            if (damage)
            {
                this.amount = 0;
                this.damage=false;
                yield return new WaitForSeconds(this.TickCooldown);
            }            
        }
    }

    public uint PeekDamageAmount()
    {
        return this.amount;
    }

    public uint GetDamage()
    {
        damage = true;
        return this.amount;
    }

    public void SetDamage(uint amount)
    {
        this.currentAmount = this.InitialAmount =  amount;
    }
}
