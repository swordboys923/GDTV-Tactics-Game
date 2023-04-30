using UnityEngine;

public static class AbilityFactory {
    public static BaseAction CreateAbility(Unit unit, ActionDataSO actionDataSO) {
        switch (actionDataSO.GetActionType()) {
            case ActionType.Movement:
                return new MoveAction(unit, actionDataSO);
            case ActionType.Spell:
                return new FireAction(unit, actionDataSO);
            case ActionType.Melee:
                return new SwordAction(unit, actionDataSO);
            case ActionType.Range:
                return new ShootAction(unit, actionDataSO);
            default:
                Debug.LogError("Invalid ability type: " + actionDataSO.GetActionType());
                return null;
        }
    }
}
