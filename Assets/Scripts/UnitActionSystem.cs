using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour {

    [SerializeField] private BaseAction moveAction;
    [SerializeField] private BaseAction attackAction;
    [SerializeField] private BaseAction[] specialActionArray;

    [SerializeField] private BaseAction interactAction;
    private BaseAction[] baseActionArray;

    /*
    The intent for this class is to house the individual baseAction gameObjects attached to a Unit for purpose of clasification
    If an action is in the baseMoveActions array - then using it will cost move action resources (i.e. allowing a unit to both move and attack)
    base attack actions will let the unit attack without generating resources
    special actions will let the unit spend the corresponding resource in order to use it.

    This will also allow the UI to know what abilities to go where.
        - Right now they are organized by the hierarchy of game objects on the Unit. 
        - Special abilities will likely go into a separate menu, for instance.

    Should there ever be a reason for a unit to have more than one base move and one base attack?
        -At the moment, I'm leaning "no"

    */

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
}
