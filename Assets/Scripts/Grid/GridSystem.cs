using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem<TGridObject>{

    private LayerMask terrainLayerMask;
    private int width;
    private int depth;
    private float cellSize;
    private TGridObject[,] gridObjectArray;
    public GridSystem(int width, int depth, float cellSize, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject) {
        this.width = width;
        this.depth = depth;
        this.cellSize = cellSize;
        gridObjectArray = new TGridObject[width, depth];
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < depth; z++) {
                GridPosition gridPosition = new GridPosition(x,z);
                gridObjectArray[x,z] = createGridObject(this, gridPosition);
            }
        }
    }

    public GridSystem(int width, int depth, float cellSize, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject, LayerMask terrainLayerMask) {
        this.width = width;
        this.depth = depth;
        this.terrainLayerMask = terrainLayerMask;
        this.cellSize = cellSize;
        gridObjectArray = new TGridObject[width, depth];
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < depth; z++) {
                GridPosition gridPosition = new GridPosition(x,z);
               
                Vector3 worldPosition = GetWorldPosition(gridPosition);
                float raycastOffsetDistance = 5f;
                RaycastHit hit;
                if(Physics.Raycast(worldPosition + Vector3.up * raycastOffsetDistance, Vector3.down, out hit ,raycastOffsetDistance*2,terrainLayerMask)){
                    //FIXME: Magic number.
                    if(hit.point.y >= .5) {
                        gridPosition.UpdateGridPositionY((int)hit.point.y);
                    }
                }
                gridObjectArray[x,z] = createGridObject(this, gridPosition);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition) {
        return new Vector3(gridPosition.x * cellSize, gridPosition.height, gridPosition.z * cellSize);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x / cellSize),
            Mathf.RoundToInt(worldPosition.z / cellSize)
        );
    }

    public void CreateDebugObjects(Transform debugPrefab) {
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < depth; z++) {
                GridPosition gridPosition = new GridPosition(x,z);
                Transform debugTranform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebugObject gridDebugObject = debugTranform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }

    public TGridObject GetGridObject(GridPosition gridPosition) {
        return gridObjectArray[gridPosition.x, gridPosition.z];
    } 

    public bool IsValidGridPosition(GridPosition gridPosition) {
        return gridPosition.x >= 0 && 
        gridPosition.z >= 0 && 
        gridPosition.x < width && 
        gridPosition.z < depth;
    }

    public int GetWidth() {
        return width;
    }
    public int GetDepth() {
        return depth;
    }
}
