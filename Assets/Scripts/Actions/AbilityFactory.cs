using UnityEngine;

public static class AbilityFactory {
    public static BaseAction CreateAbility(Unit unit, ActionDataSO actionDataSO) {
        switch (actionDataSO.GetActionType()) {
            case ActionType.Movement:
                return new MoveAction(unit, actionDataSO);
            case ActionType.Spell:
                return new AOESpellAction(unit, actionDataSO);
            case ActionType.Melee:
                return new SwordAction(unit, actionDataSO);
            case ActionType.Range:
                return new ShootAction(unit, actionDataSO);
            case ActionType.Interact:
                return new InteractAction(unit, actionDataSO);
            case ActionType.Wait:
                return new WaitAction(unit, actionDataSO);
            case ActionType.Test:
                return new SpinAction(unit, actionDataSO);
            default:
                Debug.LogError("Invalid ability type: " + actionDataSO.GetActionType());
                return null;
        }
    }
}
