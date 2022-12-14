using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Health : MonoBehaviour {
    // consts
    public static readonly int LAYER_PLAYER = 8;
    public static readonly int LAYER_ENEMY = 9;
    // refs
    private Animator        _anim;
    private SpriteRenderer  _spriteRenderer;

    // vars
    public float startingHealth;
    public float currentHealth { get; private set; }
    private bool _isDead = false;
    private bool _isInvincible = false;

    [Header ("iFrames")]
    [SerializeField]
    private float _iFramesDuration;
    [SerializeField]
    private int _numFlashes;

    [Header("Components")]
    [SerializeField] private Behaviour[] _components;

    [Header("Sounds")]
    [SerializeField] private AudioClip _deathSound;
    [SerializeField] private AudioClip _hurtSound;


    private void Awake() {
        currentHealth = startingHealth;
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float dmg) {
        if (_isDead || _isInvincible) return;

        currentHealth = Mathf.Clamp(currentHealth - dmg,
                        0, startingHealth);

        if (currentHealth == 0) {
            // ded
            foreach (Behaviour component in _components)
                component.enabled = false;

            _anim.SetBool("isGrounded", true);
            _anim.SetTrigger("Die");

            _isDead = true;
            SoundManager.instance.PlaySound(_deathSound);
            return;
        }

        _anim.SetTrigger("Hurt");
        Invincibility();
        SoundManager.instance.PlaySound(_hurtSound);
    }

    public void RestoreHealth(float healValue) {
        if (_isDead) return;

        currentHealth = Mathf.Clamp(currentHealth + healValue,
                        0, startingHealth);
    }

    public void Respawn() {
        _isDead = false;
        RestoreHealth(startingHealth);
        _anim.ResetTrigger("Die");
        _anim.Play("player_idle");
        Invincibility();

        foreach (Behaviour component in _components)
            component.enabled = true;
    }

    private async void Invincibility() {
        _isInvincible = true;
        Physics2D.IgnoreLayerCollision(LAYER_PLAYER, LAYER_ENEMY, true);
        // wait for duration of invincibility, then turn collisions back on
        for (int i = 0; i < _numFlashes; i++) {
            // flash player semitransparent red
            _spriteRenderer.color = new Color(1, 0, 0, 0.5f);
            // wait
            await Task.Delay(Mathf.RoundToInt(1000 * _iFramesDuration / _numFlashes / 2));
            // change colour back to normal
            _spriteRenderer.color = Color.white;
            // wait
            await Task.Delay(Mathf.RoundToInt(1000 * _iFramesDuration / _numFlashes / 2));
        }
        Physics2D.IgnoreLayerCollision(LAYER_PLAYER, LAYER_ENEMY, false);
        _isInvincible = false;
    }

    private void Deactivate() {
        gameObject.SetActive(false);
    }
}
