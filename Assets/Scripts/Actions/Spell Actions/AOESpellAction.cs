using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOESpellAction : BaseAction {

    public AOESpellAction(Unit unit, ActionDataSO actionDataSO) : base(unit, actionDataSO) {

    }

    public event EventHandler OnSpellCharging;
    public event EventHandler OnSpellCasting;
    public event EventHandler OnSpellCooling;

    private enum State {
        Charging, Casting, Cooloff
    }

    private State state;
    private float stateTimer;
    private GridPosition targetGridPosition;
    private bool canCastSpell;
    //private GridPosition centerEffectGridPosition;
    private List<Unit> targetUnitList;

    public override void Update() {
        if(!isActive) return;

        stateTimer -= Time.deltaTime;
        
        switch(state) {
            case State.Charging:
                float rotateSpeed = 10f;
                Vector3 aimDirection = (LevelGrid.Instance.GetWorldPosition(targetGridPosition) - unit.GetWorldPosition()).normalized;
                unit.transform.forward = Vector3.Lerp(unit.transform.forward, new Vector3(aimDirection.x, 0, aimDirection.z), Time.deltaTime * rotateSpeed);
                break;
            case State.Casting:
                break;
            case State.Cooloff:
                if(canCastSpell) {
                    InstantiateEffectVFX();
                    //Should this be in take Action?
                    foreach (Unit unit in targetUnitList) {
                        unit.ProcessHealthChange(GetDamageAmount());
                    }
                }
                canCastSpell = false;
                break;
        }

        if (stateTimer <=0f) {
            NextState();    
        }
    }

    private void InstantiateEffectVFX() {
        GameObject effectVFX = GetEffectVFX();
        if (!effectVFX) return;
        GameObject.Instantiate(effectVFX, LevelGrid.Instance.GetWorldPosition(targetGridPosition), Quaternion.identity);
    }

    private void NextState(){
        switch(state) {
            case State.Charging:
                OnSpellCharging?.Invoke(this, EventArgs.Empty);
                if (stateTimer <=0f) {
                    state = State.Casting;
                    float castingStateTime = .8f;
                    stateTimer = castingStateTime;
                }
                break;
            case State.Casting:
                OnSpellCasting?.Invoke(this, EventArgs.Empty);
                if (stateTimer <=0f) {
                    state = State.Cooloff;
                    float cooloffStateTime = .1f;
                    stateTimer = cooloffStateTime;
                }
                break;
            case State.Cooloff:
                OnSpellCooling?.Invoke(this, EventArgs.Empty);
                if (stateTimer <=0f) {
                    ActionComplete();
                }
                break;
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete) {
        targetGridPosition = gridPosition;
        targetUnitList = GetTargetUnitList(targetGridPosition);

        canCastSpell = true;
        stateTimer = .2f;
        state = State.Charging;
        
        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetActionGridPositionRangeList() {
        int maxCastDistance = actionDataSO.GetMaxRange();
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxCastDistance; x <=maxCastDistance; x++) {
            for(int z = -maxCastDistance; z <= maxCastDistance; z++) {
                GridPosition offsetGridPosition = new GridPosition(x,z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(testDistance > maxCastDistance) continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    private List<Unit> GetTargetUnitList(GridPosition gridPosition) {
        List<GridPosition> gridPositionList = new List<GridPosition>();
        List<Unit> _targetUnitList = new List<Unit>();
        EffectShape effectShape = GetEffectShape();
        switch(effectShape) {
            case EffectShape.Circle:
                gridPositionList.AddRange(GridPositionShapes.GetGridPositionRangeCircle(gridPosition,GetEffectRange(),true));
                break;
            case EffectShape.Square:
                gridPositionList.AddRange(GridPositionShapes.GetGridPositionRangeSquare(gridPosition,GetEffectRange(),true));
                break;
            case EffectShape.Cross:
                gridPositionList.AddRange(GridPositionShapes.GetGridPositionRangeCross(gridPosition,GetEffectRange(),true));
                break;
            case EffectShape.Single:
                gridPositionList.Add(gridPosition);
                break;
                
        }
        foreach(GridPosition position in gridPositionList) {
            Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(position);
            if(targetUnit && unit.IsEnemy() != targetUnit.IsEnemy()) {
                _targetUnitList.Add(targetUnit);
            }
        }
        return _targetUnitList;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) {
        return new EnemyAIAction{
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }
}
