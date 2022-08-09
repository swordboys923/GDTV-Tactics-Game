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
        UnitActionManager.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UpdateUnitVisual();
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs empty) {
        UpdateUnitVisual();
    }

    private void UpdateUnitVisual() {
        if(UnitActionManager.Instance.GetSelectedUnit() == unit) {
            meshRenderer.enabled = true;
        } else {
            meshRenderer.enabled = false;
        }
    }

    private void OnDestroy() {
        UnitActionManager.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
    }
}
