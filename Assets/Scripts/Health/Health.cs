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
    [Header ("iFrames")]
    [SerializeField]
    private float _iFramesDuration;
    [SerializeField]
    private int _numFlashes;


    private void Awake() {
        currentHealth = startingHealth;
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float dmg) {
        if (_isDead) return;

        currentHealth = Mathf.Clamp(currentHealth - dmg,
                        0, startingHealth);

        if (currentHealth == 0) {
            // ded
            _anim.SetTrigger("Die");

            // disable player movement when we ded
            if (GetComponent<PlayerMovement>() != null) {
                GetComponent<PlayerMovement>().enabled = false;
            }

            // disable enemy and patrol when enemy ded
            if (GetComponentInParent<EnemyPatrol>() != null) {
                GetComponentInParent<EnemyPatrol>().enabled = false;
            }
            if (GetComponent<MeleeEnemy>() != null) {
                GetComponent<MeleeEnemy>().enabled = false;
            }

            _isDead = true;
            return;
        }

        _anim.SetTrigger("Hurt");
        Invincibility();
    }

    public void RestoreHealth(float healValue) {
        if (_isDead) return;

        currentHealth = Mathf.Clamp(currentHealth + healValue,
                        0, startingHealth);
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
