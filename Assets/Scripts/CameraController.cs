using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;    
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private Vector3 targetFollowOffset;
    private CinemachineTransposer cinemachineTransposer;

    private void Start() {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }
    void Update() {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    private void HandleMovement() {
        Vector3 inputMoveDir = InputManager.Instance.GetCameraMoveVector();

        float moveSpeed = 10f;
        Vector3 moveVector = transform.forward * inputMoveDir.y + transform.right * inputMoveDir.x;

        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }

    private void HandleRotation() {
        Vector3 rotationVector = new Vector3(0,0,0);
        rotationVector.y = InputManager.Instance.GetCameraRotateAmount();

        float rotationSpeed = 100f;
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }

    private void HandleZoom() {
        float zoomIncreaseAmount = 1f;
        targetFollowOffset.y += InputManager.Instance.GetCameraZoomAmount() * zoomIncreaseAmount;
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

        float zoomSpeed = 5f;
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);
    }

    public void SetTransform(Vector3 location) {
        while(transform.position != location){
            transform.position = Vector3.Lerp(transform.position, location, Time.deltaTime * 3);
        }
    }
}
