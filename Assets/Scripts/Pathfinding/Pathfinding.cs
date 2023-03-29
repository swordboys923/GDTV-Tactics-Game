using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {
    public static Pathfinding Instance { get; private set; }

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] private LayerMask terrainLayerMask;
    private int width;
    private int height;
    private float cellSize;
    private GridSystem<PathNode> gridSystem;

 
    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There's more than one Pathfinding! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance  = this;
    }

    public void Setup(int width, int height, float cellSize){
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridSystem = new GridSystem<PathNode>(width, height, cellSize, 
            (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition), terrainLayerMask);
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
        
        for(int x = 0; x < width; x++) {
            for(int z = 0; z < height; z++) {
                GridPosition gridPosition = new GridPosition(x,z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float raycastOffsetDistance = 5f;
                if(Physics.Raycast(worldPosition + Vector3.up * raycastOffsetDistance, Vector3.down,raycastOffsetDistance*2,obstaclesLayerMask)){
                    //TODO: Implement movement cost with colliders placed at world positions.
                    GetNode(x,z).SetIsWalkable(false);
                }
            }
        }
    }

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLength) {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = gridSystem.GetGridObject(endGridPosition);

        openList.Add(startNode);

        //TODO: use the local width height?
        for(int x = 0; x < gridSystem.GetWidth(); x++) {
            for(int z = 0; z < gridSystem.GetDepth(); z++) {
                GridPosition gridPosition = new GridPosition(x,z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }

        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();

        while(openList.Count > 0) {
            PathNode currentNode = GetLowestFCostPathNode(openList);
            
            if(currentNode == endNode) {
                pathLength = endNode.GetFCost();
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach(PathNode neighborNode in GetNeighborList(currentNode)) {
                if(closedList.Contains(neighborNode)) continue;
                if(!neighborNode.IsWalkable()){
                    closedList.Add(neighborNode);
                    continue;
                }
                // //FIXME: Clean this up. THis is only here to test working the height into the pathfinding.
                // if(Mathf.Abs(neighborNode.GetGridPosition().y - currentNode.GetGridPosition().y)> 1) {
                //     closedList.Add(neighborNode);
                //     continue;
                // }

                int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighborNode.GetGridPosition());

                if(tentativeGCost < neighborNode.GetGCost()) {
                    neighborNode.SetCameFromPathNode(currentNode);
                    neighborNode.SetGCost(tentativeGCost);
                    neighborNode.SetHCost(CalculateDistance(neighborNode.GetGridPosition(), endGridPosition));
                    neighborNode.CalculateFCost();
                }

                if (!openList.Contains(neighborNode)){
                    openList.Add(neighborNode);
                }
            }
        }
        pathLength = 0;
        return null;
    }

    public int CalculateDistance(GridPosition a, GridPosition b){
        GridPosition gridPositionDistance = a - b;
        int heightCostMultiplier = 1;
        int xDistnace = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int yDistance = Mathf.Abs(gridPositionDistance.y);
        int remaining = Mathf.Abs(xDistnace - zDistance);
        if (yDistance > 1) {
            heightCostMultiplier += Mathf.RoundToInt(yDistance);
        }
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistnace,zDistance) + MOVE_STRAIGHT_COST * remaining * heightCostMultiplier;
    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList){
        PathNode lowestFCostPathNode = pathNodeList[0];

        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if(pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost()) {
                lowestFCostPathNode = pathNodeList[i];
            }
        }
        return lowestFCostPathNode;
    }

    private PathNode GetNode(int x, int z){
        return gridSystem.GetGridObject(new GridPosition(x,z));
    }

    private List<PathNode> GetNeighborList(PathNode currentNode) {
        List<PathNode> neighborList = new List<PathNode>();

        GridPosition gridPosition = currentNode.GetGridPosition();
        //TODO: Add in the height check here instead?
        if(gridPosition.x-1 >=0) {
            PathNode leftNeighborNode = GetNode(gridPosition.x -1, gridPosition.z);
            //FIXME: Magic Numbers in all of these height calculations. Abstract entire if statement out into a bool function.
            if(Mathf.Abs(leftNeighborNode.GetGridPosition().y - currentNode.GetGridPosition().y)<= 1) neighborList.Add(leftNeighborNode);
            if(gridPosition.z-1 >=0){
                PathNode leftDownNeighborNode = GetNode(gridPosition.x -1, gridPosition.z -1);
                if(Mathf.Abs(leftDownNeighborNode.GetGridPosition().y - currentNode.GetGridPosition().y)<= 1) neighborList.Add(leftDownNeighborNode);
            }
            if(gridPosition.z+1 < gridSystem.GetDepth()){
                PathNode leftUpNeighborNode = GetNode(gridPosition.x -1, gridPosition.z+1);
                if(Mathf.Abs(leftUpNeighborNode.GetGridPosition().y - currentNode.GetGridPosition().y)<= 1) neighborList.Add(leftUpNeighborNode);
            }
        }

        if(gridPosition.x+1 < gridSystem.GetWidth()){
            PathNode rightNeighborNode = GetNode(gridPosition.x + 1, gridPosition.z);
            if(Mathf.Abs(rightNeighborNode.GetGridPosition().y - currentNode.GetGridPosition().y)<= 1) neighborList.Add(rightNeighborNode);
            if(gridPosition.z-1 >=0){
                PathNode rightDownNeighborNode = GetNode(gridPosition.x + 1, gridPosition.z-1);
                if(Mathf.Abs(rightDownNeighborNode.GetGridPosition().y - currentNode.GetGridPosition().y)<= 1) neighborList.Add(rightDownNeighborNode);
            }
            if(gridPosition.z+1 < gridSystem.GetDepth()){
                PathNode rightUpNeighborNode = GetNode(gridPosition.x + 1, gridPosition.z+1);
                if(Mathf.Abs(rightUpNeighborNode.GetGridPosition().y - currentNode.GetGridPosition().y)<= 1) neighborList.Add(rightUpNeighborNode);
            }       
        }

        if(gridPosition.z-1 >=0){
            PathNode downNeighborNode = GetNode(gridPosition.x, gridPosition.z - 1);
            if(Mathf.Abs(downNeighborNode.GetGridPosition().y - currentNode.GetGridPosition().y)<= 1) neighborList.Add(downNeighborNode);
        }
        if(gridPosition.z+1 < gridSystem.GetDepth()){
            PathNode upNeighborNode = GetNode(gridPosition.x, gridPosition.z + 1);
            if(Mathf.Abs(upNeighborNode.GetGridPosition().y - currentNode.GetGridPosition().y)<= 1) neighborList.Add(upNeighborNode);
        }
        
        return neighborList;
    }

    private List<GridPosition> CalculatePath(PathNode endNode){
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.GetCameFromPathNode() != null) {
            pathNodeList.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }

        pathNodeList.Reverse();

        List<GridPosition> gridPositionList = new List<GridPosition>();
        foreach(PathNode pathNode in pathNodeList){
            gridPositionList.Add(pathNode.GetGridPosition());
        }

        return gridPositionList;
    }

    public void SetIsWalkableGridPosition(GridPosition gridPosition, bool isWalkable){
        gridSystem.GetGridObject(gridPosition).SetIsWalkable(isWalkable);
    }
    public bool IsWalkableGridPosition(GridPosition gridPosition){
        return gridSystem.GetGridObject(gridPosition).IsWalkable();
    }

    public bool HasPath(GridPosition startPosition, GridPosition endGridPosition){
        return FindPath(startPosition, endGridPosition, out int pathLength) != null;
    }

    public int GetPathLength(GridPosition startPosition, GridPosition endGridPosition){
        FindPath(startPosition, endGridPosition, out int pathLength);
        return pathLength;
    }
}
