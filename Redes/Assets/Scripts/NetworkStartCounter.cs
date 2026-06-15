using Fusion;
using System;
using UnityEngine;

public class NetworkStartCounter : NetworkBehaviour
{
    public event Action OnStartCounter;
    public event Action<int> OnCounterChange;
    public event Action OnFinishCounter;


    [Networked, SerializeField] public bool IsActive { get; private set; }
    [Networked] public TickTimer Timer { get; private set; }

    [Networked, OnChangedRender(nameof(ChangeTimeLeft))]
    [SerializeField] int TimeLeft { get; set; }

    private void ChangeTimeLeft()
    {
        OnCounterChange?.Invoke(TimeLeft);
    }

    public void StartCounter(int value)
    {
        IsActive = true;
        Timer = TickTimer.CreateFromSeconds(Runner, value);
        TimeLeft = value;

        OnStartCounter?.Invoke();
    }

    public override void FixedUpdateNetwork()
    {
        if (!IsActive) return;

        if (Timer.Expired(Runner))
        {
            Timer = TickTimer.None;
            IsActive = false;
            RPC_CounterFinished();
        }

        if (Timer.RemainingTime(Runner) < TimeLeft)
        {
            TimeLeft -= 1;
        }
    }

    private void RPC_CounterFinished()
    {
        OnFinishCounter?.Invoke();
    }
}
