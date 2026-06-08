using System;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Rigidbody _body;

    [Header("Acceleration")]
    [SerializeField] float _maxSpeed;

    [SerializeField] float _acceleration = 2;
    [SerializeField] float _deacceleration = 4;
    [SerializeField] float _breakForce = 4;

    [Header("Steering")]
    [SerializeField] float _steerRotation = 0.1f;
    [SerializeField] Vector2 _steeringForce;

    [Header("Result Values")]
    [SerializeField] float _speed;
    [SerializeField] float _steering;
    [SerializeField] float _currentRotation;


    [field: SerializeField] public Vector3 Direction { get; private set; }

    float _inputV;
    float _inputH;


    private void Update()
    {
        _inputV = Input.GetAxis("Vertical");
        _inputH = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        var forwardSpeed = Vector3.Dot(transform.forward, _body.linearVelocity);
        
        bool isAccelerating = Mathf.Sign(_speed) == Mathf.Sign(_inputV);

        if (_inputV != 0)
        {
            if (isAccelerating) Accelerate(ref _speed);
            else Breaking(ref _speed);
        }
        else if ( Mathf.Abs(_speed) >= 0.0001f) Deaccelerate(ref _speed);
        else _speed = 0;

        _body.linearVelocity = transform.forward * _speed;
        Rotate();
    }


    private void Rotate()
    {
        var speed = Mathf.Abs(_speed) / _maxSpeed;
        var targetRotation = Mathf.Sign(_speed) * Mathf.Lerp(_steeringForce.x * speed, _steeringForce.y, speed);
        _currentRotation = Mathf.Lerp(_currentRotation, _inputH * targetRotation, _steerRotation);
        _currentRotation = Mathf.Abs(_currentRotation) < 0.05f ? 0 : _currentRotation;

        var a =  _currentRotation * Time.fixedDeltaTime;
        var rot = Quaternion.Euler(0,a,0);
        _body.MoveRotation(transform.rotation * rot);
    }

    private void Breaking(ref float speed)
    {
        Debug.Log("Break");
        speed = speed + Mathf.Sign(_inputV) * _breakForce * Time.fixedDeltaTime;
        speed = Mathf.Clamp(speed, -_maxSpeed, _maxSpeed);
    }

    private void Accelerate(ref float speed)
    {

        Debug.Log("Accelerate");
        speed = speed + Mathf.Sign(_inputV) * _acceleration * Time.fixedDeltaTime;
        speed = Mathf.Clamp(speed, -_maxSpeed, _maxSpeed);
    }
    private void Deaccelerate(ref float speed)
    {
        Debug.Log("Deaccelerate");
        float sign = Mathf.Sign(speed);

        speed = speed - sign * _deacceleration * Time.fixedDeltaTime;
        speed = sign * Mathf.Max(Mathf.Abs(speed), 0);
    }
}
