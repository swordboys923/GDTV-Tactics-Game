using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {
    // TODO: possible bug. While testing, one unit (the Unit) wasn't included in the turnOrderList

    public static TurnManager Instance { get; private set; }
    public event EventHandler OnTurnChanged;
    public event EventHandler<OnUnitTurnChangedEventArgs> OnUnitTurnChanged;
    public class OnUnitTurnChangedEventArgs : EventArgs {
        public Unit currentTurnUnit;
    }
    [SerializeField] private Unit currentTurnUnit;
    private List<Unit> turnOrderList;

    private int turnNumber = 1;
    private bool isPlayerTurn = true;
    
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
    }

    public List<Unit> GetTurnOrderList() {
        return turnOrderList;
    }

    public void AddUnitToTurnList(Unit unit) {
        turnOrderList.Add(unit);
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
        if (turnOrderList.Count() == 1) {
            // Current Unit is last in the list
            // End Turn
            print("End turn!");
        } else {
            RemoveUnitFromTurnList(currentTurnUnit);
            currentTurnUnit = turnOrderList[0];
            OnUnitTurnChanged?.Invoke(this, new OnUnitTurnChangedEventArgs {
                currentTurnUnit = currentTurnUnit,
            });
        }
    }


    
    //TODO: Should cycle through the turn list backwards. That way we can pop an item off of the list when they are done with their turn.
    //TODO: Should we keep two lists? One for current turn and one for next? That way if the are any status changes, we are projecting off of the second list without damanging the current list?

    private void WaitAction_OnAnyWait(object sender,EventArgs e) {

        print(((BaseAction)sender).GetUnit().name);
    }


    //OLD COURSE CODE
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
