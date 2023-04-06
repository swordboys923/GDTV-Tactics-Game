using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction {

    public EventHandler OnStartMoving;
    public EventHandler OnStopMoving;
    private List<Vector3> positionList;
    private int currentPositionIndex;

    private void Update() {
        if (!isActive) return;

        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, new Vector3(moveDirection.x, 0, moveDirection.z), Time.deltaTime * rotateSpeed);
        

        float stoppingDistance = .1f;
        if(Vector3.Distance(transform.position, targetPosition) > stoppingDistance) {
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        } else {
            currentPositionIndex++;
            if(currentPositionIndex >= positionList.Count){
                OnStopMoving?.Invoke(this,EventArgs.Empty);
                ActionComplete();
            }
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete) {
        List<GridPosition> pathGridPositionList = Pathfinding.Instance.FindPath(unit.GetGridPositionXZ(),gridPosition, out int pathLength, unit.jump);
        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        foreach(GridPosition pathGridPosition in pathGridPositionList){
            GridPosition gridObjectGridPosition = LevelGrid.Instance.GetGridObjectGridPosition(pathGridPosition);
            positionList.Add(LevelGrid.Instance.GetWorldPosition(gridObjectGridPosition));
        }
        OnStartMoving?.Invoke(this,EventArgs.Empty);
        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList() {
        int maxMoveDistance = actionDataSO.GetMaxRange();
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPositionXYZ();

        for (int x = -maxMoveDistance; x <=maxMoveDistance; x++) {
            for(int z = -maxMoveDistance; z <= maxMoveDistance; z++) {
                GridPosition offsetGridPosition = new GridPosition(x,z);
                GridPosition testGridPosition = LevelGrid.Instance.GetGridObjectGridPosition(unitGridPosition + offsetGridPosition);

                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                if(unitGridPosition == testGridPosition) continue;
                if(LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;
                if(!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition)) continue;
                if(!Pathfinding.Instance.HasPath(unitGridPosition, testGridPosition)) continue;


                int pathfindingDistanceMultiplier = 10;
                if(Pathfinding.Instance.GetPathLength(unitGridPosition,testGridPosition, unit.jump) > maxMoveDistance * pathfindingDistanceMultiplier) continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        foreach(GridPosition gridPosition in validGridPositionList) {
            Debug.Log("Move Action " + gridPosition);
        }

        return validGridPositionList;
    }

    public override string GetActionName() {
        return "Move";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) {
        int targetCountAtGridPosition = unit.GetShootAction().GetTargetCountAtPosition(gridPosition);
        return new EnemyAIAction {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,
        };
    }
}
