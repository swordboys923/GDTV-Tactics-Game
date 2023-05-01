using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction {

    public GrenadeAction(Unit unit, ActionDataSO actionDataSO) : base(unit, actionDataSO) {

    }

    [SerializeField] private Transform grenadeProjectilePrefab;
    
    private void Update() {
        if(!isActive) return;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) {
        return new EnemyAIAction{
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }

    public override List<GridPosition> GetActionGridPositionRangeList() {
        int maxThrowDistance = actionDataSO.GetMaxRange();
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxThrowDistance; x <=maxThrowDistance; x++) {
            for(int z = -maxThrowDistance; z <= maxThrowDistance; z++) {
                GridPosition offsetGridPosition = new GridPosition(x,z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(testDistance > maxThrowDistance) continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    //FIXME
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete){
        // Transform grenadeProjectileTransform = Instantiate(grenadeProjectilePrefab, unit.GetWorldPosition(), Quaternion.identity);
        // grenadeProjectileTransform.GetComponent<GrenadeProjectile>().Setup(gridPosition,OnGrenadeBehaviorComplete);
        
        ActionStart(onActionComplete);
    }

    private void OnGrenadeBehaviorComplete(){
        ActionComplete();
    }

}
