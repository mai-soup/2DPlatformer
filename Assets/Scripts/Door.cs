using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    [SerializeField]
    private Transform           _prevRoom;
    [SerializeField]
    private Transform           _nextRoom;
    [SerializeField]
    private CameraController    _cam;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            if (collision.transform.position.x < transform.position.x) {
                // player entered from left
                _cam.MoveToNewRoom(_nextRoom);
            } else {
                // otherwise, player entered from right
                _cam.MoveToNewRoom(_prevRoom);
            }
        }
    }
}
