using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class RoutingAI : MonoBehaviour {

    private enum State {
        WaitingForUnitTurn,
        TakingTurn,
        Busy,
    }

    public static event EventHandler OnAnyRoutingComplete;

    Unit currentTurnUnit;

    private State state;
    private float timer;

    private void Awake() {
        state = State.WaitingForUnitTurn;
    }
    private void Start() {
        TurnManager.Instance.OnUnitTurnChanged += TurnManager_OnUnitTurnChanged;
        // OnAnyRoutingComplete +=
    }

    void Update() {
        if(!TurnManager.Instance.GetCurrentTurnUnit().GetIsRouting()) return;
        
        switch(state){
            case State.WaitingForUnitTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <=0f) {
                    TryTakeRoutingUnitAction(SetStateTakingTurn);
                    state = State.Busy;
                }
                break;
            case State.Busy:
                break;
        }
    }

    private void SetStateTakingTurn() {
        timer = .5f;
        state = State.TakingTurn;
    }


    private void TryTakeRoutingUnitAction(Action onRoutingAIActionComplete) {
        currentTurnUnit = TurnManager.Instance.GetCurrentTurnUnit();
        if(!currentTurnUnit.GetIsRouting()) return;
        if(currentTurnUnit.GetGridPosition() == LevelGrid.Instance.GetRoutingGridPosition(currentTurnUnit.GetFaction())) {
            Debug.Log("Unit has routed");
            currentTurnUnit.TakeAction(currentTurnUnit.GetWaitAction(),currentTurnUnit.GetGridPosition(),onRoutingAIActionComplete);
            return;
        }
        if(!currentTurnUnit.CanSpendActionPointsToTakeAction(currentTurnUnit.GetMoveAction())) {
            currentTurnUnit.TakeAction(currentTurnUnit.GetWaitAction(),currentTurnUnit.GetGridPosition(),onRoutingAIActionComplete);
            return;
        }
        List<GridPosition> path = Pathfinding.Instance.FindPath(currentTurnUnit.GetGridPosition(),LevelGrid.Instance.GetRoutingGridPosition(currentTurnUnit.GetFaction()), out int pathLength, currentTurnUnit.jump);
        path.Reverse();
        Debug.Log(path.Count);

        List<GridPosition> unitMovementGridPositionList = currentTurnUnit.GetMoveAction().GetActionGridPositionRangeList();
        foreach(GridPosition gridPosition in path) {
            if (unitMovementGridPositionList.Contains(gridPosition)) {
                currentTurnUnit.TakeAction(currentTurnUnit.GetMoveAction(),gridPosition,onRoutingAIActionComplete);

                //unit.TakeAction(unit.GetWaitAction(),unit.GetGridPosition(), ()=> {});
                return;
            }
        }
    }

    private void TurnManager_OnUnitTurnChanged(object sender, EventArgs e) {
        if(!TurnManager.Instance.IsPlayerTurn()) {
            state = State.TakingTurn;
            timer = 2f;
        }
    }
}
