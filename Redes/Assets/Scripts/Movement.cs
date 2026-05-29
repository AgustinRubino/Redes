using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Rigidbody _body;

    [Header("Acceleration")]
    [SerializeField] float _maxSpeed;

    [SerializeField] float _acceleration = 2;
    [SerializeField] float _deacceleration = 4;

    [Header("Steering")]
    [SerializeField] float _maxSteering;

    [field: SerializeField] public Vector3 Direction { get; private set; }


    private void Update()
    {
        var inputH = Input.GetAxis("Horizontal");

        var forwardSpeed = Vector3.Dot(transform.forward, _body.linearVelocity);



        _body.AddForce(transform.forward * Input.GetAxis("Vertical") * _maxSpeed);
        Debug.Log(_body.linearVelocity);
        ///
        Quaternion rot = transform.rotation * Quaternion.Euler(Vector3.up * inputH * _maxSpeed * Time.deltaTime);
        _body.MoveRotation(rot);

    }

    private void GetCurrentSpeed(ref float speed)
    {
        var inputV = Input.GetAxis("Vertical");

        bool isAccelerating = Mathf.Sign(speed) == Mathf.Sign(inputV);
        
        

    }
}
