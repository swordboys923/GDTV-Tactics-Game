using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionResourceSystem : MonoBehaviour {

    public static event EventHandler OnAnyResourceChanged;

    [SerializeField] protected int resource;
    protected int resourceMax;

    public abstract bool HasSufficientResource(int amount);

    public virtual float GetResourceNormalized() {
        return (float)resource / resourceMax;
    }

    public virtual int GetResource() {
        return resource;
    }

    public virtual int GetResourceMax() {
        return resourceMax;
    }

    public abstract void ProcessActionResource(int resourceAmount);

    protected void ProcessComplete(){
        OnAnyResourceChanged?.Invoke(this,EventArgs.Empty);
    }
}
