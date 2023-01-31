using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TurnManagerUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI turnNumberText;
    [SerializeField] private Button endTurnButton;

    private void Start() {
        endTurnButton.onClick.AddListener(() => {
            TurnManager.Instance.NextTurn();
        });
        
        TurnManager.Instance.OnTurnChanged += TurnManager_OnTurnChanged;
        UpdateTurnText();
        UpdateEndTurnButtonVisibility();
    }

    private void TurnManager_OnTurnChanged(object sender, EventArgs e) {
        UpdateTurnText();
        UpdateEndTurnButtonVisibility();
    }
    private void UpdateTurnText() {
        int turnNumber = TurnManager.Instance.GetTurnNumber();
        turnNumberText.text = $"TURN {turnNumber}";
    }


    private void UpdateEndTurnButtonVisibility() {
        endTurnButton.gameObject.SetActive(TurnManager.Instance.IsPlayerTurn());
    }
}
