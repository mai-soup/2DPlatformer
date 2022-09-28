using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    // consts
    private static readonly float ATTACK_COOLDOWN_MAX = 0.4f;

    // refs
    private Animator        _anim;
    private PlayerMovement  _playerMovement;
    [SerializeField]
    private GameObject[]    _fireballs;

    // vars
    private float       _attackCooldown = Mathf.Infinity;
    [SerializeField]
    private Transform   _firePoint;

    private void Awake() {
        _anim = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update() {
        // attack on left click
        if (Input.GetMouseButton(0) && _attackCooldown > ATTACK_COOLDOWN_MAX
            && _playerMovement.canAttack()) {
            Attack();
        }

        _attackCooldown += Time.deltaTime;
    }

    private void Attack() {
        _anim.SetTrigger("Attack");
        _attackCooldown = 0;

        GameObject fireball = GetFireballFromPool();
        fireball.transform.position = _firePoint.position;
        fireball.GetComponent<Projectile>().SetDirection(
                                        Mathf.Sign(transform.localScale.x));
    }

    private GameObject GetFireballFromPool() {
        foreach (GameObject fireball in _fireballs) {
            if (!fireball.activeInHierarchy) {
                return fireball;
            }
        }
        return _fireballs[0];
    }
}
