using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    [SerializeField] private bool isEnemy;
    [SerializeField] int actionActionPointsMax = 1;
    [SerializeField] int movementActionPointsMax = 1;
    private GridPosition gridPosition;
    private HealthSystem healthSystem;
    private ActionResourceSystem resourceSystem;
    private UnitActionSystem unitActionSystem;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private ShootAction shootAction;
    private BaseAction[] baseActionArray;
    private int actionActionPoints;
    private int movementActionPoints;

    private void Awake() {
        actionActionPoints = actionActionPointsMax;
        movementActionPoints = movementActionPointsMax;
        healthSystem = GetComponent<HealthSystem>();
        resourceSystem = GetComponent<ActionResourceSystem>();
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        shootAction = GetComponent<ShootAction>();
        baseActionArray = GetComponents<BaseAction>();
        unitActionSystem = GetComponent<UnitActionSystem>();
    }
    private void Start() {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        healthSystem.OnDead += HealthSystem_OnDead;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);

        foreach(BaseAction baseAction in baseActionArray){
            baseAction.OnActionComplete += BaseAction_OnActionComplete;
        }
    }
    private void Update() {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if(newGridPosition != gridPosition) {
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }
    public BaseAction GetMoveAction() {
        return unitActionSystem.GetMoveAction();
    }

    public BaseAction GetAttackAction() {
        return unitActionSystem.GetAttackAction();
    }

    public ShootAction GetShootAction() {
        return shootAction;
    }

    public GridPosition GetGridPosition() {
        return gridPosition;
    }

    public Vector3 GetWorldPosition() {
        return transform.position;
    }

    public BaseAction[] GetBaseActionArray() {
        return unitActionSystem.GetBaseActionArray();
    }

    public BaseAction[] GetSpecialActionArray() {
        return unitActionSystem.GetSpecialActionArray();
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction) {
        if(CanSpendActionPointsToTakeAction(baseAction)) {
            SpendResourcePoints(baseAction.GetActionResourceCost());
            return true;
        }
        return false;
    }
    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction) {
        int actionPoints;
        if (baseAction is MoveAction){
            actionPoints = movementActionPoints;
        } else {
            actionPoints = actionActionPoints;
        }
        return resourceSystem.HasSufficientResource(baseAction.GetActionResourceCost()) && actionPoints > 0;
    }

    public int GetActionPoints() {
        return 99;
    }

    public bool IsEnemy() {
        return isEnemy;
    }

    public void Damage(int damageAmount) {
        healthSystem.Damage(damageAmount);
    }

    private void SpendResourcePoints(int amount) {
        resourceSystem.ProcessActionResource(amount);

        //TODO: Finish this bit
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e) {
        if((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn())) {
            actionActionPoints = actionActionPointsMax;
            movementActionPoints = movementActionPointsMax;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void HealthSystem_OnDead(object sender, EventArgs e) {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition,this);
        Destroy(gameObject);

        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    private void BaseAction_OnActionComplete (object sender, EventArgs e) {
        BaseAction baseAction = sender as BaseAction;
        if(baseAction is MoveAction){
            movementActionPoints--;
        } else {
            actionActionPoints--;
        }
    }

    public float GetHealthNormalized() {
        return healthSystem.GetHealthNormalized();
    }

    public int GetHealth() {
        return healthSystem.GetHealth();
    }

    public int GetHealthMax() {
        return healthSystem.GetHealthMax();
    }

    public float GetResourceNormalized() {
        return resourceSystem.GetResourceNormalized();
    }

    public int GetResource() {
        return resourceSystem.GetResource();
    }

    public int GetResourceMax() {
        return resourceSystem.GetResourceMax();
    }
}