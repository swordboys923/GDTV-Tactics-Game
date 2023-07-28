using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour {
    [SerializeField] int actionActionPointsMax = 1;
    [SerializeField] int movementActionPointsMax = 1;

    [SerializeField] private ActionDataSO moveActionSO;
    [SerializeField] private ActionDataSO attackActionSO;
    [SerializeField] private ActionDataSO[] specialActionSOArray;
    [SerializeField] private ActionDataSO interactActionSO;
    [SerializeField] private ActionDataSO waitActionSO;
    
    private Unit unit;
    private int actionActionPoints;
    private int movementActionPoints;

    private ActionDataSO[] baseActionSOArray;
    private BaseAction moveAction;
    private BaseAction attackAction;
    private BaseAction[] specialActionArray;
    private BaseAction interactAction;
    private BaseAction waitAction;
    
    private BaseAction[] baseActionArray;
    private BaseAction currentAction;
    
    //FIXME: this is a bandaid because of the EnemyAI. Need to remove reliance on ShootAction
    private BaseAction shootAction;
    [SerializeField] ActionDataSO shootActionSO;

    private void Awake() {
        unit = GetComponent<Unit>();
        actionActionPoints = actionActionPointsMax;
        movementActionPoints = movementActionPointsMax;
        InitializeActions();
        shootAction = AbilityFactory.CreateAbility(unit,shootActionSO);
    }

    private void Start() {
        TurnManager.Instance.OnTurnChanged += TurnManager_OnTurnChanged;
    }

    private void Update() {
        if(currentAction == null) return;
        if(!currentAction.GetIsActive()) return;
        currentAction.Update();
    }

    public bool HasSufficientActionPoints(BaseAction baseAction) {
        int actionPoints;

        if (baseAction is MoveAction){
            actionPoints = movementActionPoints;
        } else if (baseAction is WaitAction) {
            return true;
        } else {
            actionPoints = actionActionPoints;
        }
        return actionPoints > 0;
    }

    public void TakeAction(BaseAction action, GridPosition mouseGridPosition, Action onActionComplete) {
        currentAction = action;
        currentAction.OnActionComplete += CurrentAction_OnActionComplete;
        action.TakeAction(mouseGridPosition, onActionComplete);
    }

    private void InitializeActions() {
        moveAction = AbilityFactory.CreateAbility(unit,moveActionSO);
        attackAction = AbilityFactory.CreateAbility(unit,attackActionSO);
        specialActionArray = new BaseAction[specialActionSOArray.Length];
        for (int i = 0; i < specialActionArray.Length; i++) {
            specialActionArray[i] = AbilityFactory.CreateAbility(unit,specialActionSOArray[i]);
        }
        interactAction = AbilityFactory.CreateAbility(unit,interactActionSO);
        waitAction = AbilityFactory.CreateAbility(unit,waitActionSO);
        InitializeArrays();
    }

    private void InitializeArrays() {
        baseActionSOArray = CreateBaseActionSOArray();
        baseActionArray = CreateBaseActionArray();
    }

    private ActionDataSO[] CreateBaseActionSOArray() {
        List<ActionDataSO> baseActionSOList = new List<ActionDataSO>();
        baseActionSOList.Add(moveActionSO);
        baseActionSOList.Add(attackActionSO);
        foreach(ActionDataSO actionSO in specialActionSOArray) {
            baseActionSOList.Add(actionSO);
        }
        baseActionSOList.Add(waitActionSO);
        return baseActionSOList.ToArray();
    }

    private BaseAction[] CreateBaseActionArray() {
        List<BaseAction> baseActionList = new List<BaseAction>();
        baseActionList.Add(moveAction);
        baseActionList.Add(attackAction);
        foreach(BaseAction action in specialActionArray) {
            baseActionList.Add(action);
        }
        baseActionList.Add(waitAction);
        return baseActionList.ToArray();
    }

    public ActionDataSO[] GetBaseActionSOArray() {
        return baseActionSOArray;
    }

    public ActionDataSO[] GetSpecialActionSOArray() {
        return specialActionSOArray;
    } 

    public ActionDataSO GetMoveActionSO() {
        return moveActionSO;
    }

    public ActionDataSO GetAttackActionSO() {
        return attackActionSO;
    }

    public ActionDataSO GetInteractActionSO() {
        return interactActionSO;
    }

    public ActionDataSO GetWaitActionSO() {
        return waitActionSO;
    }

    public BaseAction[] GetBaseActionArray() {
        return baseActionArray;
    }

    public BaseAction[] GetSpecialActionArray() {
        return specialActionArray;
    } 

    public BaseAction GetMoveAction() {
        return moveAction;
    }

    public BaseAction GetAttackAction() {
        return attackAction;
    }
    
    //FIXME: this is a bandaid because of the EnemyAI. Need to remove reliance on ShootAction
    public BaseAction GetShootAction() {
        return shootAction;
    }

    public BaseAction GetInteractAction() {
        return interactAction;
    }

    public BaseAction GetWaitAction() {
        return waitAction;
    }

    public void ProcessActionPoints() {

    }

    public void ProcessResolveCost(){

    }

    public void ProcessStaminaCost(){

    }

    public void ProcessResourceCost() {

    }

    private void CurrentAction_OnActionComplete(object sender, EventArgs e) {
        currentAction.OnActionComplete -= CurrentAction_OnActionComplete;
        if(currentAction is WaitAction) {
            currentAction = null;
            return;
        }

        if(currentAction is MoveAction){
            movementActionPoints--;
        } else {
            actionActionPoints--;
        }
        currentAction = null;
    }

    private void TurnManager_OnTurnChanged(object sender, EventArgs e) {
        actionActionPoints = actionActionPointsMax;
        movementActionPoints = movementActionPointsMax;
    }
}
