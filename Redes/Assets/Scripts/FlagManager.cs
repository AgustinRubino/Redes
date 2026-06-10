using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : NetworkBehaviour
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
            OnPlayerPassedFlag?.Invoke(_currentIndex, Runner.LocalPlayer);
        }
        _currentIndex++;
        if (_currentIndex >= _flags.Length)
        {
            Debug.Log($"Player {Runner.LocalPlayer} wins!");
            OnPlayerCompleteTrack?.Invoke(Runner.LocalPlayer);
            return;
        }

        _flags[_currentIndex].Activate();
        _flags[_currentIndex].OnFlagPassed += ActivateNextFlag;
    }

    
}
