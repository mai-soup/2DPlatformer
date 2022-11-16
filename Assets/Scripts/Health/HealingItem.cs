using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItem : MonoBehaviour {
    // vars
    [SerializeField] private float _hpValue;

    [Header("Sound")]
    [SerializeField] private AudioClip _pickupSound;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            collision.GetComponent<Health>().RestoreHealth(_hpValue);
            SoundManager.instance.PlaySound(_pickupSound);
            gameObject.SetActive(false);
        }
    }
}
