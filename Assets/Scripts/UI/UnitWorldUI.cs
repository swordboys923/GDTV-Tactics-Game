using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitWorldUI : MonoBehaviour {
    [SerializeField] private Image healthBarImage;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private TextMeshPro healthNumberText;
    private Transform healthNumberTextTransform;
    private bool isActive;

    private void Start() {
        healthNumberTextTransform = healthNumberText.transform;
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        UpdateHealthBar();
    }

    private void UpdateHealthBar() {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }

    private void HealthSystem_OnHealthChanged(object sender, EventArgs e) {
        UpdateHealthBar();
    }

    private void DisplayHealthNumbers(int amount) {
        isActive = true;
    }
}
