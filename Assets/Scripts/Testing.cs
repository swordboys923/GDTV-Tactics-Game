using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

    [SerializeField] ParticleSystem particleEffect;
    [SerializeField] int range;
    [SerializeField] Unit unit;
    List<GridPosition> gridPositionList;
    AOESpellAction fireAction;

    private void Start() {
        foreach(BaseAction baseAction in unit.GetBaseActionArray()) {
            if(baseAction is AOESpellAction) {
                AOESpellAction fireAction = (AOESpellAction) baseAction;
                fireAction.OnSpellCharging += FireAction_OnSpellCharging;
                fireAction.OnSpellCasting += FireAction_OnSpellCasting;
                fireAction.OnSpellCooling += FireAction_OnSpellCooling;
            }

        }
    }

    private void FireAction_OnSpellCooling(object sender, EventArgs e)
    {
        Debug.Log("Cooling from Event!");
    }

    private void FireAction_OnSpellCasting(object sender, EventArgs e)
    {
        Debug.Log("Casting from Event!");
    }

    private void FireAction_OnSpellCharging(object sender, EventArgs e)
    {
        Debug.Log("Charging from Event!");
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.T)) {
            GridAnimationManager.Instance.SpawnEffect(GetGridPositionRadius(), particleEffect);
        }
    }

    private GridPosition[] GetGridPositionRadius(){
        gridPositionList = new List<GridPosition>();
        Unit unit = TurnManager.Instance.GetCurrentTurnUnit();
        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -range; x <= range; x++) {
            for (int z = -range; z <= range; z++) {
                GridPosition gridPosition = new GridPosition(x,z) + unitGridPosition;
                if(LevelGrid.Instance.IsValidGridPosition(gridPosition)){
                    gridPositionList.Add(gridPosition);
                }
            }
        }
        return gridPositionList.ToArray();
    }
}
