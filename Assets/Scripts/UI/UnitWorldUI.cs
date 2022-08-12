using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour {
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private HealthSystem healthSystem;

    private void Start() {
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        UpdateHealthBar();
    }

    private void UpdateHealthBar() {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }

    private void HealthSystem_OnHealthChanged(object sender, EventArgs e) {
        UpdateHealthBar();
    }
}
