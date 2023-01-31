using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//TODO: Keep working on this script;
public class UnitStatsUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private Image manaBarImage;
    [SerializeField] private TextMeshProUGUI experienceText;
    [SerializeField] private TextMeshProUGUI unitName;

    private Unit selectedUnit;

    private void Start() {
        UnitActionManager.Instance.OnSelectedUnitChanged += UnitActionManager_OnSelectedUnitChanged;
        HealthSystem.OnAnyHealthChanged += HealthSystem_OnAnyHealthChanged;
        ActionResourceSystem.OnAnyResourceChanged += ActionResourceSystem_OnAnyResourceChanged;
        TurnManager.Instance.OnTurnChanged += TurnManager_OnTurnChanged;
        selectedUnit = UnitActionManager.Instance.GetSelectedUnit();
        UpdateAllUI();
    }

    private void TurnManager_OnTurnChanged(object sender, EventArgs e) {
        UpdateAllUI();
    }

    private void HealthSystem_OnAnyHealthChanged(object sender, EventArgs e) {
        UpdateAllUI();
    }

    private void ActionResourceSystem_OnAnyResourceChanged(object sender, EventArgs e){
        UpdateAllUI();
    }

    private void UnitActionManager_OnSelectedUnitChanged(object sender, EventArgs e) {
        UnitActionManager unitActionManager = sender as UnitActionManager;
        selectedUnit = unitActionManager.GetSelectedUnit();
        UpdateAllUI();
    }

    private void UpdateAllUI() {
        if(UnitActionManager.Instance.GetSelectedUnit()) {
            unitName.text = selectedUnit.name.ToString();
            UpdateHealth();
            UpdateResource();
        }
    }

    private void UpdateHealth() {
        int healthPoints = selectedUnit.GetHealth();
        int healthMax = selectedUnit.GetHealthMax();
        healthText.text = $"{healthPoints}/{healthMax} HP";
        healthBarImage.fillAmount = selectedUnit.GetHealthNormalized();
    }

    private void UpdateResource() {
        int manaPoints = selectedUnit.GetResource();
        int manaMax = selectedUnit.GetResourceMax();
        manaText.text = $"{manaPoints}/{manaMax} HP";
        manaBarImage.fillAmount = selectedUnit.GetResourceNormalized();
    }


}
