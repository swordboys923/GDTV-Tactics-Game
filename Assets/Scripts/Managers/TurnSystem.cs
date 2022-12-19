using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO: Change to TurnManager
public class TurnSystem : MonoBehaviour {

    public static TurnSystem Instance { get; private set; }
    public event EventHandler OnTurnChanged;
    private int turnNumber = 1;
    private bool isPlayerTurn = true;
    
    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There's more than one TurnSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance  = this;
    }

    //TODO: not an error in this script, however if there are no more friendly units at the start of a new turn, error gets thrown.
    public void NextTurn() {
        turnNumber++;
        isPlayerTurn = !isPlayerTurn;
        OnTurnChanged?.Invoke(this,EventArgs.Empty);
    }

    public int GetTurnNumber() {
        return turnNumber;
    }

    public bool IsPlayerTurn() {
        return isPlayerTurn;
    }
}
