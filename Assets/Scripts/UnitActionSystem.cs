using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour {

    [SerializeField] private BaseAction moveAction;
    [SerializeField] private BaseAction attackAction;
    [SerializeField] private BaseAction[] specialActionArray;
    [SerializeField] private BaseAction interactAction;
    [SerializeField] private BaseAction waitAction;
    private BaseAction[] baseActionArray;

    private void Start() {
        baseActionArray = CreateBaseActionArray();

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
