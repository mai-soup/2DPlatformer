using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHorizontal : MonoBehaviour {
    // vars
    [SerializeField]
    private float _damage;
    [SerializeField]
    private float _movementDistance;
    [SerializeField]
    private float _speed;
    private bool  _isMovingLeft;
    private float _leftBound;
    private float _rightBound;

    private void Awake() {
        _leftBound = transform.position.x - _movementDistance;
        _rightBound = transform.position.x + _movementDistance;
    }

    private void Update() {
        if (_isMovingLeft) {
            // if hasnt hit left bound yet
            if (transform.position.x > _leftBound) {
                transform.position = new Vector3(
                    transform.position.x - _speed * Time.deltaTime,
                    transform.position.y,
                    transform.position.z);
            } else {
                // if hit bound, gotta start moving right
                _isMovingLeft = false;
            }
        } else {
            // if hasnt hit right bound yet
            if (transform.position.x < _rightBound) {
                transform.position = new Vector3(
                    transform.position.x + _speed * Time.deltaTime,
                    transform.position.y,
                    transform.position.z);
            } else {
                // if hit bound, gotta start moving left
                _isMovingLeft = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            collision.GetComponent<Health>().TakeDamage(_damage);
        }
    }
}
