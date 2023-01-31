using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAction : BaseAction {
    public static event EventHandler<OnWaitEventArgs> OnAnyWait;

    public class OnWaitEventArgs : EventArgs {
        public Unit unit;
    }

    private void Update() {
        if (!isActive) return;
    }
    public override string GetActionName() {
        return "Wait";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) {
        throw new NotImplementedException();
    }

    public override List<GridPosition> GetValidActionGridPositionList() {
        List<GridPosition> positionList = new List<GridPosition>();
        positionList.Add(unit.GetGridPosition());
        return positionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete) {
        ActionStart(onActionComplete);
        OnAnyWait.Invoke(this, new OnWaitEventArgs {unit = unit});
        ActionComplete();
    }

}
