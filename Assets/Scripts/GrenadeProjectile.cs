using System.Collections;
using System;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour {
    public static event EventHandler OnAnyGrenadeExploded;

    [SerializeField] private Transform grenadeExplodeVFXPrefab;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private AnimationCurve arcYAnimationCurve;
    private Action onGrenadeBehaviorComplete;
    private Vector3 targetPosition;
    private float totalDistance;
    private Vector3 posXZ;
    private void Update() {
        Vector3 moveDir = (targetPosition - posXZ).normalized;
        float moveSpeed = 15f;
        posXZ += moveDir * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(posXZ, targetPosition);
        float distanceNormalized = 1 - distance / totalDistance;

        float maxHeight = totalDistance / 4f;
        float posY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(posXZ.x, posY, posXZ.z);

        float reachedTargetDistance = .2f;
        if (Vector3.Distance(posXZ, targetPosition) < reachedTargetDistance) {
            float damageRadius = 4f;
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);
            foreach (Collider col in colliderArray){
                if(col.TryGetComponent<Unit>(out Unit targetUnit)){
                    targetUnit.ProcessHealthChange(30);
                }
                if(col.TryGetComponent<DestructibleCrate>(out DestructibleCrate crate)) {
                    crate.Damage();
                }
            }
            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);

            trailRenderer.transform.parent = null;
            Instantiate(grenadeExplodeVFXPrefab, targetPosition + Vector3.up * 1f, Quaternion.identity);
            Destroy(gameObject);
            onGrenadeBehaviorComplete();
        }
    }
    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviorComplete){
        this.onGrenadeBehaviorComplete = onGrenadeBehaviorComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        posXZ = transform.position;
        posXZ.y = 0;
        totalDistance = Vector3.Distance(posXZ, targetPosition);
    }
}
