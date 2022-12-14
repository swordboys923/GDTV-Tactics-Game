using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseAction : MonoBehaviour {

    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler<BaseActionEventArgs> OnAnyActionCompleted;
    public event EventHandler OnActionComplete;
    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;

    public class BaseActionEventArgs: EventArgs {
        public Unit actingUnit;
    }

    [SerializeField] protected ActionDataSO actionDataSO;

    protected virtual void Awake() {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();
    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);
   
    public virtual bool IsValidActionGridPosition(GridPosition gridPosition){
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }
    public virtual int GetActionResourceCost() {
        return actionDataSO.GetActionCost();
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();


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

        OnActionComplete?.Invoke(this,EventArgs.Empty);
        OnAnyActionCompleted?.Invoke(this, new BaseActionEventArgs{
            actingUnit = unit
        });
    }

    public Unit GetUnit() {
        return unit;
    }

    public EnemyAIAction GetBestEnemyAIAction(){
        List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();

        List<GridPosition> validActionGridPositionList = GetValidActionGridPositionList();
        foreach(GridPosition gridPosition in validActionGridPositionList){
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
