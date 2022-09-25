using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    // refs
    private Rigidbody2D body;

    // vars
    [SerializeField] private float speed;

    private void Awake() {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        // adjust body velocity only horizontally according to input
        body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space)) {
            body.velocity = new Vector2(body.velocity.x, speed);
        }
    }
}
