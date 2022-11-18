using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour {
    [SerializeField] private AudioClip _checkpointSound;
    private Transform _lastCheckpoint;
    private Health _playerHealth;

    public void Awake() {
        _playerHealth = GetComponent<Health>();
    }

    public void Respawn() {
        // move player to last checkpoint
        transform.position = _lastCheckpoint.position;
        // restore health, reset animation
        _playerHealth.Respawn();

        // move cam back to checkpoint
        // NB: checkpoint has to be child of a room
        Camera.main.GetComponent<CameraController>()
            .MoveToNewRoom(_lastCheckpoint.parent);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.tag == "Checkpoint") {
            _lastCheckpoint = collision.transform;
            SoundManager.instance.PlaySound(_checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("activate");
        }
    }
}
