using System;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Rigidbody _body;

    [Header("Acceleration")]
    [SerializeField] float _speed;
    [SerializeField] float _maxSpeed;

    [SerializeField] float _acceleration = 2;
    [SerializeField] float _deacceleration = 4;
    [SerializeField] float _breakForce = 4;

    [Header("Steering")]
    [SerializeField] float _maxSteering;
    [SerializeField] Vector2 _steeringForce;
    [SerializeField] float _steering;

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
        else if ( _speed >= 0.001f) Deaccelerate(ref _speed);
        else _speed = 0;

        _body.linearVelocity = transform.forward * _speed;
        Rotate();
    }


    private void Rotate()
    {
        //var speedFactor = Mathf.InverseLerp(0, _maxSpeed, _speed);
        //var currentSteer = Mathf.Lerp(_steeringForce.x, _steeringForce.y, speedFactor);
        var rot = Quaternion.Euler(0,_inputH * _maxSteering * Time.fixedDeltaTime,0);
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
        

        speed = speed - Mathf.Sign(_inputV) * _deacceleration * Time.fixedDeltaTime;
        speed = Mathf.Sign(_inputV) * Mathf.Max(Mathf.Abs(speed), 0);
    }
}
