using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeHead : EnemyDamage {
    [SerializeField] private LayerMask LAYER_PLAYER;
    [SerializeField] private float _speed;
    [SerializeField] private float _visionRange;
    [SerializeField] private float _attackDelay;
    private Vector3 _destination;
    private bool _isAttacking;
    private float _attackTimer;
    private Vector3[] _directions = new Vector3[4];

    private void OnEnable() {
        Stop();
    }

    private void Update() {
        if (_isAttacking) {
            transform.Translate(_destination * Time.deltaTime * _speed);
        } else {
            _attackTimer += Time.deltaTime;
            if (_attackTimer > _attackDelay) {
                CheckForPlayer();
            }
        }
    }

    private void CheckForPlayer() {
        CalculateDirections();

        // check if player is visible in any of the 4 directions
        for (int i = 0; i < _directions.Length; i++) {
            RaycastHit2D hit = Physics2D.Raycast(transform.position,
                _directions[i], _visionRange, LAYER_PLAYER);

            if (hit.collider != null && !_isAttacking) {
                _isAttacking = true;
                _destination = _directions[i];
                _attackTimer = 0;
            }
        }
    }

    private void CalculateDirections() {
        _directions[0] = transform.right * _visionRange;
        _directions[1] = -transform.right * _visionRange;
        _directions[2] = transform.up * _visionRange;
        _directions[3] = -transform.up * _visionRange;
    }

    private new void OnTriggerEnter2D(Collider2D collision) {
        base.OnTriggerEnter2D(collision);
        // stop when hitting something
        Stop();
    }

    private void Stop() {
        _destination = transform.position; // stop the movement
        _isAttacking = false;
    }
}
