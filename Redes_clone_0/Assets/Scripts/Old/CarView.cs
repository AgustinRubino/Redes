using UnityEngine;

public class CarView : MonoBehaviour
{
    [SerializeField] CarController _carRB;
    [SerializeField] Vector3 _modelDirection; // rb linear velocity

    private void Start()
    {
        _modelDirection = transform.parent.forward;
    }
    private void Update()
    {
        if (_carRB.Speed > 1)
        {
            _modelDirection = _carRB.rb.linearVelocity.With(y: 0).normalized * _carRB.MoveDirection;
        }
        transform.forward = _modelDirection;
    }
}