using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;

public class UnitWorldUI : MonoBehaviour {
    [SerializeField] private Image healthBarImage;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private PopupNumber healthNumberTextPrefab;
    [SerializeField] private Transform healthNumberTextTransform;

    private void Start() {
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        UpdateHealthBar();
    }

    private void UpdateHealthBar() {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }

    private void HealthSystem_OnHealthChanged(object sender, EventArgs e) {
        UpdateHealthBar();
        DisplayHealthNumbers(30);
    }

    private void DisplayHealthNumbers(int amount) {
        PopupNumber popupNumber = Instantiate(healthNumberTextPrefab,healthNumberTextTransform.position, quaternion.identity);
        popupNumber.Setup(amount);
    }
}
