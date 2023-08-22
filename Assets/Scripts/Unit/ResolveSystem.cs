using System;
using System.Collections.Generic;
using UnityEngine;

public class ResolveSystem : MonoBehaviour {

    // public static event EventHandler OnAnyResolveChanged;
    public event EventHandler OnResolveBroken;
    public event EventHandler OnResolveChanged;
    [SerializeField] private int resolve = 100;
    private int resolveMax;

    private void Awake() {
        resolveMax = resolve;
    }

    public void ProcessResolveChange(int lossAmount) {
        resolve -= lossAmount;
        if(resolve < 0) {
            resolve = 0;
        }
        OnResolveChanged?.Invoke(this, EventArgs.Empty);
        // OnAnyResolveChanged?.Invoke(this,EventArgs.Empty);
        
        if (resolve == 0) {
            Break();
        }
    }

    private void Break() {
        OnResolveBroken?.Invoke(this,EventArgs.Empty);
    }

    public float GetResolveNormalized() {
        return (float)resolve / resolveMax;
    }

    public int GetResolve() {
        return resolve;
    }

    public int GetResolveMax() {
        return resolveMax;
    }
}
