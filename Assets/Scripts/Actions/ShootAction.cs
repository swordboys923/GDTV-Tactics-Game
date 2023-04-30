using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction {

    public ShootAction(Unit unit, ActionDataSO actionDataSO) : base(unit, actionDataSO) {

    }
    public event EventHandler<OnShootEventArgs> OnShoot;
    public static event EventHandler<OnShootEventArgs> OnAnyShoot;
    public class OnShootEventArgs : EventArgs {
        public Unit targetUnit;
        public Unit shootingUnit;
    }
    private enum State {
        Aiming, Shooting, Cooloff
    }

    [SerializeField] private LayerMask obstaclesLayerMask;
    private State state;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;

    private void Update(){
        if(!isActive) return;

        stateTimer -= Time.deltaTime;
        
        switch(state) {
            case State.Aiming:
                float rotateSpeed = 10f;
                Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                unit.transform.forward = Vector3.Lerp(unit.transform.forward, new Vector3(aimDirection.x, 0, aimDirection.z), Time.deltaTime * rotateSpeed);
                break;
            case State.Shooting:
                if (canShootBullet) {
                    Shoot();
                    canShootBullet = false;
                }
                break;
            case State.Cooloff:
                break;
        }

        if (stateTimer <=0f) {
            NextState();    
        }
    }

    private void Shoot(){
        OnAnyShoot?.Invoke(this, new OnShootEventArgs{
            targetUnit = targetUnit,
            shootingUnit = unit,
        });
        OnShoot?.Invoke(this, new OnShootEventArgs{
            targetUnit = targetUnit,
            shootingUnit = unit,
        });
        targetUnit.Damage(GetDamageAmount());
    }

    private void NextState(){
        switch(state) {
            case State.Aiming:
                if (stateTimer <=0f) {
                    state = State.Shooting;
                    float shootingStateTime = .1f;
                    stateTimer = shootingStateTime;
                }
                break;
            case State.Shooting:
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

    public override string GetActionName() {
        return "Shoot";
    }

    public override bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetActionGridPositionRangeList();

        if(!LevelGrid.Instance.HasAnyUnitOnGridPosition(gridPosition)) return false;

        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        if(targetUnit.IsEnemy() == unit.IsEnemy()) return false;

        Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unit.GetGridPosition());
        Vector3 shootDirection = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;
        float unitShoulderHeight = 1.7f;
        if(Physics.Raycast(
            unitWorldPosition + Vector3.up * unitShoulderHeight,shootDirection, 
            Vector3.Distance(unitWorldPosition, 
            targetUnit.GetWorldPosition()), 
            obstaclesLayerMask)) {
                return false;
            }

        return validGridPositionList.Contains(gridPosition);
    }

    public override List<GridPosition> GetActionGridPositionRangeList() {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetActionGridPositionRangeList(unitGridPosition);
    }
    public List<GridPosition> GetActionGridPositionRangeList(GridPosition unitGridPosition) {
        int maxShootDistance = actionDataSO.GetMaxRange();
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxShootDistance; x <=maxShootDistance; x++) {
            for(int z = -maxShootDistance; z <= maxShootDistance; z++) {
                GridPosition offsetGridPosition = new GridPosition(x,z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(testDistance > maxShootDistance) continue;

                // if(!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;

                // Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                // if(targetUnit.IsEnemy() == unit.IsEnemy()) continue;

                // Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                // Vector3 shootDirection = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;
                // float unitShoulderHeight = 1.7f;
                // if(Physics.Raycast(
                //     unitWorldPosition + Vector3.up * unitShoulderHeight,shootDirection, 
                //     Vector3.Distance(unitWorldPosition, 
                //     targetUnit.GetWorldPosition()), 
                //     obstaclesLayerMask)) {
                //         continue;
                //     }
                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete) {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;
        canShootBullet = true;
        
        ActionStart(onActionComplete);
    }

    public Unit GetTargetUnit() {
        return targetUnit;
    }

    public int GetMaxShootDistance(){
        return actionDataSO.GetMaxRange();
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        return new EnemyAIAction {
            gridPosition = gridPosition,
            actionValue = Mathf.RoundToInt(100 + (1- targetUnit.GetHealthNormalized()) * 100f),
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition) {
        List<GridPosition> gridPositionList = new List<GridPosition>();
        foreach(GridPosition position in GetActionGridPositionRangeList(gridPosition)) {
            if(IsValidActionGridPosition(position)) gridPositionList.Add(position);
        }
        return gridPositionList.Count;
    }
}
