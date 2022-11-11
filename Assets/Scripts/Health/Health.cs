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
    public float _startingHealth { get; private set; } = 3;
    public float currentHealth { get; private set; }
    private bool _isDead = false;
    [Header ("iFrames")]
    [SerializeField]
    private float _iFramesDuration;
    [SerializeField]
    private int _numFlashes;


    private void Awake() {
        currentHealth = _startingHealth;
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
        Invincibility();
    }

    public void RestoreHealth(float healValue) {
        if (_isDead) return;

        currentHealth = Mathf.Clamp(currentHealth + healValue,
                        0, _startingHealth);
    }

    private async void Invincibility() {
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
    }
}
