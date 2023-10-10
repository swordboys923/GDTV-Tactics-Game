using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

    [SerializeField] TestStateMachine testStateMachine;

    TestState[] testStateArray = new TestState[2];
    private void Start() {
        TestState1 testState1 = new TestState1();
        TestState2 testState2 = new TestState2();
        testStateArray[0] = testState1;
        testStateArray[1] = testState2;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.T)) {
            testStateMachine.PushStateStack(testStateArray[1]);
        }
        if(Input.GetKeyDown(KeyCode.R)) {
            testStateMachine.PopStateStack();
        }
        if(Input.GetKeyDown(KeyCode.E)) {
            testStateMachine.Pause();
        }
    }
}
