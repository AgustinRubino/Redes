using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    public static event Action<PlayerRef> OnPlayerJoinedEvent;
    public static event Action<PlayerRef> OnPlayerLeftEvent;
    public static event Action<PlayerRef> OnPlayerWinEvent;
    public static event Action<EGameState> OnGameStateChanged;

    public static GameManager Instance { get; private set; }

    [Networked, OnChangedRender(nameof(OnGameStateChangeRender))]
    public EGameState GameState { get; private set; }

    [Header("Player Preferences")]
    [SerializeField] int _minPlayers = 2;
    [SerializeField] int _maxPlayers = 6;
    [SerializeField] int _startCounterTime = 10;
    [SerializeField] int _maxPlayersBeforeEndGame = 3;
    [Space(20), Header("References")]
    [SerializeField] public FlagManager flagManager;
    [SerializeField] public NetworkStartCounter startCounter;

    List<PlayerRef> _winners = new();

    public override void Spawned()
    {
        SingletonManager.Instance.Runner = Runner;

        _winners = new();
        flagManager.OnPlayerCompleteTrack += OnPlayerWin;
        startCounter.OnFinishCounter += CounterFinished;

        OnGameStateChanged?.Invoke(GameState);
    }

    private void CounterFinished()
    {
        if (!HasStateAuthority) return;
        if (GameState == EGameState.Countdown)
        {
            GameState = EGameState.Racing;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_OnPlayerWin(PlayerRef player)
    {
        if (_winners.Contains(player)) return;
        _winners.Add(player);

        if (_winners.Count < _maxPlayersBeforeEndGame) return;

        GameState = EGameState.Finishing;
    }

    private void OnPlayerWin(PlayerRef player)
    {
        RPC_OnPlayerWin(player);
    }


    public void PlayerJoined(PlayerRef player)
    {
        if (!HasStateAuthority) return;
        CheckForPlayersCount(Runner);
        OnPlayerJoinedEvent?.Invoke(player);
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (!HasStateAuthority) return;
        if (Instance == this && player == Runner.LocalPlayer)
            Instance = null;

        CheckForPlayersCount(Runner);
        OnPlayerLeftEvent?.Invoke(player);
    }
    private void CheckForPlayersCount(NetworkRunner runner)
    {
        if (GameState == EGameState.Finishing)
        {
            return;
        }
        int count = runner.ActivePlayers.Count();

        if (GameState == EGameState.Countdown && count < _minPlayers)
        {
            GameState = EGameState.WaitingPlayers;
        }
        else if (GameState == EGameState.WaitingPlayers && count >= _minPlayers)
        {
            GameState = EGameState.Countdown;
        }
    }

    private void OnGameStateChangeRender()
    {
        Debug.Log($"Player{Runner.LocalPlayer} change to {GameState}");
        OnGameStateChanged?.Invoke(GameState);

        if (GameState == EGameState.Countdown)
            startCounter.StartCounter(_startCounterTime);
    }
    private void Awake()
    {
        Instance = this;
    }
}
