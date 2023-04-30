// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class UnitActionSystemUI : MonoBehaviour {

//     //TODO: Scale my UI with screen size
//     [SerializeField] private Transform actionButtonPrefab;
//     [SerializeField] private Transform actionButtonContainerTransform;
//     private CanvasGroup canvasGroup;
//     private bool isCanvasActive;

//     private List<ActionButtonUI> actionButtonUIList;

//     private void Awake() {
//         actionButtonUIList = new List<ActionButtonUI>();
//         canvasGroup = GetComponent<CanvasGroup>();
//         isCanvasActive = false;
//     }
    
//     private void Start() {
//         UnitActionManager.Instance.OnSelectedUnitChanged += UnitActionManager_OnSelectedUnitChanged;
//         UnitActionManager.Instance.OnSelectedActionChanged += UnitActionManager_OnSelectedActionChanged;
//         BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
//         BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;
//         TurnManager.Instance.OnUnitTurnChanged += TurnManager_OnUnitTurnChanged;

        
//         CreateUnitActionButtons();
//     }

//     private void CreateUnitActionButtons() {
//         //TODO: So here, I need to set Move, Attack, then put the special abilities into a separate context menu, then Items?, then Wait.
//         // instead of placing all of the actions directly into a single list.

//         foreach(Transform buttonTransform in actionButtonContainerTransform) {
//             Destroy(buttonTransform.gameObject);
//         }

//         actionButtonUIList.Clear();
//         Unit currentTurnUnit = TurnManager.Instance.GetCurrentTurnUnit();
//         CreateUnitMoveButton(currentTurnUnit);
//         CreateUnitAttackButton(currentTurnUnit);
//         CreateUnitSpecialActionButtons(currentTurnUnit);
//         CreateUnitWaitButton(currentTurnUnit);
//         UpdatedSelectedVisual();
//         SetActive(true);
//     }

//     private void CreateUnitMoveButton(Unit currentTurnUnit) {
//         BaseAction moveAction = currentTurnUnit.GetMoveAction();
//         CreateButton(moveAction);
//     }

//     private void CreateUnitAttackButton(Unit currentTurnUnit) {
//         BaseAction attackAction = currentTurnUnit.GetAttackAction();
//         CreateButton(attackAction);
//     }

//     private void CreateUnitSpecialActionButtons(Unit currentTurnUnit) {
//         foreach(BaseAction baseAction in currentTurnUnit.GetSpecialActionArray()) {
//             CreateButton(baseAction);
//         }
//     }

//     private void CreateUnitWaitButton (Unit currentTurnUnit) {
//         BaseAction waitAction = currentTurnUnit.GetWaitAction();
//         CreateButton(waitAction);
//     }

//     private void CreateButton(BaseAction baseAction) {
//         Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
//         ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
//         actionButtonUI.SetBaseAction(baseAction);
//         actionButtonUIList.Add(actionButtonUI);
//     }
//     private void UnitActionManager_OnSelectedActionChanged(object sender, EventArgs e) {
//         UpdatedSelectedVisual();
//     }

//     private void UnitActionManager_OnSelectedUnitChanged(object sender, EventArgs e) {
//         CreateUnitActionButtons();
//     }

//     private void BaseAction_OnAnyActionStarted(object sender, EventArgs e) {
//         SetActive(false);
//     }

//     private void BaseAction_OnAnyActionCompleted(object sender, BaseAction.BaseActionEventArgs e) {
//         if (e.actingUnit == TurnManager.Instance.GetCurrentTurnUnit()) {
//             SetActive(true);
//         }
//     }

//     private void TurnManager_OnUnitTurnChanged(object sender, TurnManager.OnUnitTurnChangedEventArgs e) {
//         CreateUnitActionButtons();
//         SetActive(TurnManager.Instance.IsPlayerTurn());
//     }

//     private void UpdatedSelectedVisual() {
//         foreach (ActionButtonUI actionButtonUI in actionButtonUIList) {
//             actionButtonUI.UpdateSelectedVisual();
//         }
//     }

//     private void SetActive(bool isActive) {
//         if (isActive == true) {
//             canvasGroup.alpha = 1;
//         } else {
//             canvasGroup.alpha = 0;
//         }
//         canvasGroup.interactable = isActive;
//         canvasGroup.blocksRaycasts = isActive;

//         isCanvasActive = isActive;
//     }

// }
