using Fusion;
using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] public Rigidbody rb;

    [Header("Acceleration")]
    [SerializeField] float _maxSpeed;

    [SerializeField] float _acceleration = 2;
    [SerializeField] float _deacceleration = 4;
    [SerializeField] float _breakForce = 4;

    [Header("Steering")]
    [SerializeField] float _steerRotation = 0.1f;
    [SerializeField] Vector2 _steeringForce;
    [SerializeField] LayerMask _ground;
    [field: Space(10)]
    [field: Header("Result Values")]
    [field: SerializeField] public float Speed { get; private set; }
    [SerializeField] float _steering;
    [SerializeField] float _currentRotation;
    [field: SerializeField] public int MoveDirection { get; private set;  }
    [SerializeField] Vector3 _targetVelocity;


    private float _inputH;
    private float _inputV;

    private void Update()
    {
        _inputV = Input.GetAxis("Vertical");
        _inputH = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        SetVariables();
        HandleSteering();
        HandleAcceleration();
        //_view.forward = _body.linearVelocity;
    }


    private void SetVariables()
    {
        Speed = rb.linearVelocity.magnitude;
        MoveDirection = MathF.Sign(Vector3.Dot(transform.forward, rb.linearVelocity));
    }
    private void HandleAcceleration()
    {
        bool isAccelerating = MoveDirection == MathF.Sign(_inputV);

        if (_inputV != 0)
        {
            if (isAccelerating || Speed < 0.05f) Accelerate();
            else Breaking();
        }
        else if (Speed > 0) Deaccelerate();
    }

    private void HandleSteering()
    {
        var speed = Mathf.Abs(Speed) / _maxSpeed;
        var targetRotation = MoveDirection * Mathf.Lerp(_steeringForce.x * speed, _steeringForce.y, speed);
        _currentRotation = Mathf.Lerp(_currentRotation, _inputH * targetRotation, _steerRotation);
        _currentRotation = Mathf.Abs(_currentRotation) < 0.05f ? 0 : _currentRotation;

        rb.AddForce(new Vector3(rb.linearVelocity.z,0,-rb.linearVelocity.x)  * _currentRotation, ForceMode.Acceleration);

        var a = _currentRotation * Time.fixedDeltaTime;
        var rot = Quaternion.Euler(0, a, 0);
        rb.MoveRotation(rb.rotation * rot);


        //var result = transform.forward * Speed * MoveDirection;
        //result = result - rb.linearVelocity;

        //rb.AddForce(result * 1 / Time.fixedDeltaTime, ForceMode.Acceleration);


    }

    //public bool IsGrounded => Physics.Raycast(transform.position + Vector3.up * 0.1f, -transform.up, 0.2f, _gronud);

    private void Breaking()
    {
        if (Speed < 0.05f) return;
        rb.AddForce(-transform.forward * _breakForce * -_inputV, ForceMode.Acceleration);
    }

    private void Accelerate()
    {
        if (Speed > _maxSpeed) return;
        rb.AddForce(transform.forward * _acceleration * _inputV, ForceMode.Acceleration);

        //Debug.Log("Accelerate");
        //speed = speed + Mathf.Sign(_inputV) * _acceleration * Time.fixedDeltaTime;
        //speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);
    }
    private void Deaccelerate()
    {
        if (Speed < 0.05f)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }
        if (Speed < 1f)
        {
            var result = Mathf.Lerp(0, _deacceleration, Speed);
            rb.AddForce(-rb.linearVelocity.normalized * result, ForceMode.Acceleration);
        }
        else rb.AddForce(-rb.linearVelocity.normalized * _deacceleration, ForceMode.Acceleration);
    }


    public void GetHit(Vector3 position, Vector3 force)
    {
        rb.AddForceAtPosition(force, position, ForceMode.Impulse);
        //_isHitted = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + rb.linearVelocity);
    }
}