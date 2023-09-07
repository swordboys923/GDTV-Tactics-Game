using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoutingAI : MonoBehaviour {

    private enum State {
        WaitingForUnitTurn,
        TakingTurn,
        Busy,
    }

    List<GridPosition> routingPath;

    private State state;
    private float timer;

    private void Start() {
        TurnManager.Instance.OnUnitTurnChanged += TurnManager_OnUnitTurnChanged;
    }

    private void Update() {
        switch(state){
            case State.WaitingForUnitTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                // if (timer <=0f) {
                //     if(TryTakeEnemyAIAction(SetStateTakingTurn)) {
                //         state = State.Busy;
                //     } else{
                //         // TODO: Here the enemy needs to end their turn. i.e. take the Wait Action.
                //         // Right now, am accomplishing this by making the Wait Action a low value, always free option, which seems to accomplish the desired result.
                //     }
                // }
                break;
            case State.Busy:
                break;
        }
    }

    private void TurnManager_OnUnitTurnChanged(object sender, EventArgs e) {
        Unit unit = TurnManager.Instance.GetCurrentTurnUnit();
        // if(!unit.GetIsRouting()) return;
        int pathLength;
        List<GridPosition> path = Pathfinding.Instance.FindPath(unit.GetGridPosition(),LevelGrid.Instance.GetRoutingGridPosition(unit.GetFaction()), out pathLength, unit.jump);
        Debug.Log(path.Count);
    }
}
