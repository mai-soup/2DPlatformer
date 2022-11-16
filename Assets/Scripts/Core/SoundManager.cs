using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager instance { get; private set; }
    private AudioSource _source;

    private void Awake() {
        if (instance == null) {
            instance = this;
            // keep object through levels
            DontDestroyOnLoad(gameObject);
        } else if (instance != null && instance != this) {
            // we are in a duplicate soundmgr, destroy ourselves
            Destroy(gameObject);
        }

        _source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip _sound) {
        _source.PlayOneShot(_sound);
    }
}
