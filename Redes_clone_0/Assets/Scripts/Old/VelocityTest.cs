using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VelocityTest : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 direction;
    public float velocity;

    [SerializeField] float _maxSpeed = 10;
    [Space(5)]
    [SerializeField] float _accelerationForce;
    [SerializeField] float _breakForce;
    [SerializeField] float _deaccelerationForce;
    [Space(5)]
    [SerializeField] Vector2 _steerAngles = new(60, 15);
    [SerializeField] float _currentAngle = 0;

    [Header("REFS")]
    [SerializeField] float _currentSpeed;

    [Space(10), Header("View")]
    [SerializeField] LineRenderer _line;
    [SerializeField] Transform _view;

    (float h, float v) input;

    Vector3 lookVector;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        input.h = Input.GetAxis("Horizontal");
        input.v = Input.GetAxis("Vertical");


        //_line.SetPosition(0, transform.position + direction * _currentAcceleration);
        //_line.SetPosition(1, transform.position);
        //_line.SetPosition(2, transform.position + direction.VectorRight() * _currentAcceleration * input.h) ;

        if (direction != Vector3.zero)
        {
            _view.rotation = Quaternion.Euler(0, Mathf.Atan2(rb.linearVelocity.x, rb.linearVelocity.z) * Mathf.Rad2Deg ,0);
        }
    }

    private void FixedUpdate()
    {
        //direction = rb.linearVelocity.With(y: 0).normalized;
        //velocity = rb.linearVelocity.magnitude;

        //if (direction == Vector3.zero && input.v != 0)
        //{
        //    direction = transform.forward * input.v;
        //}

        //var result = velocity >= maxSpeed ? Vector3.zero : direction * maxSpeed * input.v;
        ////_currentAcceleration = Mathf.Lerp(maxSpeed, 0, velocity / maxSpeed);
        //rb.AddForce(result, ForceMode.Acceleration);
        //rb.AddForce(new Vector3(direction.z,0,-direction.x)* maxSpeed * input.h, ForceMode.Acceleration);
        SetVeolcity();
    }

    private void SetVeolcity()
    {
        _currentSpeed = rb.linearVelocity.magnitude;

        var facingForward = Vector3.Dot(transform.forward, rb.linearVelocity);


        var speedFactor = Mathf.InverseLerp(0, _maxSpeed, _currentSpeed);

        Vector3 velocity;
        if (facingForward >= 0)
        {
            velocity = input.v switch
            {
                > 0 => Accelerate(),
                < 0 => this.Break(),
                _ => Deaccelerate()
            };
        }
        else
        {
            velocity = input.v switch
            {
                > 0 => this.Break(),
                < 0 => Accelerate(),
                _ => Deaccelerate()
            };
        }
        if (velocity == Vector3.zero) return;
        _currentAngle = RotationAngle(speedFactor);
        velocity = velocity.RotateY(_currentAngle);

        rb.AddForce(velocity, ForceMode.Acceleration);
    }

    private Vector3 Accelerate()
    {
        return transform.forward * _accelerationForce * input.v;
    }

    private Vector3 Break()
    {
        return -transform.forward * _breakForce * input.v;
    }

    private Vector3 Deaccelerate()
    {
        if (_currentSpeed < 0.05f) return Vector3.zero;
        return -rb.linearVelocity.normalized * _deaccelerationForce;
    }

    private float RotationAngle(float speedFactor)
    {
        var target = Mathf.Lerp(_steerAngles.x, _steerAngles.y, speedFactor) * input.h * Mathf.Sign(input.v);

        if (Mathf.Abs(target - _currentAngle) < 0.01f) return target;
        return Mathf.Lerp(_currentAngle, target, 0.01f);
    }
}
