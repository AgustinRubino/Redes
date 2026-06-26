using Fusion;
using Redes;
using System;
using UnityEngine;
using UnityEngine.Events;

public class Flag : MonoBehaviour
{
    public event Action OnFlagPassed;

    [SerializeField] Collider _collider;
    [field: SerializeField] public bool IsActive { get; private set;  }

    [Header("Events")]
    [SerializeField] UnityEvent _onFlagActivated;
    [SerializeField] UnityEvent _onFlagPassed;

    private void Awake()
    {
        if (_collider == null)
            _collider = GetComponent<Collider>();
    }

    public void Activate()
    {
        Debug.Log("Activated");
        IsActive = true;
        _collider.enabled = true;
        _onFlagActivated.Invoke();
    }
    public void Deactivate()
    {
        Debug.Log("Passed");
        _collider.enabled = false;
        IsActive = false;
        _onFlagPassed.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.name == ReferenceManager.Player.name)
        OnFlagPassed?.Invoke();
    }
}
