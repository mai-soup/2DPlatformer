using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireballHolder : MonoBehaviour {
    [SerializeField] private Transform _enemy;

    private void Update() {
        transform.localScale = _enemy.localScale;
    }
}
