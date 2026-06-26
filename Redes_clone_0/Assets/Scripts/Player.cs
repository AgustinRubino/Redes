using Fusion;
using Fusion.Addons.Physics;
using System;
using UnityEngine;

namespace Redes
{
    [RequireComponent(typeof(NetworkRigidbody3D))]
    public class Player : NetworkBehaviour
    {
        [Space(10), Header("References")]
        [SerializeField] PlayerConfig _config;
        [SerializeField] PlayerView _view;
        [Space(10)]
        [SerializeField] private PlayerController _controller;
        public PlayerController Controller => _controller;
        [Networked, OnChangedRender(nameof(OnPause))] bool Paused { get; set; }

        private void OnPause()
        {
            _view.Activate(Paused);
            if (Paused)
            {
                RB.linearVelocity = Vector3.zero;
            }
        }

        [SerializeField] private bool _isGhost = false;
        public bool IsGhost { 
            get => _isGhost;
            set
            {
                if (value)
                {
                    ReferenceManager.GhostCam.gameObject.SetActive(true);
                    _view.PlayCam.enabled = false;
                }
                else
                {
                    _view.PlayCam.enabled = true;
                    ReferenceManager.GhostCam.gameObject.SetActive(false);
                }
            }
        }

        InputHandler _inputs;
        [SerializeField] NetworkRigidbody3D _rb;
        public Rigidbody RB => _rb.Rigidbody;

        #region GameManager
        private void OnGameStateChanged(EGameState state)
        {
            switch (state)
            {
                case EGameState.Racing:
                    if (IsGhost) break;
                    _view.PlayCam.enabled = true;
                    if (HasStateAuthority) Paused = false;
                    break;
                case EGameState.Countdown:
                    if (!IsGhost) 
                     _view.PlayCam.enabled = true;
                    break;
                case EGameState.WaitingPlayers:
                    _view.PlayCam.enabled = false;
                    break;
                case EGameState.Finishing:
                    if (HasStateAuthority) Paused = true;
                    break;
            }
        }

        private void OnGameManagerSpawned(GameManager manager)
        {
            GameManager.OnGameManagerSpawned -= OnGameManagerSpawned;
            if (!HasInputAuthority) return;

            GameManager.OnGameStateChanged += OnGameStateChanged;

            OnGameStateChanged(manager.GameState);
        }
        #endregion

        #region MonoBehaviour Methods
        private void Start()
        {
            _rb = GetComponent<NetworkRigidbody3D>();
            _view = _view == null ? GetComponentInChildren<PlayerView>() : _view;

            if (GameManager.HasSpawned)
            {
                GameManager.OnGameStateChanged += OnGameStateChanged;
            }
            else if (HasInputAuthority)
                GameManager.OnGameManagerSpawned += OnGameManagerSpawned;
        }
        private void Update()
        {
            if (_inputs == null) return;
            if (!HasStateAuthority || Paused) return;

            _inputs.UpdateInputs();
        }
        #endregion
        #region Network Methods
        public override void Spawned()
        {
            if (!HasStateAuthority) return;
            //if (RB == null) _rb = GetComponent<NetworkRigidbody3D>();

            _inputs = new();
            _controller.Set(this, _inputs);
            _view.SetView(_config);

            //if (GameManager.HasSpawned)
            //{
            //    OnGameStateChanged(ReferenceManager.GameManager.GameState);
            //}

            Debug.Log($"player {Object.name} spawned!");
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority || Paused) return;

            Controller.UpdateController();
        }
        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            if (!HasStateAuthority) return;

            ReferenceManager.Player = null;
        }
        #endregion
        public void SetPlayerConfig(PlayerConfig config)
        {
            _config = config;
            Object.name = _config.name;
            Debug.Log($"Player's {config.name} config setted");
            _view.SetView(_config);
        }

    }
}