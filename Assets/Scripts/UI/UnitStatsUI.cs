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
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        //TODO: This works, but it updates the UI at the very end of the turn which feels a bit laggy.
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        UpdateAllUI();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e) {
        UpdateAllUI();
    }

    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e) {
        UpdateAllUI();
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e) {
        UnitActionSystem unitActionSystem = sender as UnitActionSystem;
        selectedUnit = unitActionSystem.GetSelectedUnit();
        UpdateAllUI();
    }

    private void UpdateAllUI() {
        if(UnitActionSystem.Instance.GetSelectedUnit()) {
            unitName.text = selectedUnit.name.ToString();
            UpdateHealth();
            UpdateMana();
        }
    }

    private void UpdateHealth() {
        int healthPoints = selectedUnit.GetHealth();
        int healthMax = selectedUnit.GetHealthMax();
        healthText.text = $"{healthPoints}/{healthMax} HP";
        healthBarImage.fillAmount = selectedUnit.GetHealthNormalized();
    }

    private void UpdateMana() {
        int manaPoints = selectedUnit.GetMana();
        int manaMax = selectedUnit.GetManaMax();
        manaText.text = $"{manaPoints}/{manaMax} HP";
        manaBarImage.fillAmount = selectedUnit.GetManaNormalized();
    }


}
