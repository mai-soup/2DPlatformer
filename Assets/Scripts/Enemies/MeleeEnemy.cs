using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour {
    [SerializeField] private float _attackCooldown;
    [SerializeField] private int _damage;
    [SerializeField] private BoxCollider2D _boxColl;
    [SerializeField] private LayerMask LAYER_PLAYER;
    [SerializeField] private float _visionRange;
    [SerializeField] private float _colliderDistance;
    private float _cooldownTimer = Mathf.Infinity;
    private Animator _anim;

    private void Awake() {
        _anim = GetComponent<Animator>();
    }

    private void Update() {
        _cooldownTimer += Time.deltaTime;

        if (PlayerInSight() && _cooldownTimer >= _attackCooldown) {
            _cooldownTimer = 0;
            _anim.SetTrigger("attackMelee");
        }
    }

    private bool PlayerInSight() {
        RaycastHit2D hit = Physics2D.BoxCast(
            _boxColl.bounds.center + transform.right * _visionRange * transform.localScale.x * _colliderDistance,
            new Vector3(
                _boxColl.bounds.size.x * _visionRange,
                _boxColl.bounds.size.y,
                _boxColl.bounds.size.z
                ),
            0, Vector2.left, 0, LAYER_PLAYER);

        // if the boxcast collides with something on the player layer,
        // the player is in sight.
        return hit.collider != null;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_boxColl.bounds.center + transform.right * _visionRange * transform.localScale.x * _colliderDistance,
             new Vector3(
                _boxColl.bounds.size.x * _visionRange,
                _boxColl.bounds.size.y,
                _boxColl.bounds.size.z
                ));
    }
}
