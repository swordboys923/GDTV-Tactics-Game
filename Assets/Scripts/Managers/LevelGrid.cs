using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour {

    public static LevelGrid Instance { get; private set; }

    public event EventHandler OnAnyUnitMovedGridPosition; 

    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] float cellSize;
    [SerializeField] LayerMask terrainLayerMask;
    //TODO: Remove eventually. Debug variable only.
    [SerializeField] bool turnOnDebug = false;
    [SerializeField] Vector3 routingPositionVector3;
    [SerializeField] RoutingCoords[] routingCoords;
    private GridPosition routingGridPosition;
    private GridSystem<GridObject> gridSystem;
    private Dictionary<Faction,GridPosition> routingCoordsDict = new Dictionary<Faction, GridPosition>();
    
    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There's more than one LevelGrid! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance  = this;

        gridSystem = new GridSystem<GridObject>(width, height, cellSize, (GridSystem<GridObject> g, GridPosition gridPosition) =>  new GridObject(g, gridPosition), terrainLayerMask);

        routingGridPosition = gridSystem.GetGridPosition(routingPositionVector3);
        if (routingGridPosition == null || !gridSystem.IsValidGridPosition(routingGridPosition)) {
            Debug.LogError($"No routing GridPosition at Vector3: {routingPositionVector3}");
        }

        if(turnOnDebug) gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    private void Start() {
        Pathfinding.Instance.Setup(width, height, cellSize);
        
        foreach(RoutingCoords routingCoord in routingCoords) {
            routingGridPosition = gridSystem.GetGridPosition(routingCoord.routingCoords);
            if (routingGridPosition == null || !gridSystem.IsValidGridPosition(routingGridPosition)) {
                Debug.LogError($"No routing GridPosition at Vector3: {routingPositionVector3}");
                continue;
            }
            routingCoordsDict.Add(routingCoord.faction,routingGridPosition);
        }
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit) {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition) {
        return gridSystem.GetGridObject(gridPosition).GetUnitList();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit) {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition) {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);
        //TODO: Don't update the grid visual on each grid position movement.
        OnAnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);
    public Vector3 GetWorldPosition(GridPosition gridPosition) {
        return gridSystem.GetWorldPosition(GetGridObjectGridPosition(gridPosition));
    } 
    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);
    public int GetWidth() => gridSystem.GetWidth();
    public int GetDepth() => gridSystem.GetDepth();
    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition) {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition) {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }

    public IInteractible GetInteractibleAtGridPosition(GridPosition gridPosition) {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetInteractible();
    }

    public void SetInteractibleAtGridPosition(GridPosition gridPosition, IInteractible interactible) {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.SetInteractible(interactible);
    }

    public GridPosition GetGridObjectGridPosition(GridPosition gridPosition){
        return gridSystem.GetGridObject(gridPosition).GetGridPosition();
    }

    public int GetAbsGridPositionHeightDifference(GridPosition a, GridPosition b) {
        int aHeight = GetGridObjectGridPosition(a).height;
        int bHeight = GetGridObjectGridPosition(b).height;

        return Mathf.Abs(aHeight-bHeight);
    }

    public GridPosition GetRoutingGridPosition(Faction faction) {
        return routingCoordsDict[faction];
    }
}

[Serializable]
public class RoutingCoords {
    public Faction faction;
    public Vector3 routingCoords;
}
