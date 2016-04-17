using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public int MaxHealth;
    private int currentHealth;

    private void Start()
    {
        this.currentHealth = this.MaxHealth;
    }

    public void Hurt(uint amount)
    {
        currentHealth = System.Math.Max(0, currentHealth - (int) amount);
    }

    public void Heal(uint amount)
    {
        currentHealth = System.Math.Min(this.MaxHealth, currentHealth + (int)amount);
    }

    public int GetCurrentHealth()
    {
        return this.currentHealth;
    }
}
