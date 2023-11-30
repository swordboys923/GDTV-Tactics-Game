using UnityEngine;
public class TestState1 : TestState {
    public override void Enter() {
        Debug.Log("Entering TestState1");
    }

    public override void Exit() {
        Debug.Log("Exiting TestState1");
    }

    public override void Eject() {
        Debug.Log("Ejecting TestState1");
    }

    public override void Update() {
        Debug.Log("Updating TestState1");
    }
}