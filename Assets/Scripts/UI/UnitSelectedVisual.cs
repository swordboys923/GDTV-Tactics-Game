using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit unit;

    private MeshRenderer meshRenderer;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start() {
        UnitActionManager.Instance.OnSelectedUnitChanged += UnitActionManager_OnSelectedUnitChanged;
        UpdateUnitVisual();
    }

    private void UnitActionManager_OnSelectedUnitChanged(object sender, EventArgs empty) {
        UpdateUnitVisual();
    }

    private void UpdateUnitVisual() {
        if(TurnManager.Instance.GetCurrentTurnUnit() == unit) {
            meshRenderer.enabled = true;
        } else {
            meshRenderer.enabled = false;
        }
    }

    private void OnDestroy() {
        UnitActionManager.Instance.OnSelectedUnitChanged -= UnitActionManager_OnSelectedUnitChanged;
    }
}
