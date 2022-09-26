using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    [SerializeField]
    public float _startingHealth { get; private set; } = 3;
    public float currentHealth { get; private set; }

    private void Awake() {
        currentHealth = _startingHealth;
    }

    private void TakeDamage(float dmg) {
        currentHealth = Mathf.Clamp(currentHealth - dmg,
                        0, _startingHealth);

        if (currentHealth == 0) {
            // we ded
        }
    }
}
