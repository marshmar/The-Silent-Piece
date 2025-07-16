using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private PlayerInput  _input;
    private InputAction  _moveAction;
    private InputAction  _jumpAction;
    private Rigidbody2D  _rigid;
    private Vector2      _moveDir;
    private float        _moveSpeed = 5.0f;
    private float        _jumpPower = 12.0f;
    private bool         _canJump = true;
    private void Awake()
    {
        _input = this.GetComponentSafe<PlayerInput>();
        _rigid = this.GetComponentSafe<Rigidbody2D>();

        InitializeMoveAction();
        InitializeJumpAction();
    }

    private void Update()
    {
        if(_rigid.linearVelocityY < 0)
            checkGround();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void InitializeMoveAction()
    {
        _moveAction = _input.actions.FindAction("Move");

        if (_moveAction.IsNull<InputAction>()) return;

        _moveAction.performed += context =>
        {
            SetMoveDir(context);
        };

        _moveAction.canceled += context =>
        {
            _moveDir = Vector2.zero;
        };
    }

    private void InitializeJumpAction()
    {
        _jumpAction = _input.actions.FindAction("Jump");
        if (_jumpAction.IsNull<InputAction>()) return;

        _jumpAction.performed += context =>
        {
            Jump();
        };
    }

    private void SetMoveDir(InputAction.CallbackContext context)
    {
        _moveDir = context.ReadValue<Vector2>();
    }

    private void Move()
    {
        _rigid.linearVelocity = new Vector2(_moveDir.x * _moveSpeed, _rigid.linearVelocity.y);
    }

    private void Jump()
    {
        if(_canJump)
        {
            _rigid.AddForceY(_jumpPower, ForceMode2D.Impulse);
            _canJump = false;
        }

    }

    private void checkGround()
    {
        float rayDist = 0.6f;
        if(!_canJump && Physics2D.Raycast(_rigid.position, Vector2.down, rayDist, LayerMask.GetMask("MAP")))
        {
            _canJump = true;
        }
    }
}
