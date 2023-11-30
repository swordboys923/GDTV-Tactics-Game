using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class TestStateMachine : MonoBehaviour {

    private Stack<TestState> stateStack;
    private bool isPaused;

    private void Start() {
        stateStack = new Stack<TestState>();
        stateStack.Push(new TestState1());
    }

    public void PushStateStack(TestState state) {
        if(isPaused) return;

        stateStack.Push(state);
    }

    public void PopStateStack() {
        if(isPaused) return;
        isPaused = true;

        if(stateStack.TryPop(out TestState state)) {
            state.Eject();
        }

        isPaused = false;
    }

    public void Pause() {
        isPaused = !isPaused;
    }

    public void PrintState() {
        Debug.Log(stateStack.Peek());
    }

    private void Update() {
        if(stateStack.Count == 0 || isPaused) return;
        stateStack.Peek().Update();
    }
}
