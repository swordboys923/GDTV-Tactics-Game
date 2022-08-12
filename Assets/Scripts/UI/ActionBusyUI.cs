using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour {

    void Start() {
        UnitActionManager.Instance.OnBusyChanged += UnitActionManager_OnBusyChanged;
        gameObject.SetActive(false);
    }

    private void UnitActionManager_OnBusyChanged(object sender, bool isBusy) {
        if(isBusy) {
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
