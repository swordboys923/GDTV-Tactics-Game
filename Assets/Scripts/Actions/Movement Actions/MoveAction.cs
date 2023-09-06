using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction, IUndo {

    public MoveAction(Unit unit, ActionDataSO actionDataSO) : base(unit, actionDataSO) {

    }

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;
    public event EventHandler<BaseAction> OnUndo;
    private List<Vector3> positionList;
    private int currentPositionIndex;

    private GridPosition startingGridPosition;

    public override void Update() {
        if (!isActive) return;

        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - unit.transform.position).normalized;

        float rotateSpeed = 10f;
        unit.transform.forward = Vector3.Slerp(unit.transform.forward, new Vector3(moveDirection.x, 0, moveDirection.z), Time.deltaTime * rotateSpeed);
        

        float stoppingDistance = .1f;
        if(Vector3.Distance(unit.transform.position, targetPosition) > stoppingDistance) {
            float moveSpeed = 4f;
            unit.transform.position += moveDirection * moveSpeed * Time.deltaTime;
        } else {
            currentPositionIndex++;
            if(currentPositionIndex >= positionList.Count){
                OnStopMoving?.Invoke(this,EventArgs.Empty);
                ActionComplete();
            }
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete) {
        startingGridPosition = unit.GetGridPosition();
        List<GridPosition> pathGridPositionList = Pathfinding.Instance.FindPath(unit.GetGridPosition(),gridPosition, out int pathLength, unit.jump);
        currentPositionIndex = 0;
        positionList = new List<Vector3>();
        foreach(GridPosition pathGridPosition in pathGridPositionList){
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }
        OnStartMoving?.Invoke(this,EventArgs.Empty);
        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetActionGridPositionRangeList() {
        int maxMoveDistance = actionDataSO.GetMaxRange();
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <=maxMoveDistance; x++) {
            for(int z = -maxMoveDistance; z <= maxMoveDistance; z++) {
                GridPosition offsetGridPosition = new GridPosition(x,z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

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

        return validGridPositionList;
    }

    public override int GetActionStaminaCost() {
        int cost = 0;
        int staminaCostPerGridPosition = actionDataSO.GetActionStaminaCost();
        int heightMultiplier = 2;
        for(int i = 1; i < positionList.Count; i++) {
            if(positionList[i-1].y == positionList[i].y) {
                cost += staminaCostPerGridPosition;
            } else if (positionList[i-1].y < positionList[i].y) {
                cost += staminaCostPerGridPosition * heightMultiplier;
            }
        }
        return cost;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) {
        int targetCountAtGridPosition = unit.GetShootAction().GetTargetCountAtPosition(gridPosition);
        return new EnemyAIAction {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,
        };
    }

    public void Undo() {
        if(startingGridPosition == null) return;
        unit.transform.position = LevelGrid.Instance.GetWorldPosition(startingGridPosition);
        OnUndo?.Invoke(this,this);
    }
}
