using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FireTrap : MonoBehaviour {
    [Header("Firetrap Timers")]
    [SerializeField]
    private float _activationDelay;
    [SerializeField]
    private float _activeTime;
    [SerializeField] private float _damage;

    [Header("Sound")]
    [SerializeField] private AudioClip _fireSound;

    private Health _playerHealth;

    private bool _isTriggered;
    private bool _isActive;

    // refs
    private Animator        _anim;
    private SpriteRenderer  _spriteRenderer;

    private void Awake() {
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if (_playerHealth != null && _isActive) {
            _playerHealth.TakeDamage(_damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            _playerHealth = collision.GetComponent<Health>();
            if (!_isTriggered && _playerHealth.currentHealth > 0) {
                ActivateFiretrap();
            }

            if (_isActive) {
                // if active - hurt player
                collision.GetComponent<Health>().TakeDamage(1);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player") {
            _playerHealth = null;
        }
    }

    private async void ActivateFiretrap() {
        _isTriggered = true;
        _spriteRenderer.color = Color.red;

        // wait for the activation delay before activating
        await Task.Delay(Mathf.RoundToInt(1000 * _activationDelay));
        SoundManager.instance.PlaySound(_fireSound);
        _isActive = true;
        _anim.SetBool("isActive", true);

        // let be active for duration, then deactivate
        await Task.Delay(Mathf.RoundToInt(1000 * _activeTime));
        _isActive = false;
        _isTriggered = false;
        _spriteRenderer.color = Color.white;
        _anim.SetBool("isActive", false);
    }
}
