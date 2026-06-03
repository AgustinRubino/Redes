using UnityEngine;
using System;

public class ForceMovement : MonoBehaviour
{
    [SerializeField] Rigidbody _body;
    [SerializeField] Transform _view;

    [Header("Acceleration")]
    [SerializeField] float _maxSpeed;

    [SerializeField] float _acceleration = 2;
    [SerializeField] float _deacceleration = 4;
    [SerializeField] float _breakForce = 4;

    [Header("Steering")]
    [SerializeField] float _steerRotation = 0.1f;
    [SerializeField] Vector2 _steeringForce;
    [SerializeField] LayerMask _gronud;

    [Header("Result Values")]
    [SerializeField] float _speed;
    [SerializeField] float _steering;
    [SerializeField] float _currentRotation;
    [SerializeField] int _moveDirection;
    [SerializeField] Vector3 _targetVelocity;

    [field: SerializeField] public Vector3 Direction { get; private set; }

    bool _isHitted;

    float _inputV;
    float _inputH;

    public bool IsMovingForward => _moveDirection > 0;

    private void Update()
    {
        _inputV = Input.GetAxis("Vertical");
        _inputH = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        if (!IsGrounded) return;
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
    }


    private void SetVariables()
    {
        _speed = _body.linearVelocity.magnitude;

        _moveDirection = MathF.Sign(Vector3.Dot(transform.forward, _body.linearVelocity));
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
        _body.MoveRotation(_body.rotation * rot);


        var result = transform.forward * _speed * _moveDirection;
        result = result - _body.linearVelocity;

        _body.AddForce(result * 1 / Time.fixedDeltaTime, ForceMode.Acceleration);


    }

    public bool IsGrounded => Physics.Raycast(transform.position + Vector3.up * 0.1f, -transform.up, 0.2f, _gronud);

    private void Breaking()
    {
        if (_speed < 0.05f) return;
        _body.AddForce(-transform.forward * _breakForce * -_inputV, ForceMode.Acceleration);
    }

    private void Accelerate()
    {
        if (_speed > _maxSpeed) return;
        _body.AddForce(transform.forward *  _acceleration * _inputV * _body.mass);

        //Debug.Log("Accelerate");
        //speed = speed + Mathf.Sign(_inputV) * _acceleration * Time.fixedDeltaTime;
        //speed = Mathf.Clamp(speed, -_maxSpeed, _maxSpeed);
    }
    private void Deaccelerate()
    {
        if (_speed < 0.05f)
        {
            _body.linearVelocity = Vector3.zero;
            return;
        }
        _body.AddForce(-_body.linearVelocity.normalized  * _deacceleration, ForceMode.Acceleration);
    }


    public void GetHit(Vector3 position, Vector3 force)
    {
        _body.AddForceAtPosition(force, position, ForceMode.Impulse);
        _isHitted = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + _body.linearVelocity);
    }
}