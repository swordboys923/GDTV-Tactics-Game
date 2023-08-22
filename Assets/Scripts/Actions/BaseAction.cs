using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class BaseAction {

    public BaseAction(Unit unit, ActionDataSO actionDataSO) {
        this.unit = unit;
        this.actionDataSO = actionDataSO;
    }

    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler<BaseActionEventArgs> OnAnyActionCompleted;
    public event EventHandler<ActionResourceEventArgs> OnActionComplete;
    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;

    public class ActionResourceEventArgs: EventArgs {
        public int resourceCost;
        public int staminaCost;
        public int resolveCost;
    }

    public class BaseActionEventArgs: EventArgs {
        public Unit actingUnit;
    }

    [SerializeField] protected ActionDataSO actionDataSO;

    public virtual bool GetIsActive() {
        return isActive;
    }
    public virtual string GetActionName() {
        return actionDataSO.GetActionName();
    }
    public virtual ActionType GetActionType() {
        return actionDataSO.GetActionType();
    }

    public virtual int GetMaxHeight() {
        return actionDataSO.GetMaxHeight();
    }

    public virtual int GetEffectRange() {
        return actionDataSO.GetEffectRange();
    }
    public virtual EffectShape GetEffectShape() {
        return actionDataSO.GetEffectShape();
    }

    public virtual GameObject GetEffectVFX() {
        return actionDataSO.GetEffectVFX();
    }

    public virtual string GetAnimationString() {
        return actionDataSO.GetAnimationString();
    }
    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);
    public abstract void Update();
   
    public virtual bool IsValidActionGridPosition(GridPosition gridPosition){
        List<GridPosition> validGridPositionList = GetActionGridPositionRangeList();
        return validGridPositionList.Contains(gridPosition);
    }
    public virtual int GetActionResourceCost() {
        return actionDataSO.GetActionResourceCost();
    }

    public abstract List<GridPosition> GetActionGridPositionRangeList();

    public virtual int GetActionStaminaCost() {
        return actionDataSO.GetActionStaminaCost();
    }

    public virtual int GetActionResolveCost() {
        return actionDataSO.GetActionResolveCost();
    }

    protected void ActionStart(Action onActionComplete) {
        isActive = true;
        this.onActionComplete = onActionComplete;

        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    protected int GetDamageAmount() {
        return actionDataSO.GetBaseActionDamage();
    }

    protected void ActionComplete() {
        isActive = false;
        onActionComplete();

        OnActionComplete?.Invoke(this,new ActionResourceEventArgs {
            resourceCost = 0,
            staminaCost = GetActionStaminaCost(),
            resolveCost = GetActionResolveCost()
        });
        OnAnyActionCompleted?.Invoke(this, new BaseActionEventArgs{
            actingUnit = unit
        });
    }

    public Unit GetUnit() {
        return unit;
    }

    public EnemyAIAction GetBestEnemyAIAction(){
        List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();

        List<GridPosition> validActionGridPositionList = GetActionGridPositionRangeList();
        foreach(GridPosition gridPosition in validActionGridPositionList){
            if(!IsValidActionGridPosition(gridPosition)) continue;
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
            enemyAIActionList.Add(enemyAIAction);
        }
        if(enemyAIActionList.Count > 0) {
            enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);
            return enemyAIActionList[0];
        }
        return null;
    }

    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);
}
