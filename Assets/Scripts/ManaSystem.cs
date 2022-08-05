using System;
using System.Collections.Generic;
using UnityEngine;

public class ManaSystem : MonoBehaviour {

    
    public event EventHandler OnDead;
    public event EventHandler OnDamaged;
    [SerializeField] private int mana = 100;
    private int manaMax;

    private void Awake() {
        manaMax = mana;
    }

    public void ProcessMana(int manaAmount) {
        mana -= manaAmount;
        if(mana < 0) {
            mana = 0;
        }
        //OnDamaged?.Invoke(this, EventArgs.Empty);
        
        // if (mana == 0) {
        //     Die();
        // }
    }


    public float GetManaNormalized() {
        return (float)mana / manaMax;
    }

    public int GetMana() {
        return mana;
    }

    public int GetManaMax() {
        return manaMax;
    }
}
