using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAction : BaseAction {

    public WaitAction(Unit unit, ActionDataSO actionDataSO) : base(unit, actionDataSO) {

    }
    
    public static event EventHandler<OnAnyWaitEventArgs> OnAnyWait;
    public class OnAnyWaitEventArgs : EventArgs {
        public Unit unit;
    }


    public override void Update() {
        if (!isActive) return;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) {
        return new EnemyAIAction {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }

    public override List<GridPosition> GetActionGridPositionRangeList() {
        GridPosition unitGridPosition = unit.GetGridPosition();

        return new List<GridPosition> {unitGridPosition};
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete) {
        ActionStart(onActionComplete);
        OnAnyWait?.Invoke(this, new OnAnyWaitEventArgs {
            unit = this.unit
        });
        ActionComplete();
    }

}
