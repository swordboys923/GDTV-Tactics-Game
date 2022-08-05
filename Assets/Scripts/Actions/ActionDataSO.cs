using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionDataSO", menuName = "GDTV-Tactics-Game/ActionDataSO", order = 0)]
public class ActionDataSO : ScriptableObject {
    [SerializeField] int actionCost;
    [SerializeField] int actionBaseDamage;
    [SerializeField] int maxRange;

    public int GetActionCost() {
        return actionCost;
    }

    public int GetBaseActionDamage() {
        return actionBaseDamage;
    }

    public int GetMaxRange() {
        return maxRange;
    }
}

