using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStateMachine : MonoBehaviour {

    private Stack<TestState> stateStack;
    private TestState currentState;

    public void PushStateStack(TestState state) {
        stateStack.Push(state);
    }

    private void Update() {
        if(currentState == null) return;
        currentState.Update();
    }
}
