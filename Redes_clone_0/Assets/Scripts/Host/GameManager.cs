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
        [Space(10)]
        [SerializeField] Countdown _countdownPrefab;
        Countdown _countdown;
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
                _waitMenu = Runner.Spawn(_waitMenu);
                _waitMenu.GetComponent<NetworkTransform>().transform.parent = _startCanvas.transform;
                _waitMenu.OnPlayersReady += OnPlayersReady;
            }
        }

        private void OnPlayersReady()
        {
            PlayerManager.Instance.RPC_SpawnAll();
            _startCamera.SetActive(false);
            _countdown = Runner.Spawn(_countdownPrefab);
            _countdown.OnTimerFinished += StartRace;
        }

        private void StartRace()
        {
            Runner.Despawn(_countdown.Object);

            PlayerManager.Instance.ForEach((p, d) => d.PlayerObj.CanMove(true));
        }

        //public override void FixedUpdateNetwork()
        //{

        //}
    }
}