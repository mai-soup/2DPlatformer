using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    // refs
    private Rigidbody2D     _body;
    private Animator        _anim;
    private BoxCollider2D   _boxCollider;

    // vars
    [SerializeField] private float _speed;
    [SerializeField] private LayerMask _groundLayer;

    private void Awake() {
        // set up refs
        _body = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update() {
        float horizontalInput = Input.GetAxis("Horizontal");
        // adjust body velocity only horizontally according to input
        _body.velocity = new Vector2(
            horizontalInput * _speed, _body.velocity.y);

        // turn player around when changing movement direction
        if (horizontalInput > 0.01f) {
            transform.localScale = Vector3.one;
        } else if (horizontalInput < -0.01f) {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // jump when hitting space only if player is on ground
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded()) {
            Jump();
        }

        // set animator params
        _anim.SetBool("isRunning", (horizontalInput != 0));
        _anim.SetBool("isGrounded", isGrounded());
    }

    private void Jump() {
        _body.velocity = new Vector2(_body.velocity.x, _speed);
        _anim.SetTrigger("Jump");
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
        }
    }

    private bool isGrounded() {
        RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center,
            _boxCollider.size, 0, Vector2.down, 0.1f, _groundLayer);
        return raycastHit.collider != null;
    }
}
