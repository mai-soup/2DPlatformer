using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    // refs
    private Rigidbody2D     _body;
    private Animator        _anim;
    private BoxCollider2D   _boxCollider;
    [SerializeField]
    private LayerMask _groundLayer;

    // vars
    [Header("Movement")]
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _jumpPower;
    private float _horizontalInput;

    [Header("Coyote Time")]
    [SerializeField] private float _coyoteTime;
    private float _coyoteCountdown;

    [Header("Sound")]
    [SerializeField] private AudioClip _jumpSound;

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

        // jump when hitting space
        if (Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        }

        // adjustable jump height depending on hold length
        if (Input.GetKeyUp(KeyCode.Space) && _body.velocity.y > 0) {
            _body.velocity = new Vector2(
                _body.velocity.x,
                _body.velocity.y / 2);
        }


        _body.velocity = new Vector2(
            _horizontalInput * _speed,
            _body.velocity.y);

        if (isGrounded()) {
            // reset coyote countdown
            _coyoteCountdown = _coyoteTime;
        } else {
            _coyoteCountdown -= Time.deltaTime;
        }
    }

    private void Jump() {
        if (_coyoteCountdown <= 0)
            return;

        SoundManager.instance.PlaySound(_jumpSound);

        if (isGrounded() || _coyoteCountdown > 0)
            _body.velocity = new Vector2(_body.velocity.x, _jumpPower);

        _coyoteCountdown = 0;
    }

    private bool isGrounded() {
        RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center,
            _boxCollider.size, 0, Vector2.down, 0.1f, _groundLayer);
        return raycastHit.collider != null;
    }
    public bool canAttack() {
        // can attack when not moving horizontally, on ground, and not on wall.
        return _horizontalInput == 0 && isGrounded();
    }
}
