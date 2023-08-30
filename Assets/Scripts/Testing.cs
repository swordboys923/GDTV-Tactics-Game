using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

    private void Start() {
        UnitActionManager.Instance.OnActionChosen += Test;
    }

    private void Test(object sender, UnitActionManager.OnActionChosenEventArgs e) {
        Debug.Log("Target: "+ LevelGrid.Instance.GetUnitAtGridPosition(e.gridPosition));
        Debug.Log("Attacking Unit: " + e.unit);
    }
}
