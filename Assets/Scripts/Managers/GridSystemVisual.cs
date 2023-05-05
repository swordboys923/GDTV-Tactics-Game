using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    //TODO: At each position, I need to sample the grid at that position and find the height, then spawn the visual at that height.
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
        foreach (var gridPosition in gridPositionList) {
            if(!LevelGrid.Instance.IsValidGridPosition(gridPosition))continue;
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show(GetGridVisualTypeMaterial(gridVisualType));
        }
    }

    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType) {
        List<GridPosition> gridPositionList = new List<GridPosition>();
        for(int x = -range; x <= range; x++) {
            for(int z = -range; z<= range; z++) {
                GridPosition testGridPosition = gridPosition + new GridPosition(x,z);
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(testDistance > range) continue;

                gridPositionList.Add(testGridPosition);
            }
        }
        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    private void ShowGridPositionRangeSquare(GridPosition gridPosition, int horizontalRange, GridVisualType gridVisualType, int verticalRange = int.MaxValue) {
        List<GridPosition> gridPositionList = new List<GridPosition>();
        for(int x = -horizontalRange; x <= horizontalRange; x++) {
            for(int z = -horizontalRange; z<= horizontalRange; z++) {
                GridPosition testGridPosition = gridPosition + new GridPosition(x,z);
                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                if(testGridPosition == gridPosition) continue;
                if(LevelGrid.Instance.GetAbsGridPositionHeightDifference(gridPosition,testGridPosition) > verticalRange) continue;

                gridPositionList.Add(testGridPosition);
            }
        }
        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    private void ShowGridPositionRangeCross(GridPosition gridPosition, int horizontalRange, GridVisualType gridVisualType, int verticalRange = int.MaxValue) {
        List<GridPosition> gridPositionList = new List<GridPosition>();
        for(int x = -horizontalRange; x <= horizontalRange; x++) {
            for(int z = -horizontalRange; z<= horizontalRange; z++) {
                GridPosition testGridPosition = gridPosition + new GridPosition(x,z);
                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                if(testGridPosition == gridPosition) continue;
                if(testGridPosition.x != gridPosition.x && testGridPosition.z != gridPosition.z) continue;
                if(LevelGrid.Instance.GetAbsGridPositionHeightDifference(gridPosition,testGridPosition) > verticalRange) continue;

                gridPositionList.Add(testGridPosition);
            }
        }
        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    private void UpdateGridVisual() {
        HideAllGridPositions();
        Unit currentTurnUnit = TurnManager.Instance.GetCurrentTurnUnit();
        BaseAction selectedAction = UnitActionManager.Instance.GetSelectedAction();
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
                gridVisualType = GridVisualType.Red;
                break;
            case SwordAction swordAction:
                gridVisualType = GridVisualType.Red;
                break;
            case InteractAction interactAction:
                gridVisualType = GridVisualType.Blue;
                break;
        }
        
        if (selectedAction != null){
            ShowGridPositionList(selectedAction.GetActionGridPositionRangeList(), gridVisualType);
        }


        GridVisualType effectGridVisualType;
        GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
        switch(selectedAction.GetEffectShape()) {
            default:
            case EffectShape.Square:
                effectGridVisualType = GridVisualType.Purple;
                ShowGridPositionRangeSquare(mouseGridPosition,selectedAction.GetEffectRange(),effectGridVisualType,1);
                //ShowGridPositionList(mouseGridPosition,effectGridVisualType);
                break;
            case EffectShape.Circle:
                break;
            case EffectShape.Cross:
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

        Debug.LogError("Could not find GridVisualTypeMaterial for GridVIsualType " + gridVisualType);
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
        ShowGridPositionList(gridPositionList, GridVisualType.Purple);
    }
}
