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

    private Unit currentTurnUnit;

    private void Start() {
        // UnitActionManager.Instance.OnSelectedUnitChanged += UnitActionManager_OnSelectedUnitChanged;
        HealthSystem.OnAnyHealthChanged += HealthSystem_OnAnyHealthChanged;
        ActionResourceSystem.OnAnyResourceChanged += ActionResourceSystem_OnAnyResourceChanged;
        TurnManager.Instance.OnTurnChanged += TurnManager_OnTurnChanged;
        TurnManager.Instance.OnUnitTurnChanged += TurnManager_OnUnitTurnChanged;
        currentTurnUnit = TurnManager.Instance.GetCurrentTurnUnit();
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


    // private void UnitActionManager_OnSelectedUnitChanged(object sender, EventArgs e) {
    //     UnitActionManager unitActionManager = sender as UnitActionManager;
    //     currentTurnUnit = unitActionManager.GetSelectedUnit();
    //     UpdateAllUI();
    // }

    private void TurnManager_OnUnitTurnChanged(object sender, EventArgs e) {
        currentTurnUnit = TurnManager.Instance.GetCurrentTurnUnit();
        UpdateAllUI();
    }

    private void UpdateAllUI() {
        if(TurnManager.Instance.GetCurrentTurnUnit()) {
            unitName.text = currentTurnUnit.name.ToString();
            UpdateHealth();
            UpdateResource();
        }
    }

    private void UpdateHealth() {
        int healthPoints = currentTurnUnit.GetHealth();
        int healthMax = currentTurnUnit.GetHealthMax();
        healthText.text = $"{healthPoints}/{healthMax} HP";
        healthBarImage.fillAmount = currentTurnUnit.GetHealthNormalized();
    }

    private void UpdateResource() {
        int manaPoints = currentTurnUnit.GetResource();
        int manaMax = currentTurnUnit.GetResourceMax();
        manaText.text = $"{manaPoints}/{manaMax} HP";
        manaBarImage.fillAmount = currentTurnUnit.GetResourceNormalized();
    }


}
