using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private static readonly float WALLJUMP_COOLDOWN_MAX = 0.5f;

    // refs
    private Rigidbody2D     _body;
    private Animator        _anim;
    private BoxCollider2D   _boxCollider;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _wallLayer;

    // vars
    [SerializeField] private float _speed;
    [SerializeField] private float _walljumpCooldown;
    [SerializeField] private float _jumpPower;
    [SerializeField] private float _walljumpXForce;
    [SerializeField] private float _walljumpYForce;
                     private float _horizontalInput;

    private void Awake() {
        // set up refs
        _body = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update() {
        _horizontalInput = Input.GetAxis("Horizontal");

        // turn player around when changing movement direction
        if (_horizontalInput > 0.01f) {
            transform.localScale = Vector3.one;
        } else if (_horizontalInput < -0.01f) {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // set animator params
        _anim.SetBool("isRunning", (_horizontalInput != 0));
        _anim.SetBool("isGrounded", isGrounded());

        // walljump logic
        if (_walljumpCooldown > WALLJUMP_COOLDOWN_MAX) {
            // adjust body velocity only horizontally according to input
            _body.velocity = new Vector2(
                _horizontalInput * _speed, _body.velocity.y);

            if (isOnWall() && !isGrounded()) {
                // if touching wall already, dont move towards it
                _body.velocity = new Vector2(0, _body.velocity.y);
            }

            // jump when hitting space
            if (Input.GetKeyDown(KeyCode.Space)) {
                Jump();
            }
        } else {
            _walljumpCooldown += Time.deltaTime;
        }
    }

    private void Jump() {
        if (isGrounded()) {
            // if jumping from ground, do normal jump
            _body.velocity = new Vector2(_body.velocity.x, _jumpPower);
            _anim.SetTrigger("Jump");
        } else if (isOnWall() && !isGrounded()) {
            // if doing walljump
            // reset cooldown
            _walljumpCooldown = 0;

            if (_horizontalInput == 0) {
                // make bigger force away from wall BUT NOT up
                _body.velocity = new Vector2(
                    -Mathf.Sign(transform.localScale.x) * _walljumpXForce * 2,
                    0);
                // flip player sprite
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x),
                    transform.localScale.y, transform.localScale.z);
            } else {
                    // make force away from wall and up
                    _body.velocity = new Vector2(
                        -Mathf.Sign(transform.localScale.x) * _walljumpXForce,
                        _walljumpYForce);
            }
        }
    }

    private bool isGrounded() {
        RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center,
            _boxCollider.size, 0, Vector2.down, 0.1f, _groundLayer);
        return raycastHit.collider != null;
    }

    private bool isOnWall() {
        RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center,
            _boxCollider.size, 0, new Vector2(transform.localScale.x, 0), 0.1f,
            _wallLayer);
        return raycastHit.collider != null;
    }
}
