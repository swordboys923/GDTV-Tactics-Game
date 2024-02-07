using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class SwordAction : BaseAction {

    public SwordAction(Unit unit, ActionDataSO actionDataSO) : base(unit, actionDataSO) {

    }
    public static event EventHandler OnAnySwordHit;
    public event EventHandler OnSwordActionStarted;
    public event EventHandler OnSwordActionCompleted;
    private enum State {
        SwingingSwordBeforeHit,
        SwingingSwordAfterHit,
    }
    private State state;
    private float stateTimer;
    private Unit targetUnit;

    public async override Task Update() {
        if(!isActive) return;

        stateTimer -= Time.deltaTime;
        
        switch(state) {
            case State.SwingingSwordBeforeHit:
                float rotateSpeed = 10f;
                Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                unit.transform.forward = Vector3.Lerp(unit.transform.forward, new Vector3(aimDirection.x, 0, aimDirection.z), Time.deltaTime * rotateSpeed);
                break;
            case State.SwingingSwordAfterHit:
                break;
        }

        if (stateTimer <=0f) {
            NextState();    
        }
    }

    private void NextState(){
        switch(state) {
            case State.SwingingSwordBeforeHit:
                state = State.SwingingSwordAfterHit;
                float afterHitStateTime = .5f;
                stateTimer = afterHitStateTime;
                targetUnit.ProcessHealthChange(GetDamageAmount());
                OnAnySwordHit?.Invoke(this, EventArgs.Empty);
                break;
            case State.SwingingSwordAfterHit:
                OnSwordActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;

        }
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) {
        return new EnemyAIAction{
            gridPosition = gridPosition,
            actionValue = 200,
        };
    }

    public override bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetActionGridPositionRangeList();
        if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(gridPosition)) return false;

        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        if(targetUnit.IsEnemy() == unit.IsEnemy()) return false;
        return validGridPositionList.Contains(gridPosition);
    }

    public override List<GridPosition> GetActionGridPositionRangeList() {
        int maxSwordDistance = actionDataSO.GetMaxRange();
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxSwordDistance; x <=maxSwordDistance; x++) {
            for(int z = -maxSwordDistance; z <= maxSwordDistance; z++) {
                GridPosition offsetGridPosition = new GridPosition(x,z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                //Testing for orthogonal adjancency
                if (testGridPosition.x != unitGridPosition.x && testGridPosition.z != unitGridPosition.z) continue;
                // Ensuring enemy is within one height differential 
                if (LevelGrid.Instance.GetAbsGridPositionHeightDifference(testGridPosition, unitGridPosition) > GetMaxHeight()) continue;
                if (testGridPosition == unitGridPosition) continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public async override Task TakeAction(GridPosition gridPosition, Action onActionComplete) {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        state = State.SwingingSwordBeforeHit;
        float beforeHitstateTime = .7f;
        stateTimer = beforeHitstateTime;

        OnSwordActionStarted?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }
}
