using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    // refs
    private Animator _anim;

    // vars
    [SerializeField]
    public float _startingHealth { get; private set; } = 3;
    public float currentHealth { get; private set; }
    private bool _isDead = false;

    private void Awake() {
        currentHealth = _startingHealth;
        _anim = GetComponent<Animator>();
    }

    public void TakeDamage(float dmg) {
        if (_isDead) return;

        currentHealth = Mathf.Clamp(currentHealth - dmg,
                        0, _startingHealth);

        if (currentHealth == 0) {
            // we ded
            _anim.SetTrigger("Die");
            // disable movement when ded
            GetComponent<PlayerMovement>().enabled = false;
            _isDead = true;
            return;
        }

        _anim.SetTrigger("Hurt");
    }
}
