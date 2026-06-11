using UnityEngine;
using Fusion;
using System;
using Fusion.Addons.Physics;

public class ForceMovement : NetworkBehaviour
{
    [SerializeField] NetworkRigidbody3D _body;

    [Header("Acceleration")]
    [SerializeField] float _maxSpeed;

    [SerializeField] float _acceleration = 2;
    [SerializeField] float _deacceleration = 4;
    [SerializeField] float _breakForce = 4;

    [Header("Steering")]
    [SerializeField] float _steerRotation = 0.1f;
    [SerializeField] Vector2 _steeringForce;
    [SerializeField] LayerMask _gronud;

    [Header("Jump")]
    [SerializeField] float _jumpForce = 100;
    [SerializeField] LayerMask _groundLayer = 1 << 8;

    [Header("Dash")]
    [SerializeField] float _dashForce = 100f;
    [SerializeField] float _dashCooldown;


    [Header("Result Values")]
    [SerializeField] float _speed;
    [SerializeField] float _steering;
    [SerializeField] float _currentRotation;
    [SerializeField] Vector3 _currentVelocity;
    [SerializeField] int _moveDirection;
    [SerializeField] Vector3 _targetVelocity;
    [SerializeField] float _dashTimer;

    public float Speed => _speed;

    [field: SerializeField] public Vector3 Direction { get; private set; }

    bool _isHitted;

    float _inputV;
    float _inputH;
    bool _jumpInput;
    bool _dashInput;

    public bool IsMovingForward => _moveDirection > 0;

    private void Update()
    {
        _inputV = Input.GetAxis("Vertical");
        _inputH = Input.GetAxis("Horizontal");
        if (!_jumpInput)
        {
            _jumpInput = Input.GetKeyDown(KeyCode.Space);
        }
        if (!_dashInput)
        {
            _dashInput = Input.GetKeyDown(KeyCode.LeftShift);
        }

    }

    public override void FixedUpdateNetwork()
    {
        SetVariables();
        if (!_isHitted)
        {
            HandleSteering();
            HandleAcceleration();
        }
        else
        {
            if (_speed < 0.4f)
                _isHitted = false;
        }
        //_view.forward = _body.linearVelocity;

        HandleJump();

        HandleDash();
    }

    private void HandleJump()
    {
        if (_jumpInput)
        {
            _jumpInput = false;
            if (!IsGrounded()) return;
            _body.Rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }

    private void HandleDash()
    {
        if (_dashTimer < _dashCooldown)
        {
            _dashTimer += Runner.DeltaTime;
            _dashInput = false;
            return;
        }

        if (_dashInput)
        {
            _dashInput = false;
            _dashTimer = 0;
            _body.Rigidbody.AddForce(_currentVelocity.normalized * _dashForce, ForceMode.Impulse);
        }
    }

    private void SetVariables()
    {
        _speed = _body.Rigidbody.linearVelocity.magnitude;

        _currentVelocity = _body.Rigidbody.linearVelocity.With(y: 0);
        _moveDirection = MathF.Sign(Vector3.Dot(transform.forward, _currentVelocity));
    }
    private void HandleAcceleration()
    {
        bool isAccelerating = _moveDirection == MathF.Sign(_inputV);

        if (_inputV != 0)
        {
            if (isAccelerating || _speed  < 0.05f) Accelerate();
            else Breaking();
        }
        else if (_speed > 0) Deaccelerate();
    }

    private void HandleSteering()
    {
        var speed = Mathf.Abs(_speed) / _maxSpeed;
        var targetRotation = _moveDirection * Mathf.Lerp(_steeringForce.x * speed, _steeringForce.y, speed);
        _currentRotation = Mathf.Lerp(_currentRotation, _inputH * targetRotation, _steerRotation);
        _currentRotation = Mathf.Abs(_currentRotation) < 0.05f ? 0 : _currentRotation;

        var a = _currentRotation * Time.fixedDeltaTime;
        var rot = Quaternion.Euler(0, a, 0);
        _body.Rigidbody.MoveRotation(_body.Rigidbody.rotation * rot);


        var result = transform.forward * _speed * _moveDirection;
        result = result - _currentVelocity;

        _body.Rigidbody.AddForce(result * 1 / Time.fixedDeltaTime, ForceMode.Acceleration);


    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, 0.2f, _groundLayer);
    }

    private void Breaking()
    {
        if (_speed < 0.05f) return;
        _body.Rigidbody.AddForce(-transform.forward * _breakForce * -_inputV, ForceMode.Acceleration);
    }

    private void Accelerate()
    {
        if (_speed > _maxSpeed) return;
        _body.Rigidbody.AddForce(transform.forward *  _acceleration * _inputV * _body.Rigidbody.mass);

        //Debug.Log("Accelerate");
        //speed = speed + Mathf.Sign(_inputV) * _acceleration * Time.fixedDeltaTime;
        //speed = Mathf.Clamp(speed, -_maxSpeed, _maxSpeed);
    }
    private void Deaccelerate()
    {
        if (_speed < 0.05f)
        {
            _body.Rigidbody.linearVelocity = Vector3.zero;
            return;
        }
        _body.Rigidbody.AddForce(-_currentVelocity.normalized * _deacceleration, ForceMode.Acceleration);
    }


    public void GetHit(Vector3 position, Vector3 force)
    {
        _body.Rigidbody.AddForceAtPosition(force, position, ForceMode.Impulse);
        _isHitted = true;
    }
}