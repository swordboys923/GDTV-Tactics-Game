using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ActionConfirmationUI : MonoBehaviour {

    [SerializeField] TextMeshProUGUI accuracyChance;
    [SerializeField] TextMeshProUGUI damagePrediction;
    [SerializeField] TextMeshProUGUI actionConfirmText;
    [SerializeField] Button confirmButton;
    [SerializeField] Button declineButton;

    void Start() {
        UnitActionManager.Instance.OnActionChosen += UnitActionManager_OnActionChosen;
        UnitActionManager.Instance.OnActionStarted += UnitActionManager_OnActionStarted;
        UnitActionManager.Instance.OnActionCancelled += UnitActionManager_OnActionCancelled;
        confirmButton.onClick.AddListener(() => {
            UnitActionManager.Instance.TakeAction();
        });
        declineButton.onClick.AddListener(() =>{
            UnitActionManager.Instance.DeclineAction();
        });
        gameObject.SetActive(false);
    }

    private void UnitActionManager_OnActionChosen(object sender, UnitActionManager.OnActionChosenEventArgs e) {
        gameObject.SetActive(!this.isActiveAndEnabled);
        if(e.action is MoveAction) {
            actionConfirmText.text = "Move here?";
            accuracyChance.text = "";
            damagePrediction.text = "";
            return;
        }
        if(e.action is WaitAction) {
            actionConfirmText.text = "Wait here?";
            accuracyChance.text = "";
            damagePrediction.text = "";
            return;
        }
        actionConfirmText.text = $"Perform {e.action.GetActionName()}?";
        accuracyChance.text = $"Chance to hit: {e.action.GetPercentToHit()}";
        damagePrediction.text = $"Damage: {e.action.GetDamageRange().Item1} - {e.action.GetDamageRange().Item2}";
    }

    private void UnitActionManager_OnActionStarted(object sender, EventArgs e) {
        gameObject.SetActive(false);
    }

    private void UnitActionManager_OnActionCancelled(object sender, EventArgs e) {
        gameObject.SetActive(false);
    }
}


