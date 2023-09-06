using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            MoveAction moveAction = (MoveAction)TurnManager.Instance.GetCurrentTurnUnit().GetMoveAction();
            moveAction.Undo();
        }
    }
}
