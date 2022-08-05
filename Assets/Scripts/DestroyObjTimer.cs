using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjTimer : MonoBehaviour {

    [SerializeField] private float timerMax;

    private void Start() {
        timerMax = 5f;
    }

    private void Update() {
        timerMax -= Time.deltaTime;
        if(timerMax <= 0) {
            Destroy(gameObject);
        }
    }
}


