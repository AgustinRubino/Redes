using UnityEngine;
using UnityEngine.UIElements;

public class WheelControlTest: MonoBehaviour
{
    [SerializeField] WheelCollider _collider;
    [SerializeField] Transform _transform;
    [Space(20)]
    [SerializeField] float _torque = 100;
    [SerializeField] float _steering = 60;

    private Vector3 position;
    private Quaternion rotation;

    private void Update()
    {
        _collider.GetWorldPose(out position, out rotation);
        _transform.rotation = rotation;

        _collider.motorTorque = _torque;
        _collider.steerAngle = _steering;
    }
}