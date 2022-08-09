using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour {

    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
    [SerializeField] private TextMeshProUGUI actionPointsText;

    private List<ActionButtonUI> actionButtonUIList;

    private void Awake() {
        actionButtonUIList = new List<ActionButtonUI>();
    }
    private void Start() {
        UnitActionManager.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionManager.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionManager.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyOnAnyActionPointsChanged;
        
        UpdateActionPoints();
        CreateUnitActionButtons();
        UpdatedSelectedVisual();
    }


    private void CreateUnitActionButtons() {
        foreach(Transform buttonTransform in actionButtonContainerTransform) {
            Destroy(buttonTransform.gameObject);
        }

        actionButtonUIList.Clear();

        Unit selectedUnit = UnitActionManager.Instance.GetSelectedUnit();
        foreach(BaseAction baseAction in selectedUnit.GetBaseActionArray()) {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);

            actionButtonUIList.Add(actionButtonUI);
        }
    }
    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdatedSelectedVisual();
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e) {
        CreateUnitActionButtons();
        UpdatedSelectedVisual();
        UpdateActionPoints();
    }

    private void UnitActionSystem_OnActionStarted(object sender, EventArgs e) {
        UpdateActionPoints();
    }
    
    private void TurnSystem_OnTurnChanged(object sender, EventArgs e) {
        UpdateActionPoints();
    }

    private void Unit_OnAnyOnAnyActionPointsChanged(object sender, EventArgs e) {
        UpdateActionPoints();
    }

    private void UpdatedSelectedVisual() {
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList) {
            actionButtonUI.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPoints() {
        Unit selectedUnit = UnitActionManager.Instance.GetSelectedUnit();
        int actionPoints = selectedUnit.GetActionPoints();
        actionPointsText.text = $"Action Points: {actionPoints}";
    }
}
