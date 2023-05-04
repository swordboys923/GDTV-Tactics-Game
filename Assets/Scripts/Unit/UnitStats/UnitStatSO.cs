using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UnitStatSO", menuName = "GDTV-Tactics-Game/UnitStatSO", order = 0)]
public class UnitStatSO : ScriptableObject {
    [SerializeField] Stat[] stats;
}

[System.Serializable]
public class Stat {
    public UnitStat unitStat;
    public int amount;
}
