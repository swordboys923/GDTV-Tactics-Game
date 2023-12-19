using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: StateManager

[RequireComponent(typeof(GameFlowStateMachine))]
public class GameFlowManager : MonoBehaviour {
    GameFlowStateMachine gameFlowStateMachine;

    private void Start() {
        gameFlowStateMachine = GetComponent<GameFlowStateMachine>();

        Initialize();
    }

    private void Initialize() {
        // Push the following states:
            // TurnStart
            // RoutingCleanup
            // Cutscene
            // PrepUnitTurn
            // UnitTurn
                // RoutingAI
                // EnemyAI
                // UnitActionManager
            // TurnEnd

        GameState turnStartState = new TurnStartState();
        gameFlowStateMachine.PushStateStack(turnStartState);
    }
}

