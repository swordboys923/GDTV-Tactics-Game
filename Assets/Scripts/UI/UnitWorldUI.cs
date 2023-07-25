using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour {
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Image staminaImage;
    [SerializeField] private Color[] staminaImageColorArray;
    [SerializeField] private StaminaSystem staminaSystem;
    int staminaIndex = 0;

    private void Start() {
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        staminaSystem.OnStaminaChanged += StaminaSystem_OnStaminaChanged;

        UpdateHealthBar();
        staminaIndex = 0;
        staminaImage.color = staminaImageColorArray[staminaIndex];
    }

    private void UpdateHealthBar() {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }

    private void HealthSystem_OnHealthChanged(object sender, EventArgs e) {
        UpdateHealthBar();
    }

    public void StaminaSystem_OnStaminaChanged(object sender, EventArgs e) {
        if (staminaIndex == 2) return; //TODO: Fix this
        staminaIndex++;
        staminaImage.color = staminaImageColorArray[staminaIndex];
    }
}
