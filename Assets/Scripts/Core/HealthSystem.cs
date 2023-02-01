using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tactics.Core{
public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    [SerializeField] private int health = 100;
    private int healthMax;
    public event EventHandler OnDamaged;

    private void Awake() {
        healthMax = health;
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;

        if(health < 0)
        {
            health = 0;
        }

        OnDamaged?.Invoke(this, EventArgs.Empty);

        if(health == 0)
        {
            Die();
        }
        Debug.Log(health);
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return (float)health / healthMax;
    }

}
}
