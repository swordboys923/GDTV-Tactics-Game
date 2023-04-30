using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour {
    // FIXME

    [SerializeField] private ActionDataSO moveActionSO;
    [SerializeField] private ActionDataSO attackActionSO;
    [SerializeField] private ActionDataSO[] specialActionSOArray;
    [SerializeField] private ActionDataSO interactActionSO;
    [SerializeField] private ActionDataSO waitActionSO;
    private ActionDataSO[] baseActionSOArray;
    private BaseAction moveAction;
    private BaseAction attackAction;
    private BaseAction[] specialActionArray;
    private BaseAction interactAction;
    private BaseAction waitAction;
    private BaseAction[] baseActionArray;


    private void Start() {
        baseActionSOArray = CreateBaseActionSOArray();

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

    public BaseAction GetInteractAction() {
        return interactAction;
    }

    public BaseAction GetWaitAction() {
        return waitAction;
    }
}
