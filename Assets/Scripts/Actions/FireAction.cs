using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAction : BaseAction {

    public FireAction(Unit unit, ActionDataSO actionDataSO) : base(unit, actionDataSO) {

    }

    private enum State {
        Charging, Casting, Cooloff
    }

    private State state;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;

    public override void Update() {
        if(!isActive) return;

        stateTimer -= Time.deltaTime;
        
        switch(state) {
            case State.Charging:
                break;
            case State.Casting:
                break;
            case State.Cooloff:
                break;
        }

        if (stateTimer <=0f) {
            NextState();    
        }
    }

    private void NextState(){
        switch(state) {
            case State.Charging:
                if (stateTimer <=0f) {
                    state = State.Casting;
                    float shootingStateTime = .1f;
                    stateTimer = shootingStateTime;
                }
                break;
            case State.Casting:
                if (stateTimer <=0f) {
                    state = State.Cooloff;
                    float cooloffStateTime = .5f;
                    stateTimer = cooloffStateTime;
                }
                break;
            case State.Cooloff:
                if (stateTimer <=0f) {
                    ActionComplete();
                }
                break;
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete) {
        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetActionGridPositionRangeList() {
        int maxCastDistance = actionDataSO.GetMaxRange();
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxCastDistance; x <=maxCastDistance; x++) {
            for(int z = -maxCastDistance; z <= maxCastDistance; z++) {
                GridPosition offsetGridPosition = new GridPosition(x,z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(testDistance > maxCastDistance) continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) {
        return new EnemyAIAction{
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }
}
