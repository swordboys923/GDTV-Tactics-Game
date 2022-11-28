using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    [SerializeField] private GameObject actionCameraGameObject;
    [SerializeField] private CameraController cameraController;

    private void Start() {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;
        //TODO: Fix This. Want to reorient the camera to the selected character. 
        // UnitActionManager.Instance.OnSelectedUnitChanged += UnitActionManager_OnSelectedUnitChanged;

        HideActionCamera();
    }
    private void ShowActionCamera() {
        actionCameraGameObject.SetActive(true);
    }

    private void HideActionCamera() {
        actionCameraGameObject.SetActive(false);
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e) {
        switch(sender) {
            case ShootAction shootAction:
                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();
                Vector3 cameraCharacterHeight = (Vector3.up * 1.7f);

                Vector3 shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;

                float shoulderOffsetAmount = .5f;
                Vector3 shoulderOffset = Quaternion.Euler(0,90,0) * shootDir * shoulderOffsetAmount;

                Vector3 actionCameraPosition = shooterUnit.GetWorldPosition() + cameraCharacterHeight + shoulderOffset + (shootDir * -1);

                actionCameraGameObject.transform.position = actionCameraPosition;
                actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);
                ShowActionCamera();
                break;
        }
    }

    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e) {
        switch(sender) {
            case ShootAction shootAction:
                HideActionCamera();
                break;
        }
    }

    // private void UnitActionManager_OnSelectedUnitChanged(object sender, EventArgs e) {
    //     Unit unit = UnitActionManager.Instance.GetSelectedUnit();
    //     Vector3 location = unit.GetWorldPosition();
    //     cameraController.SetTransform(location);
    // }

}
