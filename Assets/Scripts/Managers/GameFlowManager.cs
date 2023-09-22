using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour {
        public enum State {
        TurnStart,
        RoutingCleanup,
        Cutscene,
        PrepUnitTurn,
        UnitTurn,
        TurnEnd,
    }

    public event EventHandler OnTurnStart;
    public event EventHandler OnRoutingCleanup;
    public event EventHandler OnCutscene;
    public event EventHandler OnPrepUnitTurn;
    public event EventHandler OnUnitTurn;
    public event EventHandler OnTurnEnd;

    List<IManage> managerList = new List<IManage>();


    private State state;
    private float stateTimer;

    private void Start() {
        
    }


    // private void Update() {
    //     //if(!isActive) return;

    //     stateTimer -= Time.deltaTime;
        
    //     switch(state) {
    //         case State.TurnStart:
    //             Debug.Log("TurnStart");
    //             break;
    //         case State.RoutingCleanup:
    //             Debug.Log("RoutingCleanup");
    //             break;
    //         case State.Cutscene:
    //             Debug.Log("Cutscene");
    //             break;
    //         case State.UnitTurn:
    //             Debug.Log("UnitTurn");
    //             break;
    //         case State.TurnEnd:
    //             Debug.Log("TurnEnd");
    //             break;
    //     }

    //     if (stateTimer <=0f) {
    //         NextState();    
    //     }
    // }
    private void NextState(){
        switch(state) {
            case State.TurnStart:
                if (stateTimer <=0f) {
                    //turnOrderList = GenerateTurnList();
                    state = State.RoutingCleanup;
                    float routingStateTime = .1f;
                    stateTimer = routingStateTime;
                }
                break;
            case State.RoutingCleanup:
                if (stateTimer <=0f) {
                    state = State.Cutscene;
                    float cutsceneStateTime = .5f;
                    stateTimer = cutsceneStateTime;
                }
                break;
            case State.Cutscene:
                if (stateTimer <=0f) {
                    state = State.UnitTurn;
                    float unitTurnStateTime = .5f;
                    stateTimer = unitTurnStateTime;
                    //SetNextCurrentTurnUnit();
                }
                break;
            case State.UnitTurn:
                break;
            case State.TurnEnd:
                break;
        }
    }

}

