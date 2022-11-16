using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour {
    [SerializeField] private float _attackCooldown;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject[] _arrows;
    private float _cooldownTimer;

    [Header("Sounds")]
    [SerializeField] private AudioClip _shootSound;

    private void Attack() {
        _cooldownTimer = 0;

        SoundManager.instance.PlaySound(_shootSound);
        int arr = FindArrow();
        _arrows[arr].transform.position = _firePoint.position;
        _arrows[arr].GetComponent<EnemyProjectile>().
            ActivateProjectile();
    }

    private int FindArrow() {
        for (int i = 0; i < _arrows.Length; i++) {
            if (!_arrows[i].activeInHierarchy) {
                return i;
            }
        }

        return 0;
    }

    private void Update() {
        _cooldownTimer += Time.deltaTime;

        if (_cooldownTimer >= _attackCooldown) {
            Attack();
        }
    }
}
