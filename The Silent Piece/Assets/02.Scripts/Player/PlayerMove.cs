using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private PlayerInput  _input;
    private InputAction  _moveAction;
    private InputAction  _jumpAction;
    private Rigidbody2D  _rigid;
    private Vector2      _moveDir;
    private float        _moveSpeed = 10.0f;
    private float        _jumpPower = 12f;

    private void Awake()
    {
        _input = this.GetComponentSafe<PlayerInput>();
        _rigid = this.GetComponentSafe<Rigidbody2D>();

        InitializeMoveAction();
        InitializeJumpAction();
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
        _rigid.MovePosition(_rigid.position + _moveDir * Time.fixedDeltaTime * _moveSpeed);
    }

    // TODO: jump is not worked; may be gravity scale problem??
    private void Jump()
    {
        Debug.Log("Jump");
        _rigid.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
    }
}
