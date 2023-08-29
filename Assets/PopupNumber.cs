using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupNumber : MonoBehaviour {
    [SerializeField] Color healColor;
    [SerializeField] Color damageColor;
    private int amount;
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;


    private void Awake() {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(int amount) {
        textMesh.text = amount.ToString();
        
        textColor = textMesh.color;
        if(amount < 0) textColor = healColor;
        if(amount > 0) textColor = damageColor;
        textMesh.color = textColor;
        
        disappearTimer = .2f;
    }

    void Update() {
        float moveYSpeed = 2f;
        transform.position += new Vector3(0,moveYSpeed, 0) * Time.deltaTime;

        disappearTimer -= Time.deltaTime;
        if(disappearTimer < 0) {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0) {
                Destroy(gameObject);
            }
        }
    }
}
