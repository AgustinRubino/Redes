using Fusion;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Host
{
    public class GameManager : NetworkBehaviour
    {
        public static Action<int> OnCounter;

        [Header("Wait")]
        [SerializeField] Canvas _startCanvas;
        [SerializeField] GameObject _startCamera;
        [SerializeField] UI_ReadyScreen _waitMenuPrefab;
        UI_ReadyScreen _waitMenu;
        [Space(10), Header("COUNTDOWN")]
        [SerializeField] Countdown _countdownPrefab;
        Countdown _countdown;
        [SerializeField] GameObject _playerCanvas;
        [Space(10), Header("RACE")]
        [SerializeField] FlagManager _flagManager;
        [Space(10)]
        [SerializeField] GameObject _winScreen;
        [SerializeField] GameObject _loseScreen;
        public static GameManager Local { get; private set; }

        #region MonoBehaviour
        private void Awake()
        {
            if (Local == null)
                Local = this;
        }
        #endregion

        public override void Spawned()
        {
            if (Runner.IsServer)
            {
                _waitMenu = Runner.Spawn(_waitMenuPrefab);
                _waitMenu.GetComponent<NetworkTransform>().transform.parent = _startCanvas.transform;
                _waitMenu.transform.localPosition = Vector3.zero;
                _waitMenu.transform.localRotation = Quaternion.identity;
                _waitMenu.transform.localScale = Vector3.one;
                _waitMenu.OnPlayersReady += OnPlayersReady;
            }
        }

        private void OnPlayersReady()
        {
            PlayerManager.Instance.RPC_SpawnAll();
            _startCamera.SetActive(false);
            _playerCanvas.SetActive(true);
            _countdown = Runner.Spawn(_countdownPrefab);
            _countdown.OnTimerFinished += StartRace;
        }

        private void StartRace()
        {
            Runner.Despawn(_countdown.Object);

            RPC_StartRace();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_StartRace()
        {
            Player.Local.CanMove(true);
            _flagManager.OnPlayerCompleteTrack += FinishRace;
        }

        private void FinishRace(PlayerRef player)
        {
            RPC_FinishRace(player);
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_FinishRace(PlayerRef player)
        {
            PlayerManager.Instance.ForEach((p, d) => {
                d.PlayerObj.CanMove(false);
                if (p == player)
                {
                    RPC_Win(p);
                }
                else
                { 
                    RPC_Lose(p); 
                }
            }
            );
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_Win([RpcTarget]  PlayerRef player)
        {
            _winScreen.SetActive(true);
        }
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_Lose([RpcTarget]  PlayerRef player)
        {
            _loseScreen.SetActive(true);
        }
    }
}