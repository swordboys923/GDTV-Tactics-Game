using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

    public static TurnManager Instance { get; private set; }
    public event EventHandler OnTurnChanged;
    private int turnNumber = 1;
    private bool isPlayerTurn = true;
    private List<Unit> turnOrderList;
    
    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There's more than one TurnManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance  = this;
    }

    private void Start() {
        GenerateTurnList();
    }

    private void GenerateTurnList() {
        turnOrderList.Clear();
        List<Unit> turnOrderArray = UnitManager.Instance.GetUnitList();
        turnOrderList = turnOrderArray.OrderByDescending(t=> t.GetStaminaNormalized()).ToList();
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

    public List<Unit> GetTurnOrderList() {
        return turnOrderList;
    }
    
    //TODO: Should cycle through the turn list backwards. That way we can pop an item off of the list when they are done with their turn.
    //TODO: Should we keep two lists? One for current turn and one for next? That way if the are any status changes, we are projecting off of the second list without damanging the current list?
}
