using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour {
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;
    [SerializeField] private Transform rifleTransform;
    [SerializeField] private Transform swordTransform;
    private UnitActionSystem unitActionSystem;

    private void Start() {
        unitActionSystem = GetComponent<UnitActionSystem>();
        foreach(BaseAction action in unitActionSystem.GetBaseActionArray()){
            SubscribeToAnimEvent(action, action.GetActionType());
        }
        EquipSword();
    }

    private void SubscribeToAnimEvent(BaseAction action, ActionType actionType)  {
        switch (actionType){
            case ActionType.Movement:
                MoveAction moveAction = action as MoveAction;
                moveAction.OnStartMoving += MoveAction_OnStartMoving;
                moveAction.OnStopMoving += MoveAction_OnStopMoving;
                break;
            case ActionType.Melee:
                SwordAction swordAction = action as SwordAction;
                swordAction.OnSwordActionStarted += SwordAction_OnSwordActionStarted;
                swordAction.OnSwordActionCompleted += SwordAction_OnSwordActionCompleted;
                break;
            case ActionType.Spell:
                AOESpellAction fireAction = action as AOESpellAction;
                fireAction.OnSpellCharging += FireAction_OnSpellCharging;
                fireAction.OnSpellCasting += FireAction_OnSpellCasting;
                fireAction.OnSpellCooling += FireAction_OnSpellCooling;
                break;
            default:
                break;
        }
    }

    private void FireAction_OnSpellCharging(object sender, EventArgs e) {
        Debug.Log("FireActionCharging!");
        BaseAction action = sender as BaseAction;
        string chargingString = action.GetAnimationString() + "Charging";
        animator.SetTrigger(chargingString);
    }

    private void FireAction_OnSpellCasting(object sender, EventArgs e) {
        BaseAction action = sender as BaseAction;
        string chargingString = action.GetAnimationString() + "Casting";
    }

    private void FireAction_OnSpellCooling(object sender, EventArgs e) {
        BaseAction action = sender as BaseAction;
        string chargingString = action.GetAnimationString() + "Cooling";
    }

    private void SwordAction_OnSwordActionStarted(object sender, EventArgs e) {
        EquipSword();
        BaseAction action = sender as BaseAction;
        
        animator.SetTrigger(action.GetAnimationString());
    }

    private void SwordAction_OnSwordActionCompleted(object sender, EventArgs e){
        // EquipRifle();
    }


    private void MoveAction_OnStartMoving(object sender, EventArgs e) {
        BaseAction action = sender as BaseAction;
        animator.SetBool(action.GetAnimationString(), true);
    }
    private void MoveAction_OnStopMoving(object sender, EventArgs e) {
        BaseAction action = sender as BaseAction;
        animator.SetBool(action.GetAnimationString(), false);
    }

    private void ShootAction_OnShoot (object sender, ShootAction.OnShootEventArgs e) {
        BaseAction action = sender as BaseAction;
        animator.SetTrigger(action.GetAnimationString());
        Transform bulletProjectileTransform = Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();

        targetUnitShootAtPosition.y = shootPointTransform.position.y;
        bulletProjectile.Setup(targetUnitShootAtPosition);
    }

    private void EquipSword() {
        swordTransform.gameObject.SetActive(true);
        rifleTransform.gameObject.SetActive(false);
    }

    private void EquipRifle() {
        swordTransform.gameObject.SetActive(false);
        rifleTransform.gameObject.SetActive(true);
    }
}
