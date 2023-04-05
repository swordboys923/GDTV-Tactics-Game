using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAction : BaseAction {
    public static event EventHandler<OnAnyWaitEventArgs> OnAnyWait;
    public class OnAnyWaitEventArgs : EventArgs {
        public Unit unit;
    }


    private void Update() {
        if (!isActive) return;
    }
    public override string GetActionName() {
        return "Wait";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) {
        return new EnemyAIAction {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList() {
        GridPosition unitGridPosition = unit.GetGridPositionXZ();

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
