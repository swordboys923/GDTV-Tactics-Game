using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: DataManager

public class GridSystemVisual : MonoBehaviour {

    public static GridSystemVisual Instance { get; private set; }

    [Serializable]
    public struct GridVisualTypeMaterial {
        public GridVisualType gridVisualType;
        public Material material;
    }
    public enum GridVisualType {
        White,
        Blue,
        Red,
        Redsoft,
        Yellow,
        Purple,
    }
    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;
    
    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;
    
    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There's more than one GridSystemVisual! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance  = this;
    }
    private void Start() {
        gridSystemVisualSingleArray = new GridSystemVisualSingle[LevelGrid.Instance.GetWidth(), LevelGrid.Instance.GetDepth()];
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++) {
            for (int z = 0; z < LevelGrid.Instance.GetDepth(); z++) {
                GridPosition gridPosition = LevelGrid.Instance.GetGridObjectGridPosition(new GridPosition(x,z));
                Transform gridSystemVisualSingleTransform = Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                gridSystemVisualSingleArray[x,z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }

        UnitActionManager.Instance.OnSelectedActionChanged += UnitActionManager_OnSelectedActionChanged;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        UnitActionManager.Instance.OnSelectedUnitChanged += UnitActionManager_OnSelectedUnitChanged;
        UnitActionManager.Instance.OnSelectedGridPositionChanged += UnitActionManager_OnSelectedGridPositionChanged;

        UpdateGridVisual();
    }

    public void HideAllGridPositions() {
        foreach (var visual in gridSystemVisualSingleArray) {
            visual.Hide();
        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType) {
        if (gridPositionList.Count <= 0) return;
        foreach (var gridPosition in gridPositionList) {
            if(!LevelGrid.Instance.IsValidGridPosition(gridPosition))continue;
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show(GetGridVisualTypeMaterial(gridVisualType));
        }
    }

    private void ShowGridPositionRangeCircle(GridPosition gridPosition, int horizontalRange, GridVisualType gridVisualType,int verticalRange = int.MaxValue) {
        if (!LevelGrid.Instance.IsValidGridPosition(gridPosition)) return;
        List<GridPosition> gridPositionList = GridPositionShapes.GetGridPositionRangeCircle(gridPosition,horizontalRange);
        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    private void ShowGridPositionRangeSquare(GridPosition gridPosition, int horizontalRange, GridVisualType gridVisualType, int verticalRange = int.MaxValue) {
        if (!LevelGrid.Instance.IsValidGridPosition(gridPosition)) return;
        List<GridPosition> gridPositionList = GridPositionShapes.GetGridPositionRangeSquare(gridPosition,horizontalRange);
        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    private void ShowGridPositionRangeCross(GridPosition gridPosition, int horizontalRange, GridVisualType gridVisualType, int verticalRange = int.MaxValue) {
        if (!LevelGrid.Instance.IsValidGridPosition(gridPosition)) return;
        List<GridPosition> gridPositionList = GridPositionShapes.GetGridPositionRangeCross(gridPosition,horizontalRange);
        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    private void UpdateGridVisual() {
        HideAllGridPositions();
        Unit currentTurnUnit = TurnManager.Instance.GetCurrentTurnUnit();
        BaseAction selectedAction = UnitActionManager.Instance.GetSelectedAction();
        if(selectedAction == null) return;
        if(!currentTurnUnit.CanSpendActionPointsToTakeAction(selectedAction)) return;
        List<GridPosition> actionGridPositionRangeList = selectedAction.GetActionGridPositionRangeList();

        ShowActionRange(selectedAction, actionGridPositionRangeList);

        ShowActionEffectRange(selectedAction, actionGridPositionRangeList);

    }

    private void ShowActionRange(BaseAction selectedAction, List<GridPosition> actionGridPositionRangeList) {
        // Show Action Range
        GridVisualType gridVisualType;
        switch (selectedAction) {
            default:
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Blue;
                break;
            case GrenadeAction grenadeAction:
                gridVisualType = GridVisualType.Yellow;
                break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Redsoft;
                break;
            case SwordAction swordAction:
                gridVisualType = GridVisualType.Redsoft;
                break;
            case InteractAction interactAction:
                gridVisualType = GridVisualType.Blue;
                break;
        }

        ShowGridPositionList(actionGridPositionRangeList, gridVisualType);
    }

    private void ShowActionEffectRange(BaseAction selectedAction, List<GridPosition> actionGridPositionRangeList) {
        // Show Action Effect Range
        GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
        if (!actionGridPositionRangeList.Contains(mouseGridPosition)) return;

        GridVisualType effectGridVisualType;
        effectGridVisualType = GridVisualType.Purple;
        switch (selectedAction.GetEffectShape()) {
            default:
            case EffectShape.Square:
                // effectGridVisualType = GridVisualType.Purple;
                //TODO: Need to refactor out the 1 and put a height variable in the actionDataSO
                ShowGridPositionRangeSquare(mouseGridPosition, selectedAction.GetEffectRange(), effectGridVisualType, selectedAction.GetMaxHeight());
                break;
            case EffectShape.Circle:
                //TODO: Need to refactor out the 1 and put a height variable in the actionDataSO
                ShowGridPositionRangeCircle(mouseGridPosition, selectedAction.GetEffectRange(), effectGridVisualType, selectedAction.GetMaxHeight());
                break;
            case EffectShape.Cross:
                ShowGridPositionRangeCross(mouseGridPosition, selectedAction.GetEffectRange(), effectGridVisualType, selectedAction.GetMaxHeight());
                break;
            case EffectShape.Single:
                break;
        }
    }

    private void UnitActionManager_OnSelectedActionChanged(object sender, EventArgs e) {
        UpdateGridVisual();
    }

    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs e) {
        //UpdateGridVisual();
    }

    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType) {
        foreach(GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialList) {
            if(gridVisualTypeMaterial.gridVisualType == gridVisualType) {
                return gridVisualTypeMaterial.material;
            }
        }

        Debug.LogError("Could not find GridVisualTypeMaterial for GridVisualType " + gridVisualType);
        return null;
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e){
        HideAllGridPositions();
    }

    private void UnitActionManager_OnSelectedUnitChanged(object sender, EventArgs e) {
        HideAllGridPositions();
    }

    private void UnitActionManager_OnSelectedGridPositionChanged(object sender, UnitActionManager.OnSelectedGridPositionChangedEventArgs e) {
        UpdateGridVisual();
        List<GridPosition> gridPositionList = new List<GridPosition>() {e.gridPosition};
        ShowGridPositionList(gridPositionList, GridVisualType.Blue);
    }
}
