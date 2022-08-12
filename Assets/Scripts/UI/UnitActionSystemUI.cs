using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour {

    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;

    private List<ActionButtonUI> actionButtonUIList;

    private void Awake() {
        actionButtonUIList = new List<ActionButtonUI>();
    }
    private void Start() {
        UnitActionManager.Instance.OnSelectedUnitChanged += UnitActionManager_OnSelectedUnitChanged;
        UnitActionManager.Instance.OnSelectedActionChanged += UnitActionManager_OnSelectedActionChanged;
        
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
    private void UnitActionManager_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdatedSelectedVisual();
    }

    private void UnitActionManager_OnSelectedUnitChanged(object sender, EventArgs e) {
        CreateUnitActionButtons();
        UpdatedSelectedVisual();
    }

    private void UpdatedSelectedVisual() {
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList) {
            actionButtonUI.UpdateSelectedVisual();
        }
    }
}
