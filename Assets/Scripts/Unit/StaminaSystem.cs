using System;

using UnityEngine;

public class StaminaSystem : MonoBehaviour{

    public static event EventHandler OnAnyStaminaChanged;
    public event EventHandler OnStaminaDrained;
    public event EventHandler OnStaminaChanged;
    [SerializeField] private int stamina = 100;
    private int staminaMax;

    private void Awake() {
        staminaMax = stamina;
    }

    public void ProcessStaminaChange(int lossAmount) {
        stamina -= lossAmount;
        if(stamina < 0) {
            stamina = 0;
        }
        if(stamina > staminaMax) {
            stamina = staminaMax;
        }
        OnStaminaChanged?.Invoke(this, EventArgs.Empty);
        // OnAnyStaminaChanged?.Invoke(this,EventArgs.Empty);
        
        if (stamina == 0) {
            Exhaust();
        }
    }

    private void Exhaust() {
        OnStaminaDrained?.Invoke(this,EventArgs.Empty);
    }

    public float GetStaminaNormalized() {
        return (float)stamina / staminaMax;
    }

    public int GetStamina() {
        return stamina;
    }

    public int GetStaminaMax() {
        return staminaMax;
    }


}
