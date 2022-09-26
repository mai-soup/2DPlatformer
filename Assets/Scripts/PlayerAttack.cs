using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    // consts
    [SerializeField] private
        //static
        //readonly
        float ATTACK_COOLDOWN_MAX = 0.5f;

    // refs
    private Animator        _anim;
    private PlayerMovement  _playerMovement;

    // vars
    private float _attackCooldown = Mathf.Infinity;

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
    }
}
