using Fusion;
using Redes;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Host
{
    public class FlagManager : MonoBehaviour
    {
        public event Action<int, PlayerRef> OnPlayerPassedFlag;
        public event Action<PlayerRef> OnPlayerCompleteTrack;

        [SerializeField] Flag[] _flags;
        [SerializeField] int _currentIndex = 0;

        public void Start()
        {
            if (_flags == null || _flags.Length == 0)
            {
                _flags = GetComponentsInChildren<Flag>();
            }
            _currentIndex = -1;
            ActivateNextFlag();
        }


        private void ActivateNextFlag()
        {
            if (_currentIndex >= 0)
            {
                _flags[_currentIndex].Deactivate();
                _flags[_currentIndex].OnFlagPassed -= ActivateNextFlag;
                //Debug.Log($"Player {Runner.LocalPlayer} passed flag {_currentIndex}!");
                PlayerPassedFlag(_currentIndex, Player.Local.Runner.LocalPlayer);
            }
            _currentIndex++;
            if (_currentIndex >= _flags.Length)
            {
                PlayerCompletedTrack(Player.Local.Runner.LocalPlayer);
                return;
            }

            _flags[_currentIndex].Activate();
            _flags[_currentIndex].OnFlagPassed += ActivateNextFlag;
        }

        private void PlayerPassedFlag(int index, PlayerRef player)
        {
            Debug.Log($"Player {player} passed flag {index}");
            OnPlayerPassedFlag?.Invoke(index, player);
        }

        private void PlayerCompletedTrack(PlayerRef player)
        {
            Debug.Log($"Player {player} wins!");
            OnPlayerCompleteTrack?.Invoke(player);
        }
    }
}
