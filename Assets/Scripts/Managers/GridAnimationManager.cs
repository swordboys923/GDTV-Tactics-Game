using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: DataManager


public class GridAnimationManager : MonoBehaviour {

    public static GridAnimationManager Instance;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There's more than one GridAnimationManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance  = this;
    }
    
    public void SpawnEffect(GridPosition[] gridPositionArray, ParticleSystem particleSystem) {

        foreach(GridPosition gridPosition in gridPositionArray){
            Instantiate(particleSystem,LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
        }
        
    }
}
