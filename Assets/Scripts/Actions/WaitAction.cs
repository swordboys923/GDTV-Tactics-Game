using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAction : BaseAction {

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
        return null; //unit.GetGridPosition();
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete) {
        ActionStart(onActionComplete);
    }

}
