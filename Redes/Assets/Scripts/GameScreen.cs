using UnityEngine;
using Fusion;
using System;

public class GameScreen : MonoBehaviour
{
    [SerializeField] GameObject _waitMenu;
    [SerializeField] GameObject _countdownMenu;
    [SerializeField] GameObject _raceMenu;

    public void OnEnable()
    {
        GameManager.OnGameStateChanged += ChangeState;
}
    public void OnDisable()
    {
        GameManager.OnGameStateChanged -= ChangeState;
    }

    private void ChangeState(EGameState state)
    {
        switch (state)
        {
            case EGameState.WaitingPlayers:
                _waitMenu.SetActive(true);
                _countdownMenu.SetActive(false);
                _raceMenu.SetActive(false);
                break;
            case EGameState.Countdown:
                _waitMenu.SetActive(false);
                _countdownMenu.SetActive(true);
                _raceMenu.SetActive(false);
                break;
            case EGameState.Racing:
                _waitMenu.SetActive(false);
                _raceMenu.SetActive(true);
                break;
        }
    }
}

