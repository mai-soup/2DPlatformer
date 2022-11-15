using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    // refs
    [SerializeField]
    private Health _playerHealth;
    [SerializeField]
    private Image  _totalHealthBar;
    [SerializeField]
    private Image  _currentHealthBar;

    private void Start() {
        // divide by 10 to get fraction, so e.g. 3 hearts -> 0.3
        _currentHealthBar.fillAmount = _playerHealth.startingHealth / 10;
    }

    private void Update() {
        // divide by 10 to get fraction, so e.g. 3 hearts -> 0.3
        _currentHealthBar.fillAmount = _playerHealth.currentHealth / 10;
    }
}
