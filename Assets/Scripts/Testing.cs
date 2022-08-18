using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {
    
    [SerializeField] private UnitActionSystemUI unitActionSystemUI;
    CanvasGroup canvasGroup;
    bool isCanvasActive;

    private void Start() {
        canvasGroup = unitActionSystemUI.GetComponent<CanvasGroup>();
        isCanvasActive = false;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.T)){
            SetActive(!isCanvasActive);
        }
    }

    private void SetActive(bool isActive) {
        if (isActive == true) {
            canvasGroup.alpha = 1;
        } else {
            canvasGroup.alpha = 0;
        }
        canvasGroup.interactable = isActive;
        canvasGroup.blocksRaycasts = isActive;

        isCanvasActive = isActive;
    }

}
