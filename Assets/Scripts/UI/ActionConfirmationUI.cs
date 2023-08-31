using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ActionConfirmationUI : MonoBehaviour {

    [SerializeField] TextMeshProUGUI accuracyChance;
    [SerializeField] TextMeshProUGUI damagePrediction;

    void Start() {
        UnitActionManager.Instance.OnActionChosen += UnitActionManager_OnActionChosen;
        UnitActionManager.Instance.OnActionStarted += UnitActionManager_OnActionStarted;
        UnitActionManager.Instance.OnActionCancelled += UnitActionManager_OnActionCancelled;
        gameObject.SetActive(false);
    }

    private void UnitActionManager_OnActionChosen(object sender, UnitActionManager.OnActionChosenEventArgs e) {
        gameObject.SetActive(!this.isActiveAndEnabled);
        Debug.Log(e.action.GetActionName());
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

