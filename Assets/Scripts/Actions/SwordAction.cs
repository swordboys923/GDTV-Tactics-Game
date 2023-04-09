using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
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

    private void Update() {
        if(!isActive) return;

        stateTimer -= Time.deltaTime;
        
        switch(state) {
            case State.SwingingSwordBeforeHit:
                float rotateSpeed = 10f;
                Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, new Vector3(aimDirection.x, 0, aimDirection.z), Time.deltaTime * rotateSpeed);
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
                targetUnit.Damage(GetDamageAmount());
                OnAnySwordHit?.Invoke(this, EventArgs.Empty);
                break;
            case State.SwingingSwordAfterHit:
                OnSwordActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;

        }
    }

    public override string GetActionName() {
        return "Sword";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) {
        return new EnemyAIAction{
            gridPosition = gridPosition,
            actionValue = 200,
        };
    }

    //TODO: Experimenting with different ways to work with the Y height API. Perhaps I should ignore it completely and hide it until I specifically need it, like I have right now, instead of what is commented out. Might reduce complexity.
    public override List<GridPosition> GetValidActionGridPositionList() {
        int maxSwordDistance = actionDataSO.GetMaxRange();
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        //GridPosition unitGridPosition = unit.GetGridPositionXYZ();
        GridPosition unitGridPosition = unit.GetGridPositionXZ();

        for (int x = -maxSwordDistance; x <=maxSwordDistance; x++) {
            for(int z = -maxSwordDistance; z <= maxSwordDistance; z++) {
                GridPosition offsetGridPosition = new GridPosition(x,z);
                //GridPosition testGridPosition = LevelGrid.Instance.GetGridObjectGridPosition(unitGridPosition + offsetGridPosition);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                //Testing for orthogonal adjancency
                if (testGridPosition.x != unitGridPosition.x && testGridPosition.z != unitGridPosition.z) continue;
                //FIXME: Magic Number with the one below?
                // Ensuring enemy is within one height differential 
                if (Mathf.Abs(LevelGrid.Instance.GetGridObjectGridPosition(testGridPosition).height-LevelGrid.Instance.GetGridObjectGridPosition(unitGridPosition).height) > 1) continue;
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if(targetUnit.IsEnemy() == unit.IsEnemy()) continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete) {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        state = State.SwingingSwordBeforeHit;
        float beforeHitstateTime = .7f;
        stateTimer = beforeHitstateTime;

        OnSwordActionStarted?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }

    public int GetMaxSwordDistance() {
        return actionDataSO.GetMaxRange();
    }
    //TODO: This needs to be refactored out ASAP. Only here to test the new feature in GridSystemVisual;
    public int GetMaxSwordHeight() {
        return 1;
    }
}
