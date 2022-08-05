using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    [SerializeField] private bool isEnemy;
    [SerializeField] int actionPointsMax = 2;
    private GridPosition gridPosition;
    private HealthSystem healthSystem;
    private ManaSystem manaSystem;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private ShootAction shootAction;
    private BaseAction[] baseActionArray;
    private int actionPoints;

    private void Awake() {
        actionPoints = actionPointsMax;
        healthSystem = GetComponent<HealthSystem>();
        manaSystem = GetComponent<ManaSystem>();
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        shootAction = GetComponent<ShootAction>();
        baseActionArray = GetComponents<BaseAction>();
    }
    private void Start() {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        healthSystem.OnDead += HealthSystem_OnDead;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }
    private void Update() {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if(newGridPosition != gridPosition) {
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }
    public MoveAction GetMoveAction() {
        return moveAction;
    }

    public SpinAction GetSpinAction() {
        return spinAction;
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
        return baseActionArray;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction) {
        if(CanSpendActionPointsToTakeAction(baseAction) && baseAction.CanAct()) {
            SpendManaPoints(baseAction.GetActionManaCost());
            return true;
        }
        return false;
    }
    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction) {
        return manaSystem.GetMana() >= baseAction.GetActionManaCost();
    }

    public int GetActionPoints() {
        return actionPoints;
    }

    public bool IsEnemy() {
        return isEnemy;
    }

    public void Damage(int damageAmount) {
        healthSystem.Damage(damageAmount);
    }

    private void SpendManaPoints(int amount) {
        manaSystem.ProcessMana(amount);

        //TODO: Finish this bit
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e) {
        if((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn())) {
            actionPoints = actionPointsMax;
            foreach(BaseAction baseAction in baseActionArray){
                baseAction.SetStartTurnValue();
            }
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void HealthSystem_OnDead(object sender, EventArgs e) {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition,this);
        Destroy(gameObject);

        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
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

    public float GetManaNormalized() {
        return manaSystem.GetManaNormalized();
    }

    public int GetMana() {
        return manaSystem.GetMana();
    }

    public int GetManaMax() {
        return manaSystem.GetManaMax();
    }
}