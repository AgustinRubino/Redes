using UnityEngine;

public class CarControl2 : MonoBehaviour

{
    public float forwardSpeed = 25f;
    public float backwardSpeed = 10f;
    public float turnSpeed = 150f;
    public float brakeForce = 30f;
    public float acceleration = 10f;
    public float deceleration = 5f;

    float currentSpeed;
    float currentTurnAngle;
    Rigidbody rb;

    float verticalInput;
    float horizontalInput;
    bool isBraking;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("CarController : Rigidbody not found for car");
            enabled = false;
        }
        //  rb.freezeRotation = true;
    }

    private void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        isBraking = Input.GetKey(KeyCode.Space);
    }

    private void FixedUpdate()
    {
        // FixedUpdate It is more suitable for applying physical forces.

        HandleMovement();
        HandleSteering();
        HandleBraking();



    }

    void HandleMovement()
    {
        if (verticalInput > 0)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, forwardSpeed, Time.fixedDeltaTime * acceleration);
        }
        else if (verticalInput < 0)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, -backwardSpeed, Time.fixedDeltaTime * acceleration);
        }
        else
        {
            //If no button is pressed, the speed will gradually decrease.
            currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.fixedDeltaTime * deceleration);
        }

        rb.AddForce(transform.forward * currentSpeed, ForceMode.Acceleration);

        //*Speed ​​limit
        /* float maxCurrentAllowedSpeed = (verticalInput > 0) ? forwardSpeed : backwardSpeed;
         rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxCurrentAllowedSpeed);*/
    }

    void HandleSteering()
    {
        //If the car doesn't move, the steering wheel shouldn't work either.
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            currentTurnAngle = horizontalInput * turnSpeed;
        }
        else
        {
            currentTurnAngle = 0;
        }

        //Rotation of around Y
        Quaternion turnRotation = Quaternion.Euler(0f, currentTurnAngle * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    void HandleBraking()
    {
        if (isBraking && rb.linearVelocity.magnitude > 0.1f)
        {
            rb.AddForce(-rb.linearVelocity.normalized * brakeForce, ForceMode.Acceleration);
            currentSpeed = 0;
        }
        else if (isBraking && rb.linearVelocity.magnitude <= 0.1f)
        {
            rb.linearVelocity = Vector3.zero;
            currentSpeed = 0;
        }
    }
}