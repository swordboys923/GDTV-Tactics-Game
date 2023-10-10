using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

//TODO: StateManager


public class TurnManager : MonoBehaviour {

    public static TurnManager Instance { get; private set; }
    public event EventHandler OnTurnChanged;
    public event EventHandler<OnUnitTurnChangedEventArgs> OnUnitTurnChanged;
    public class OnUnitTurnChangedEventArgs : EventArgs {
        public Unit currentTurnUnit;
    }
    [SerializeField] private Unit currentTurnUnit;
    private List<Unit> turnOrderList;

    private int turnNumber = 1;

    private float stateTimer;
    
    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There's more than one TurnManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance  = this;
    }


    private void Start() {
        turnOrderList = GenerateTurnList();
        SetNextCurrentTurnUnit();
    }

    private void OnEnable() {
        WaitAction.OnAnyWait += WaitAction_OnAnyWait;
        RoutingAI.OnAnyRoutingComplete += RoutingAI_OnAnyRoutingComplete;
    }


    public List<Unit> GetTurnOrderList() {
        return turnOrderList;
    }

    public void AddUnitToTurnList(Unit unit) {
        turnOrderList.Add(unit);
    }

    public Unit GetCurrentTurnUnit() {
        return currentTurnUnit;
    }

    public void AddUnitsToTurnList(Unit[] units){
        foreach(Unit unit in units) {
            turnOrderList.Add(unit);
        }
    }

    public bool IsUnitTurnActive(Unit unit) {
        return unit == turnOrderList[0];
    }

    private List<Unit> GenerateTurnList() {
        List<Unit> unitList = UnitManager.Instance.GetUnitList();
        return unitList.OrderByDescending(t=> t.GetStaminaNormalized()).ToList();
    }

    private void RemoveUnitFromTurnList(Unit unit) {
        turnOrderList.Remove(unit);
    }

    private void SetNextCurrentTurnUnit () {
        RemoveUnitFromTurnList(currentTurnUnit);
        if (turnOrderList.Count() <= 0) {
            NextTurn();
            turnOrderList = GenerateTurnList();
        }
        currentTurnUnit = turnOrderList[0];
        OnUnitTurnChanged?.Invoke(this, new OnUnitTurnChangedEventArgs {
            currentTurnUnit = currentTurnUnit,
        });
    }


    
    //TODO: Should cycle through the turn list backwards. That way we can pop an item off of the list when they are done with their turn.
    //TODO: Should we keep two lists? One for current turn and one for next? That way if the are any status changes, we are projecting off of the second list without damanging the current list?

    private void WaitAction_OnAnyWait(object sender, WaitAction.OnAnyWaitEventArgs e) {
        if(e.unit != currentTurnUnit) {
            Debug.LogError($"Wait Action Received from {e.unit}; however currentTurnUnit is {currentTurnUnit}.");
            return;
        }

        SetNextCurrentTurnUnit();
    }

    private void RoutingAI_OnAnyRoutingComplete(object sender, EventArgs e) {
        SetNextCurrentTurnUnit();
    }


    //OLD COURSE CODE
    //BUG: not an error in this script, however if there are no more friendly units at the start of a new turn, the enemy takes thousands of turns at a time and freezes up the game.
    private void NextTurn() {
        print("End Turn!");
        turnNumber++;
        OnTurnChanged?.Invoke(this,EventArgs.Empty);
    }

    public int GetTurnNumber() {
        return turnNumber;
    }

    public bool IsPlayerTurn() {
        if(currentTurnUnit == null) return false;
        return !currentTurnUnit.GetIsEnemy();
    }
}
