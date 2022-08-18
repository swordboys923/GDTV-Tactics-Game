using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour {

    //TODO: Scale my UI with screen size
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
    private CanvasGroup canvasGroup;
    private bool isCanvasActive;

    private List<ActionButtonUI> actionButtonUIList;

    private void Awake() {
        actionButtonUIList = new List<ActionButtonUI>();
        canvasGroup = GetComponent<CanvasGroup>();
        isCanvasActive = false;
    }
    private void Start() {
        UnitActionManager.Instance.OnSelectedUnitChanged += UnitActionManager_OnSelectedUnitChanged;
        UnitActionManager.Instance.OnSelectedActionChanged += UnitActionManager_OnSelectedActionChanged;
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        
        CreateUnitActionButtons();
        UpdatedSelectedVisual();
        SetActive(true);
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
    private void UnitActionManager_OnSelectedActionChanged(object sender, EventArgs e) {
        UpdatedSelectedVisual();
    }

    private void UnitActionManager_OnSelectedUnitChanged(object sender, EventArgs e) {
        CreateUnitActionButtons();
        UpdatedSelectedVisual();
        SetActive(true);
    }
    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e) {
        SetActive(false);
    }

    private void BaseAction_OnAnyActionCompleted(object sender, BaseAction.BaseActionEventArgs e) {
        if (e.actingUnit == UnitActionManager.Instance.GetSelectedUnit()) {
            SetActive(true);
        }
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e) {
        SetActive(TurnSystem.Instance.IsPlayerTurn());
    }

    private void UpdatedSelectedVisual() {
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList) {
            actionButtonUI.UpdateSelectedVisual();
        }
    }

    private void SetActive(bool isActive) {
        if (isActive == true) {
            canvasGroup.alpha = 1;
        } else {
            canvasGroup.alpha = 0;
        }
        canvasGroup.interactable = isActive;
        canvasGroup.blocksRaycasts = isActive;

        isCanvasActive = isActive;
    }

}
