using UnityEngine;

public class TorqueTest : MonoBehaviour
{
    [SerializeField] float _torque;
    [SerializeField] float _speed;
    Rigidbody rb;
    float input;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }
    private void Update()
    {
        input = Input.GetAxis("Vertical");
    }
    private void FixedUpdate()
    {
        rb.AddTorque(transform.up * _torque * input);

        Debug.Log(rb.linearVelocity);
        //rb.AddForce(transform.forward * _speed * input, ForceMode.Acceleration);
    }
}