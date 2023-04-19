using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridPositionShapes {

public static List<GridPosition> ShowGridPositionRangeCircle(GridPosition sourceGridPosition, int horizontalRange, bool includeSourceGridPosition, int verticalRange = int.MaxValue) {
        List<GridPosition> gridPositionList = new List<GridPosition>();
        for(int x = -horizontalRange; x <= horizontalRange; x++) {
            for(int z = -horizontalRange; z<= horizontalRange; z++) {
                GridPosition testGridPosition = sourceGridPosition + new GridPosition(x,z);
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                if(testGridPosition == sourceGridPosition && !includeSourceGridPosition) continue;
                if(LevelGrid.Instance.GetAbsGridPositionHeightDifference(sourceGridPosition,testGridPosition) > verticalRange) continue;
                
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(testDistance > horizontalRange) continue;

                gridPositionList.Add(testGridPosition);
            }
        }
        return gridPositionList;
    }

public static List<GridPosition> ShowGridPositionRangeSquare(GridPosition sourceGridPosition, int horizontalRange, bool includeSourceGridPosition, int verticalRange = int.MaxValue) {
        List<GridPosition> gridPositionList = new List<GridPosition>();
        for(int x = -horizontalRange; x <= horizontalRange; x++) {
            for(int z = -horizontalRange; z<= horizontalRange; z++) {
                GridPosition testGridPosition = sourceGridPosition + new GridPosition(x,z);
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                if(testGridPosition == sourceGridPosition && !includeSourceGridPosition) continue;
                if(LevelGrid.Instance.GetAbsGridPositionHeightDifference(sourceGridPosition,testGridPosition) > verticalRange) continue;

                gridPositionList.Add(testGridPosition);
            }
        }
        return gridPositionList;
    }

public static List<GridPosition> ShowGridPositionRangeCross(GridPosition sourceGridPosition, int horizontalRange, bool includeSourceGridPosition, int verticalRange = int.MaxValue) {
        List<GridPosition> gridPositionList = new List<GridPosition>();
        for(int x = -horizontalRange; x <= horizontalRange; x++) {
            for(int z = -horizontalRange; z<= horizontalRange; z++) {
                GridPosition testGridPosition = sourceGridPosition + new GridPosition(x,z);
                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                if(testGridPosition == sourceGridPosition && !includeSourceGridPosition) continue;
                if(testGridPosition.x != sourceGridPosition.x && testGridPosition.z != sourceGridPosition.z) continue;
                if(LevelGrid.Instance.GetAbsGridPositionHeightDifference(sourceGridPosition,testGridPosition) > verticalRange) continue;

                gridPositionList.Add(testGridPosition);
            }
        }
        return gridPositionList;
    }

}
