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
        TurnManager.Instance.OnTurnChanged += TurnManager_OnTurnChanged;
    }

    void Update() {
        if(!currentTurnUnit || !currentTurnUnit.GetIsRouting()) return;
        
        switch(state){
            case State.WaitingForUnitTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <=0f) {
                    if(TryTakeRoutingUnitAction(SetStateTakingTurn)) {
                        state = State.Busy;
                    }
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


    private bool TryTakeRoutingUnitAction(Action onRoutingAIActionComplete) {
        if(!currentTurnUnit.GetIsRouting()) return false;
        if(currentTurnUnit.GetGridPosition() == LevelGrid.Instance.GetRoutingGridPosition(currentTurnUnit.GetFaction())) {
            Debug.Log("Unit has routed");
            currentTurnUnit.TakeAction(currentTurnUnit.GetWaitAction(),currentTurnUnit.GetGridPosition(),onRoutingAIActionComplete);
            return false;
        }
        if(!currentTurnUnit.CanSpendActionPointsToTakeAction(currentTurnUnit.GetMoveAction())) {
            currentTurnUnit.TakeAction(currentTurnUnit.GetWaitAction(),currentTurnUnit.GetGridPosition(),onRoutingAIActionComplete);
            return false;
        }
        List<GridPosition> path = Pathfinding.Instance.FindPath(currentTurnUnit.GetGridPosition(),LevelGrid.Instance.GetRoutingGridPosition(currentTurnUnit.GetFaction()), out int pathLength, currentTurnUnit.jump);
        if(path == null) {
            Debug.LogError($"No path found to routing coords: {LevelGrid.Instance.GetRoutingGridPosition(currentTurnUnit.GetFaction())}");
            currentTurnUnit.TakeAction(currentTurnUnit.GetWaitAction(),currentTurnUnit.GetGridPosition(),onRoutingAIActionComplete);
            return true;
        }
        path.Reverse();

        List<GridPosition> unitMovementGridPositionList = currentTurnUnit.GetMoveAction().GetActionGridPositionRangeList();
        foreach(GridPosition gridPosition in path) {
            if (unitMovementGridPositionList.Contains(gridPosition)) {
                currentTurnUnit.TakeAction(currentTurnUnit.GetMoveAction(),gridPosition,onRoutingAIActionComplete);

                //unit.TakeAction(unit.GetWaitAction(),unit.GetGridPosition(), ()=> {});
                return true;
            }
        }
        return false;
    }

    private void TurnManager_OnUnitTurnChanged(object sender, EventArgs e) {
        currentTurnUnit = TurnManager.Instance.GetCurrentTurnUnit();
        if(!currentTurnUnit.GetIsRouting()) return;
        Debug.Log(currentTurnUnit.name);
        state = State.TakingTurn;
        timer = .1f;
    }

    private void TurnManager_OnTurnChanged(object sender, EventArgs e) {
        
    }
}
