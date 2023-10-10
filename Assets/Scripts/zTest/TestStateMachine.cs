using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestStateMachine : MonoBehaviour {

    private Stack<TestState> stateStack;
    private TestState currentState;
    private bool isPaused;

    private void Start() {
        currentState = new TestState1();
        stateStack = new Stack<TestState>();
    }

    public void PushStateStack(TestState state) {
        if(isPaused) return;

        stateStack.Push(currentState);
        currentState = state;
        currentState.Enter();
    }

    public void PopStateStack() {
        if(isPaused) return;
        
        currentState?.Exit();
        if(stateStack.TryPop(out TestState state)){
            currentState = state;
        } else {
            currentState = null;
        }
    }

    public void Pause() {
        isPaused = !isPaused;
    }

    private void Update() {
        if(currentState == null || isPaused) return;
        currentState.Update();
    }
}
