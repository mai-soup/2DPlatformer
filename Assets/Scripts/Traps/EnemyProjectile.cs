using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : EnemyDamage {
    [SerializeField] private float _speed;
    [SerializeField] private float _resetTime;
    private float _lifetime;

    public void ActivateProjectile() {
        _lifetime = 0;
        gameObject.SetActive(true);
    }

    private void Update() {
        float movementAmount = _speed * Time.deltaTime;
        transform.Translate(movementAmount, 0, 0);

        _lifetime += Time.deltaTime;
        if (_lifetime > _resetTime) {
            gameObject.SetActive(false);
        }
    }

    private new void OnTriggerEnter2D(Collider2D collision) {
        base.OnTriggerEnter2D(collision);
        gameObject.SetActive(false);
    }
}
