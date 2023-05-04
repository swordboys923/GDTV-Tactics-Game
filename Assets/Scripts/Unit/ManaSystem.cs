using System;
using System.Collections.Generic;
using UnityEngine;

public class ManaSystem : ActionResourceSystem {

    public event EventHandler OnDead;
    public event EventHandler OnDamaged;

    private void Awake() {
        resourceMax = resource;
    }

    public override void ProcessActionResource(int resourceAmount) {
        resource -= resourceAmount;
        if(resource < 0) {
            resource = 0;
        }
        ProcessComplete();
    }

    public override bool HasSufficientResource(int amount) {
        return resource >= amount;
    }
}
