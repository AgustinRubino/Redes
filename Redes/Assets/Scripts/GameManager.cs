using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Redes
{
    public class GameManager : NetworkBehaviour
    {
        public static bool HasSpawned { get; private set; } = false;
        public static Action<GameManager> OnGameManagerSpawned;
        public static Action<EGameState> OnGameStateChanged;
        public static Action<int, PlayerRef> OnPlayersWin;

        [Networked, OnChangedRender(nameof(OnGameStateRender))]
        public EGameState GameState { get; private set; } = EGameState.WaitingPlayers;
        [Space(10)]
        [SerializeField] int _minPlayersToStart = 2;
        [SerializeField] int _maxPlayers = 8;
        [SerializeField] float _startCountdown = 10;
        [SerializeField] int _maxWinners = 3;
        [SerializeField] List<PlayerRef> _winners;
        [Space(10)]
        [SerializeField] GameObject _winScreen;
        [SerializeField] GameObject _loseScreen;
        [Space(10), Header("References")]
        [SerializeField] PlayerManager _playerManager;
        [SerializeField] FlagManager _flagManager;
        [SerializeField] public NetworkStartCounter counter;

        public override void Spawned()
        {
            HasSpawned = true;
            OnGameManagerSpawned?.Invoke(this);

            _flagManager.OnPlayerCompleteTrack += PlayerCompletedTrack;

            if (!HasStateAuthority) return;
            counter.OnFinishCounter += CounterFinished;


        }

        #region Countdown
        private void CounterFinished()
        {
            RPC_RaceBegin();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.StateAuthority)]
        private void RPC_RaceBegin()
        {
            if (GameState != EGameState.Countdown) return;
            GameState = EGameState.Racing;
        }
        #endregion
        private void OnEnable()
        {
            PlayerDetector.OnPlayerJoined += CheckPlayerAmount;
            PlayerDetector.OnPlayerLeft += CheckPlayerAmount;

            ReferenceManager.GameManager = this;
        }
        private void OnDisable()
        {
            PlayerDetector.OnPlayerJoined -= CheckPlayerAmount;
            PlayerDetector.OnPlayerLeft -= CheckPlayerAmount;


            ReferenceManager.GameManager = null;
        }

        private void CheckPlayerAmount(PlayerRef player)
        {
            if (!HasStateAuthority || !HasSpawned) return;
            if (GameState == EGameState.Finishing) return;

            int count = Runner.SessionInfo.PlayerCount;

            if (count < _minPlayersToStart)
            {
                if (GameState == EGameState.Countdown)
                    GameState = EGameState.WaitingPlayers;
                return;
            }
            else if (count <= _maxPlayers)
            {
                //if (GameState == EGameState.WaitingPlayers)
                //    GameState = EGameState.Countdown;
                // Cambiar luego

                GameState = EGameState.Racing;
            }
            else
            {
                Runner.GetPlayerObject(player).GetComponent<Player>().IsGhost = true;
            }
        }


        public int GetMaxPlayers() => _maxPlayers;

        #region Finnished
        private void PlayerCompletedTrack(PlayerRef @ref)
        {
            Debug.Log("on gamemanager");
            RPC_OnPlayerCompletedTrack(@ref);
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_OnPlayerCompletedTrack(PlayerRef player)
        {
            Debug.Log("on completed track");

            if (_winners == null) _winners = new List<PlayerRef>();
            _winners.Add(player);

            if (_winners.Count < _maxWinners)
                return;

            GameState = EGameState.Finishing;

            foreach(var pRef in Runner.ActivePlayers)
            {
                if (_winners.Contains(pRef))
                {
                    RPC_Win(pRef);
                }
                else
                    RPC_Defeat(pRef);
            }

        }

        [Rpc]
        public void RPC_Win([RpcTarget] PlayerRef player)
        {
            // Show Win
            _winScreen.transform.SetParent(Camera.main.transform);
            _winScreen.GetComponent<RectTransform>().ForceUpdateRectTransforms();
            _winScreen.SetActive(true);
        }

        [Rpc]
        public void RPC_Defeat([RpcTarget] PlayerRef player)
        {
            // Show Defeat
            _loseScreen.transform.SetParent(Camera.main.transform);
            _loseScreen.GetComponent<RectTransform>().ForceUpdateRectTransforms();
            _loseScreen.SetActive(true);
            //OnPlayersWin?.Invoke(index, player);
        }

        //[Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        //public void RPC_OnFinishing(int index, PlayerRef player)
        //{
        //}

        #endregion

        private void OnGameStateRender()
        {
            Debug.Log("Game State changed to: " + GameState);
            OnGameStateChanged?.Invoke(GameState);
        }



    }
}
