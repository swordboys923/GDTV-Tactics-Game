using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TurnManagerUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI turnNumberText;
    [SerializeField] private TextMeshProUGUI unitTurnOrder;

    private void Start() {
        TurnManager.Instance.OnTurnChanged += TurnManager_OnTurnChanged;
        TurnManager.Instance.OnUnitTurnChanged += TurnManager_OnUnitTurnChanged;
        UpdateTurnText();
    }

    private void TurnManager_OnTurnChanged(object sender, EventArgs e) {
        UpdateTurnText();
    }
    private void UpdateTurnText() {
        int turnNumber = TurnManager.Instance.GetTurnNumber();
        turnNumberText.text = $"TURN {turnNumber}";
    }

    private void TurnManager_OnUnitTurnChanged(object sender, EventArgs e) {
        string text = "";
        int orderNumber = 1;
        foreach(Unit unit in TurnManager.Instance.GetTurnOrderList()) {
            text += orderNumber + " " + unit.name + "\n";
            orderNumber++;
        }
        text += "--End Turn--";
        unitTurnOrder.text = text;
    }

}
