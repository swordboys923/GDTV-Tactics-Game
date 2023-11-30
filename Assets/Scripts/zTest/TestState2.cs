using UnityEngine;
public class TestState2 : TestState {
    public override void Enter() {
        Debug.Log("Entering TestState2");
    }

    public override void Exit() {
        Debug.Log("Exiting TestState2");
    }

    public override void Eject() {
        Debug.Log("Ejecting TestState2");
    }

    public override void Update() {
        Debug.Log("Updating TestState2");
    }
}