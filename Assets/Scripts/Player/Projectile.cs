using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    // const
    private static readonly float MAX_LIFETIME = 5.0f;

    // refs
    private BoxCollider2D   _boxCollider;
    private Animator        _anim;

    // vars
    [SerializeField]
    private float   _speed;
    private bool    _hasHit;
    private float   _direction;
    private float   _lifetime;

    private void Awake() {
        _boxCollider = GetComponent<BoxCollider2D>();
        _anim = GetComponent<Animator>();
    }

    private void Update() {
        if (_hasHit) return;

        // take into account time and direction
        float movementAmount = _speed * Time.deltaTime * _direction;
        // move fireball on x axis by movementspeed
        transform.Translate(movementAmount, 0, 0);

        _lifetime += Time.deltaTime;

        if (_lifetime > MAX_LIFETIME) {
            Deactivate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        _hasHit = true;
        _boxCollider.enabled = false;
        _anim.SetTrigger("Explode");

        if (collision.tag == "Enemy" && collision.GetComponent<Health>() != null) {
            collision.GetComponent<Health>().TakeDamage(1);
        }
    }

    // decide if fireball goes left or right
    public void SetDirection(float dir) {
        _lifetime = 0;
        _direction = dir;

        // reset fireball
        gameObject.SetActive(true);
        _hasHit = false;
        _boxCollider.enabled = true;

        // make sure the projectile is facing the direction it's fired in
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != Mathf.Sign(_direction)) {
            localScaleX = -localScaleX;
            transform.localScale = new Vector3(localScaleX,
                                            transform.localScale.y,
                                            transform.localScale.z);
        }
    }

    private void Deactivate() {
        gameObject.SetActive(false);
    }
}
