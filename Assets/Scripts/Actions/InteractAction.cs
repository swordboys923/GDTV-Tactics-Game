using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction {

    public InteractAction(Unit unit, ActionDataSO actionDataSO) : base(unit, actionDataSO) {

    }
    
    public override void Update() {
        if(!isActive) return;

    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) {
        return new EnemyAIAction {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }

    public override List<GridPosition> GetActionGridPositionRangeList() {
        int maxInteractDistance = actionDataSO.GetMaxRange();
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxInteractDistance; x <=maxInteractDistance; x++) {
            for(int z = -maxInteractDistance; z <= maxInteractDistance; z++) {
                GridPosition offsetGridPosition = new GridPosition(x,z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                IInteractible interactible = LevelGrid.Instance.GetInteractibleAtGridPosition(testGridPosition);
                if (interactible == null) continue;

                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete) {
        IInteractible interactible = LevelGrid.Instance.GetInteractibleAtGridPosition(gridPosition);
        interactible.Interact(OnInteractComplete);

        ActionStart(onActionComplete);
    }


    private void OnInteractComplete(){
        ActionComplete();
    }
}
