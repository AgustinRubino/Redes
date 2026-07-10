using Fusion;
using System;

namespace Host
{
    public class Countdown : NetworkBehaviour
    {
        public event Action OnTimerFinished;
        [Networked, OnChangedRender(nameof(UpdateTime))] public int TimeRemaining { get; set; }

        private void UpdateTime()
        {
            GameManager.OnCounter?.Invoke(TimeRemaining);
        }

        float seconds = 7;
        TickTimer _timer;
        public override void Spawned()
        {
            _timer = TickTimer.CreateFromSeconds(Runner, seconds);
            TimeRemaining = (int)seconds;
        }
        public override void FixedUpdateNetwork()
        {
            if (_timer.Expired(Runner))
            {
                // Resetear el timer para que no siga disparando
                _timer = TickTimer.None;
                TimeRemaining = 0;
                OnTimerFinished?.Invoke();
            }
            else
            {
                // Mostrar tiempo restante
                float? remaining = _timer.RemainingTime(Runner);
                if (remaining.HasValue)
                {
                    if (remaining.Value < TimeRemaining - 1)
                    {
                        TimeRemaining--;
                    }
                }
            }
        }
    }
}