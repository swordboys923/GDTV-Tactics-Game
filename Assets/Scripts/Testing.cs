using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

    [SerializeField] UnitWorldUI unitWorldUI;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.T)) {
            unitWorldUI.StaminaSystem_OnStaminaChanged();
        }
    }
}
