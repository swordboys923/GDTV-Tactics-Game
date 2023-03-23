using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TurnManagerUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI turnNumberText;

    private void Start() {
        TurnManager.Instance.OnTurnChanged += TurnManager_OnTurnChanged;
        UpdateTurnText();
    }

    private void TurnManager_OnTurnChanged(object sender, EventArgs e) {
        UpdateTurnText();
    }
    private void UpdateTurnText() {
        int turnNumber = TurnManager.Instance.GetTurnNumber();
        turnNumberText.text = $"TURN {turnNumber}";
    }

}
