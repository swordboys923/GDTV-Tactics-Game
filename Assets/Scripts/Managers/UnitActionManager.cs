using System;
using UnityEngine;
using UnityEngine.EventSystems;

//TODO: StateManager

public class UnitActionManager : MonoBehaviour {

    public static UnitActionManager Instance { get; private set; }
    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<OnSelectedGridPositionChangedEventArgs> OnSelectedGridPositionChanged;
    public class OnSelectedGridPositionChangedEventArgs : EventArgs {
        public GridPosition gridPosition;
    }
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler<OnActionChosenEventArgs> OnActionChosen;
    public event EventHandler OnActionStarted;
    public event EventHandler OnActionCancelled;
    public class OnActionChosenEventArgs : EventArgs {
        public GridPosition gridPosition;
        public Unit unit;
        public BaseAction action;
    }

    [SerializeField] private Unit clickedOnUnit;
    [SerializeField] private LayerMask unitLayerMask;
    private Unit currentTurnUnit;
    private GridPosition selectedGridPosition;
    private GridPosition mouseGridPosition;
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
        //SetSelectedUnit(selectedUnit);
        TurnManager.Instance.OnUnitTurnChanged += TurnManager_OnUnitTurnChanged;
        // BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;
    }
    private void Update() {
        if(isBusy) return;
        if(!TurnManager.Instance.IsPlayerTurn()) return;
        if(currentTurnUnit.GetIsRouting()) return;
        if(EventSystem.current.IsPointerOverGameObject()) return;
        // if(TryHandleUnitSelection()) return;
        if(selectedAction != null) HandleSelectedAction();
    }

    private void HandleSelectedAction() {
        GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

        if(mouseGridPosition != selectedGridPosition) {
            selectedGridPosition = mouseGridPosition;
            OnSelectedGridPositionChanged?.Invoke(this, new OnSelectedGridPositionChangedEventArgs{
                gridPosition = selectedGridPosition
        });
        }
        if(InputManager.Instance.IsMouseButtonDownThisFrame()){
            if(!selectedAction.IsValidActionGridPosition(mouseGridPosition)) return;
            if(!currentTurnUnit.CanSpendActionPointsToTakeAction(selectedAction)) return;
            OnActionChosen?.Invoke(this, new OnActionChosenEventArgs {
                gridPosition = selectedGridPosition,
                unit = currentTurnUnit,
                action = selectedAction,
            });
            SetMouseGridPosition(mouseGridPosition);
            SetBusy();
        }
    }

    //Called by the Confirmation button on the ActionConfirmationUI
    public async void TakeAction(){
        OnActionStarted?.Invoke(this, EventArgs.Empty);
        await selectedAction.TakeAction(mouseGridPosition,ClearBusy);
        Debug.Log("Finished action");
    }

    //Called by the Decline button on the ActionConfirmationUI
    public void DeclineAction() {
        OnActionCancelled?.Invoke(this, EventArgs.Empty);
        ClearBusy();
    }

    private void SetMouseGridPosition(GridPosition mouseGridPosition) {
        this.mouseGridPosition = mouseGridPosition;
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
                    if (unit == currentTurnUnit) return false;
                    if (unit.IsEnemy()) return false;
                    
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;
    }

    private void SetSelectedUnit (Unit unit) {
        clickedOnUnit = unit;
        //SetSelectedAction(unit.GetMoveAction());

        OnSelectedUnitChanged?.Invoke(this,EventArgs.Empty);
    }

    private void SetCurrentTurnUnit(Unit unit) {
        currentTurnUnit = unit;
        SetSelectedAction(unit.GetMoveAction());
    }

    public BaseAction GetSelectedAction() {
        return selectedAction;
    }

    public void SetSelectedAction(BaseAction baseAction) {
        if (!currentTurnUnit.CanSpendActionPointsToTakeAction(baseAction)) return;
        selectedAction = baseAction;
        OnSelectedActionChanged?.Invoke(this,EventArgs.Empty);
    }

    //TODO: Commenting this out to see what else in the project relies on the selectedUnit.
    // public Unit GetSelectedUnit() {
    //     return clickedOnUnit;
    // }

    private void TurnManager_OnUnitTurnChanged(object sender, TurnManager.OnUnitTurnChangedEventArgs e) {
        SetCurrentTurnUnit(e.currentTurnUnit);
    }

    // private void BaseAction_OnAnyActionStarted(object sender, EventArgs e) {
    //     SetSelectedAction(null);
    // }

    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e) {
        foreach (BaseAction action in currentTurnUnit.GetBaseActionArray()) {
            if (currentTurnUnit.CanSpendActionPointsToTakeAction(action)){
                SetSelectedAction(action);
                return;
            }
        }
    }
    
}
