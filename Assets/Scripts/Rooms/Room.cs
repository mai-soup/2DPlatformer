using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
    [SerializeField] private GameObject[] _enemies;
    private Vector3[] _initialPositions;

    private void Awake() {
        // save the initial positions
        _initialPositions = new Vector3[_enemies.Length];
        for (int i = 0; i < _enemies.Length; i++) {
            if (_enemies[i] == null)
                continue;

            _initialPositions[i] = _enemies[i].transform.position;
        }
    }

    public void ToggleRoomEnabled(bool status) {
        for (int i = 0; i < _enemies.Length; i++) {
            if (_enemies[i] == null)
                continue;

            _enemies[i].SetActive(status);
            _enemies[i].transform.position = _initialPositions[i];
        }
    }
}
