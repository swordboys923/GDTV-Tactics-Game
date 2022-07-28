using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractible {

    [SerializeField] private bool isOpen;
    private GridPosition gridPosition;
    private Animator animator;
    private Action onInteractComplete;
    private float timer;
    private bool isActive;

    private void Awake() {
        animator = GetComponent<Animator>();
    }
    private void Start() {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractibleAtGridPosition(gridPosition, this);

        animator.speed = 100;
        if(isOpen){
            OpenDoor();
        } else {
            CloseDoor();
        }
    }
    private void Update() {
        if(!isActive) return;

        timer -= Time.deltaTime;
        if (timer <=0f ){
            isActive = false;
            onInteractComplete();
        }
    }
    public void Interact(Action onInteractComplete) {
        animator.speed = 1;
        this.onInteractComplete = onInteractComplete;
        isActive = true;
        timer = 1f;
        if(isOpen){
            CloseDoor();
        } else {
            OpenDoor();
        }
    }

    private void OpenDoor(){
        isOpen = true;
        animator.SetBool("isOpen",true);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);
    }

    private void CloseDoor() {
        isOpen = false;
        animator.SetBool("isOpen",false);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
    }
}

