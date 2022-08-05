using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DestructibleCrate : MonoBehaviour {
    public static event EventHandler OnAnyDestroyed;
    [SerializeField] private Transform crateDestroyedPrefab;
    private GridPosition gridPosition;

    private void Start() {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public GridPosition GetGridPosition() {
        return gridPosition;
    }
    public void Damage() {
        Transform crateDestroyedTransform = Instantiate(crateDestroyedPrefab, transform.position, transform.rotation);
        Vector3 randomDir = new Vector3(Random.Range(-1f,1f), 0, Random.Range(-1f,1f));
        ApplyExplosionToChildren(crateDestroyedTransform, 150f, transform.position + randomDir, 10f);
        
        Destroy(gameObject);
        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
    }

    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange) {
        foreach (Transform child in root) {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody)) {
                childRigidbody.AddExplosionForce(explosionForce,explosionPosition,explosionRange);
            }
            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
