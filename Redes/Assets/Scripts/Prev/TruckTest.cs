using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TruckTest : MonoBehaviour
{
    [SerializeField] float _force;

    private void OnTriggerEnter(Collider other)
    {
        var rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * _force * rb.mass, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.rigidbody.gameObject.TryGetComponent(out ForceMovement m)){
            m.GetHit(collision.contacts[0].point ,GetComponent<Rigidbody>().linearVelocity);
        }
    }
}
