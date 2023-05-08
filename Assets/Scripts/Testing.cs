using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

    [SerializeField] ParticleSystem particleEffect;
    [SerializeField] int range;
    List<GridPosition> gridPositionList;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.T)) {
            GridAnimationManager.Instance.SpawnEffect(GetGridPositionRadius(), particleEffect);
        }
    }

    private GridPosition[] GetGridPositionRadius(){
        gridPositionList = new List<GridPosition>();
        Unit unit = TurnManager.Instance.GetCurrentTurnUnit();
        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -range; x <= range; x++) {
            for (int z = -range; z <= range; z++) {
                GridPosition gridPosition = new GridPosition(x,z) + unitGridPosition;
                if(LevelGrid.Instance.IsValidGridPosition(gridPosition)){
                    gridPositionList.Add(gridPosition);
                }
            }
        }
        return gridPositionList.ToArray();
    }
}
