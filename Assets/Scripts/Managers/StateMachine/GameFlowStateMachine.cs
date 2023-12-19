using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowStateMachine : MonoBehaviour {

    private Stack<GameState> stateStack;
    private bool isPaused;

    private void Awake() {
        stateStack = new Stack<GameState>();
    }

    public void PushStateStack(GameState state) {
        if(isPaused) return;

        stateStack.Push(state);
    }

    public void PopStateStack() {
        if(isPaused) return;
        isPaused = true;

        if(stateStack.TryPop(out GameState state)) {
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
