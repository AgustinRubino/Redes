using Fusion;
using System;
using UnityEngine;

public class NetworkStartCounter : NetworkBehaviour
{
    public event Action OnStartCounter;
    public event Action<int> OnCounterChange;
    public event Action OnFinishCounter;


    [Networked, SerializeField] public bool IsActive { get; private set; }
    [SerializeField] int _time = 0;
    [SerializeField] float _timer = 0;

    public void StartCounter(int value)
    {
        IsActive = true;
        _time = value;
        _timer = value;

        OnStartCounter?.Invoke();
    }

    public override void FixedUpdateNetwork()
    {
        if (!IsActive) return;

        _timer -= Time.deltaTime;

        if ( _timer < _time)
        {
            _time = _timer.CeilToInt();
            OnCounterChange?.Invoke(_time);
        }

        if (_timer < 0)
        {
            IsActive = false;
            OnFinishCounter?.Invoke();
        }
}
}}
