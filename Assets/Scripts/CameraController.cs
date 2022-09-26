using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    // vars
    [SerializeField]
    private float   _speed = 0.5f;
    private float   _currentPosX;
    private Vector3 _velocity;

    private void Update() {
        transform.position = Vector3.SmoothDamp(
            transform.position,
            new Vector3(_currentPosX, transform.position.y, transform.position.z),
            ref _velocity,
            _speed
            );
    }

    public void MoveToNewRoom(Transform newRoom) {
        _currentPosX = newRoom.position.x;
    }
}
