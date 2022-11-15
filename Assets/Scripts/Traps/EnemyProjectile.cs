using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : EnemyDamage {
    [SerializeField] private float _speed;
    [SerializeField] private float _resetTime;
    private float _lifetime;
    private Animator _anim;
    private BoxCollider2D _coll;
    private bool _hasHit;

    private void Awake() {
        _anim = GetComponent<Animator>();
        _coll = GetComponent<BoxCollider2D>();
    }

    public void ActivateProjectile() {
        _hasHit = false;
        _lifetime = 0;
        gameObject.SetActive(true);
        _coll.enabled = true;
    }

    private void Update() {
        if (_hasHit)
            return;
        
        float movementAmount = _speed * Time.deltaTime;
        transform.Translate(movementAmount, 0, 0);

        _lifetime += Time.deltaTime;
        if (_lifetime > _resetTime) {
            gameObject.SetActive(false);
        }
    }

    private new void OnTriggerEnter2D(Collider2D collision) {
        _hasHit = true;
        _coll.enabled = false;
        base.OnTriggerEnter2D(collision);

        if (_anim != null) {
            _anim.SetTrigger("Explode"); // explode fireball
        } else {
            gameObject.SetActive(false); // deactivate arrow
        }
    }

    private void Deactivate() {
        gameObject.SetActive(false);
    }
}
