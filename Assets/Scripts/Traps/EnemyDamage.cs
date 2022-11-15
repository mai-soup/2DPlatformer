using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour {
    // vars
    [SerializeField] protected float _damage;

    protected void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            collision.GetComponent<Health>().TakeDamage(_damage);
        }
    }
}
