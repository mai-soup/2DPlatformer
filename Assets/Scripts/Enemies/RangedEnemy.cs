using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour {
    [Header("Attack")]
    [SerializeField] private float _attackCooldown;
    [SerializeField] private int _damage;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject[] _fireballs;
    private Health _playerHealth;

    [Header("Collider")]
    [SerializeField] private BoxCollider2D _boxColl;
    [SerializeField] private LayerMask LAYER_PLAYER;

    [Header("Vision")]
    [SerializeField] private float _visionRange;
    [SerializeField] private float _colliderDistance;
    private float _cooldownTimer = Mathf.Infinity;

    [Header("Sound")]
    [SerializeField] private AudioClip _fireballSound;

    // refs
    private Animator _anim;
    private EnemyPatrol _enemyPatrol;

    private void Awake() {
        _anim = GetComponent<Animator>();
        _enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update() {
        _cooldownTimer += Time.deltaTime;

        if (PlayerInSight() && _cooldownTimer >= _attackCooldown
            && _playerHealth.currentHealth != 0) {
            _cooldownTimer = 0;
            _anim.SetTrigger("attackRanged");
        }

        // if this enemy has a patrol, enable it only when player not in sight.
        if (_enemyPatrol != null) {
            _enemyPatrol.enabled = !PlayerInSight();
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

        //if the boxcast collides with something on the player layer,
        //the player is in sight.
        if (hit.collider != null) {
            _playerHealth = hit.transform.GetComponent<Health>();
        }

        return hit.collider != null;
    }

    private void RangedAttack() {
        _cooldownTimer = 0;
        SoundManager.instance.PlaySound(_fireballSound);

        // shoot a projectile
        int f = FindFireball();
        _fireballs[f].transform.position = _firePoint.position;
        _fireballs[f].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    private int FindFireball() {
        for (int i = 0; i < _fireballs.Length; i++) {
            if (!_fireballs[i].activeInHierarchy) {
                return i;
            }
        }
        return 0;
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
