using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour {
    [Header ("Patrol")]
    [SerializeField] private Transform _leftEnd;
    [SerializeField] private Transform _rightEnd;
    [SerializeField] private float _idleTime;

    [Header("Enemy")]
    [SerializeField] private Transform _enemy;
    [SerializeField] private float _enemySpeed;
    [SerializeField] private Animator _animator;

    private Vector3 _initialScale;
    private bool _isMovingLeft;
    private float _idleCountdown;

    private void Awake() {
        _initialScale = _enemy.localScale;
    }

    private void OnDisable() {
        _animator.SetBool("isMoving", false);
    }

    private void Update() {
        if (_isMovingLeft && (_enemy.position.x >= _leftEnd.position.x)) {
            MoveInDirection(-1);
        } else if (!_isMovingLeft && (_enemy.position.x <= _rightEnd.position.x)) {
            MoveInDirection(1);
        } else {
            // if reached an endpoint, idle
            _animator.SetBool("isMoving", false);
            _idleCountdown += Time.deltaTime;

            // if idle time ended, change directions
            if (_idleCountdown > _idleTime) {
                _isMovingLeft = !_isMovingLeft;
            }
        }
    }

    private void MoveInDirection(int direction) {
        _idleCountdown = 0;
        _animator.SetBool("isMoving", true);

        // make enemy face given direction
        _enemy.localScale = new Vector3(
            Mathf.Abs(_initialScale.x) * direction,
            _initialScale.y,
            _initialScale.z
            );

        // move in faced direction
        _enemy.position = new Vector3(
            _enemy.position.x + Time.deltaTime * direction * _enemySpeed,
            _enemy.position.y,
            _enemy.position.z
            );
    }
}
