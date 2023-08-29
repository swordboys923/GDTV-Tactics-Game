using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour {

    public static event EventHandler OnAnyHealthChanged;
    public event EventHandler OnDead;
    public event EventHandler<OnHealthChangedEventArgs> OnHealthChanged;

    public class OnHealthChangedEventArgs : EventArgs {
        public int changeAmount;
    }
    [SerializeField] private int health = 100;
    private int healthMax;

    private void Awake() {
        healthMax = health;
    }

    public void ProcessHealthChange(int healthChangeAmount) {
        health -= healthChangeAmount;
        if(health < 0) {
            health = 0;
        }
        if(health > healthMax) {
            health = healthMax;
        }
        OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs{
            changeAmount = healthChangeAmount
        });
        OnAnyHealthChanged?.Invoke(this,EventArgs.Empty);
        
        if (health == 0) {
            Die();
        }
    }

    private void Die() {
        OnDead?.Invoke(this,EventArgs.Empty);
    }

    public float GetHealthNormalized() {
        return (float)health / healthMax;
    }

    public int GetHealth() {
        return health;
    }

    public int GetHealthMax() {
        return healthMax;
    }
}
