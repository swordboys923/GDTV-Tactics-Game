using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Unit : MonoBehaviour {

    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    [SerializeField] private bool isEnemy;
    [SerializeField] private bool isRouting = false;
    [SerializeField] private Faction faction;

    // FIXME: Temporary variable to test out the pathfinding in the MoveAction;
    public int jump = 1;
    //FIXME: Temporary variables to test unit UI
    public Sprite backgroundImage;
    public Sprite uiImage;
    private GridPosition gridPosition;
    private HealthSystem healthSystem;
    private ActionResourceSystem resourceSystem;
    private UnitActionSystem unitActionSystem;
    private StaminaSystem staminaSystem;
    private ResolveSystem resolveSystem;
    private UnitStatSO unitStats;
    

    private void Awake() {
        healthSystem = GetComponent<HealthSystem>();
        resourceSystem = GetComponent<ActionResourceSystem>();
        staminaSystem = GetComponent<StaminaSystem>();
        resolveSystem = GetComponent<ResolveSystem>();
        unitActionSystem = GetComponent<UnitActionSystem>();
    }
    
    private void Start() {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

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
    public BaseAction GetMoveAction() {
        return unitActionSystem.GetMoveAction();
    }

    public BaseAction GetAttackAction() {
        return unitActionSystem.GetAttackAction();
    }

    public BaseAction GetWaitAction() {
        return unitActionSystem.GetWaitAction();
    }

    //FIXME: this is a bandaid because of the EnemyAI. Need to remove reliance on ShootAction
    public ShootAction GetShootAction() {
        return (ShootAction)unitActionSystem.GetShootAction();
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

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction) {
        return resourceSystem.HasSufficientResource(baseAction.GetActionResourceCost()) &&
            unitActionSystem.HasSufficientActionPoints(baseAction);
    }

    public void TakeAction(BaseAction action, GridPosition mouseGridPosition, Action onActionComplete) {
        unitActionSystem.TakeAction(action, mouseGridPosition, onActionComplete);
    }

    public bool IsEnemy() {
        return isEnemy;
    }

    public void ProcessHealthChange(int healthChangeAmount) {
        healthSystem.ProcessHealthChange(healthChangeAmount);
    }

    private void SpendResourcePoints(int amount) {
        resourceSystem.ProcessActionResource(amount);
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

    public float GetResourceNormalized() {
        return resourceSystem.GetResourceNormalized();
    }

    public int GetResource() {
        return resourceSystem.GetResource();
    }

    public int GetResourceMax() {
        return resourceSystem.GetResourceMax();
    }

    public float GetStaminaNormalized() {
        return staminaSystem.GetStaminaNormalized();
    }

    public int GetStamina() {
        return staminaSystem.GetStamina();
    }

    public int GetStaminaMax() {
        return staminaSystem.GetStaminaMax();
    }

    public bool GetIsEnemy() {
        return isEnemy;
    }

    public Sprite GetBackgroundImage(){
        return backgroundImage;
    }
    
    public Sprite GetForegroundImage() {
        return uiImage;
    }
    
    public void ProcessResoureChange(int resourceCost) {
        resourceSystem.ProcessActionResource(resourceCost);
    }

    public void ProcessStaminaChange(int staminaCost) {
        staminaSystem.ProcessStaminaChange(staminaCost);
    }

    public void ProcessResolveChange(int resolveCost) {
        resolveSystem.ProcessResolveChange(resolveCost);
    }

    public bool GetIsRouting() {
        return isRouting;
    }

    public Faction GetFaction() {
        return faction;
    }
}