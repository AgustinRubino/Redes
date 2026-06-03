using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class VelocityLine : MonoBehaviour
{
    LineRenderer _line;
    Rigidbody _rb;
    void Start()
    {
        _line = GetComponent<LineRenderer>();
        _rb = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _line.SetPosition(0, transform.position);
        _line.SetPosition(1, transform.position + _rb.linearVelocity.With(y:0));
    }
}
