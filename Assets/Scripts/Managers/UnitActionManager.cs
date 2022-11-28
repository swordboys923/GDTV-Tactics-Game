using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionManager : MonoBehaviour {

    public static UnitActionManager Instance { get; private set; }
    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;

    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;
    
    private BaseAction selectedAction;
    private bool isBusy;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There's more than one UnitActionManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance  = this;
    }
    private void Start() {
        SetSelectedUnit(selectedUnit);
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }
    private void Update() {
        if(isBusy) return;
        if(!TurnSystem.Instance.IsPlayerTurn()) return;
        if(EventSystem.current.IsPointerOverGameObject()) return;
        if(TryHandleUnitSelection()) return;
        if(selectedAction) HandleSelectedAction();
    }

    private void HandleSelectedAction() {
        if(InputManager.Instance.IsMouseButtonDownThisFrame()){
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            if(!selectedAction.IsValidActionGridPosition(mouseGridPosition)) return;
            if(!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction)) return;
            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);
            OnActionStarted?.Invoke(this,EventArgs.Empty);
        }
    }

    private void SetBusy() {
        isBusy = true;
        OnBusyChanged?.Invoke(this, isBusy);
    }

    private void ClearBusy() {
        isBusy = false;
        OnBusyChanged?.Invoke(this, isBusy);
    }

    private bool TryHandleUnitSelection() {
        if(InputManager.Instance.IsMouseButtonDownThisFrame()) {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
            if(Physics.Raycast(ray, out RaycastHit raycastHit,float.MaxValue,unitLayerMask)) {
                if(raycastHit.transform.TryGetComponent<Unit>(out Unit unit)) {
                    if (unit == selectedUnit) return false;
                    if (unit.IsEnemy()) return false;
                    
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;
    }

    private void SetSelectedUnit (Unit unit) {
        selectedUnit = unit;
        //SetSelectedAction(unit.GetMoveAction());

        OnSelectedUnitChanged?.Invoke(this,EventArgs.Empty);
    }

    public BaseAction GetSelectedAction() {
        return selectedAction;
    }

    public void SetSelectedAction(BaseAction baseAction) {
        selectedAction = baseAction;
        OnSelectedActionChanged?.Invoke(this,EventArgs.Empty);
    }

    public Unit GetSelectedUnit() {
        return selectedUnit;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e) {
        if(selectedUnit == null) {
            SetSelectedUnit(UnitManager.Instance.GetFriendlyUnitList()[0]);
        }
        //SetSelectedAction(selectedUnit.GetBaseActionArray()[0]);
    }
}
