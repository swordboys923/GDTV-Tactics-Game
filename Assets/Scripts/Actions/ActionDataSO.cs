using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionDataSO", menuName = "GDTV-Tactics-Game/ActionDataSO", order = 0)]
public class ActionDataSO : ScriptableObject {

    [Header("Ability Identity Data")]
    [SerializeField] ActionType actionType;
    [SerializeField] string actionName;

    [Header("Ability Use Data")]
    [SerializeField] int actionCost;
    [SerializeField] int actionBaseDamage;
    [SerializeField] int actionBasePercentageBonusDamage;
    [SerializeField] int maxRange;
    [SerializeField] UnitStat unitStatScalingAttribute;

    [Header("Ability Animation Data")]
    [SerializeField] string animationString;

    public string GetActionName() {
        return actionName;
    }

    public int GetActionCost() {
        return actionCost;
    }

    public int GetBaseActionDamage() {
        return actionBaseDamage;
    }

    public int GetActionBasePercentageBonusDamage() {
        return actionBasePercentageBonusDamage;
    }

    public int GetMaxRange() {
        return maxRange;
    }

    public UnitStat GetUnitStatScalingAttribute() {
        return unitStatScalingAttribute;
    }

    public ActionType GetActionType() {
        return actionType;
    }

    public string GetAnimationString() {
        return animationString;
    }
}

