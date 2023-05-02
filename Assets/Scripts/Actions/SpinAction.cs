using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction{
    public SpinAction(Unit unit, ActionDataSO actionDataSO) : base(unit, actionDataSO) {

    }

    private float totalSpinAmount;
    public override void Update() {
        if(!isActive) return;
        
        float spinAddAmount = 360f * Time.deltaTime;
        unit.transform.eulerAngles += new Vector3(0,spinAddAmount,0);
        totalSpinAmount += spinAddAmount;
        if(totalSpinAmount >= 360f) {
            ActionComplete();
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete) {
        totalSpinAmount = 0f;
        ActionStart(onActionComplete);
    }


    public override List<GridPosition> GetActionGridPositionRangeList() {
        GridPosition unitGridPosition = unit.GetGridPosition();

        return new List<GridPosition> {unitGridPosition};
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) {
        return new EnemyAIAction {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }
}
