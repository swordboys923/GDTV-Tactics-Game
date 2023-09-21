using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    private enum State {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,
    }

    //TODO: Testing code, remove.
    [SerializeField] bool turnOffAI;

    private State state;
    private float timer;

    private void Awake() {
        state = State.WaitingForEnemyTurn;
    }
    private void Start() {
        TurnManager.Instance.OnUnitTurnChanged += TurnManager_OnUnitTurnChanged;
    }
    void Update() {
        if(turnOffAI) return;
        if(TurnManager.Instance.IsPlayerTurn()) return;
        if(TurnManager.Instance.GetCurrentTurnUnit().GetIsRouting()) return;
        //TODO: Testing Code -- Remove.
        if (turnOffAI) {
            Unit enemyUnit = TurnManager.Instance.GetCurrentTurnUnit();
            BaseAction waitAction = enemyUnit.GetBaseActionArray()[^1];
            enemyUnit.TakeAction(waitAction,enemyUnit.GetGridPosition(),SetStateTakingTurn);
        }
        
        switch(state){
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <=0f) {
                    if(TryTakeEnemyAIAction(SetStateTakingTurn)) {
                        state = State.Busy;
                    } else{
                        // TODO: Here the enemy needs to end their turn. i.e. take the Wait Action.
                        // Right now, am accomplishing this by making the Wait Action a low value, always free option, which seems to accomplish the desired result.
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

    private void TurnManager_OnUnitTurnChanged(object sender, EventArgs e) {
        if(!TurnManager.Instance.IsPlayerTurn()) {
            state = State.TakingTurn;
            timer = 2f;
        }
    }

    //TODO: Cycles through all enemies? Shouldn't I just use the current turn enemy?
    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete) {
        foreach(Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList()){
            if(TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete)) return true;
        }
        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete){
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;

        foreach(BaseAction baseAction in enemyUnit.GetBaseActionArray()){
            
            if (!enemyUnit.CanSpendActionPointsToTakeAction(baseAction))continue;

            if (bestEnemyAIAction == null) {
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;
            } else {
                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                if(testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue) {
                    bestEnemyAIAction = testEnemyAIAction;
                    bestBaseAction = baseAction;
                }
            }
        }

        if (bestEnemyAIAction != null && enemyUnit.CanSpendActionPointsToTakeAction(bestBaseAction)){
            enemyUnit.TakeAction(bestBaseAction,bestEnemyAIAction.gridPosition, onEnemyAIActionComplete);
            return true;
        } else {
            return false;
        }
    }
}