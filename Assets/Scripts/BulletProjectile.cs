using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour {

    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform bulletHitVFXPrefab;
    private Vector3 targetPosition;
    public void Setup(Vector3 targetPosition) {
        this.targetPosition = targetPosition;
    }

    private void Update() {
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        float sqrMagnitudeBeforeMoving = (transform.position - targetPosition).sqrMagnitude;
        float moveSpeed = 200f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        float sqrMagnitudeAfterMoving = (transform.position - targetPosition).sqrMagnitude;

        if(sqrMagnitudeBeforeMoving < sqrMagnitudeAfterMoving) {
            transform.position = targetPosition;
            trailRenderer.transform.parent = null;
            Destroy(gameObject);
            Instantiate(bulletHitVFXPrefab, targetPosition, Quaternion.identity);
        }
    }
}
