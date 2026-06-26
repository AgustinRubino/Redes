using Fusion;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Redes
{
    public class PlayerView : NetworkBehaviour
    {
        [Header("Car View")]
        [Networked, OnChangedRender(nameof(SetCarModel))] public int CarModelIndex { get; set; }
        [SerializeField] CarModels _models;
        [SerializeField] GameObject _carModel;
        [SerializeField] MeshRenderer _renderer;
        [Networked, OnChangedRender(nameof(SetCarColor))] public Color CarColor { get; set; }
        [Space(5)]
        [SerializeField] public Camera PlayCam;
        [field: Space(5)]
        [Networked, OnChangedRender(nameof(SetTagName))] public string PlayerName { get; set;  }
        [SerializeField] GameObject _playerNameTagObj;
        [SerializeField] TMPro.TMP_Text _playerNameTagTxt;
        [Space(15)]
        [SerializeField] AudioSource _engineAudio;
        [SerializeField] float _maxEngineVolume = 1;
        [Space(10)]
        [SerializeField] UnityEvent _onMoving;
        [SerializeField] UnityEvent _onStopping;
        [SerializeField] UnityEvent _onDashing;
        [SerializeField] UnityEvent _onJumping;

        bool _isMoving = false;
        [SerializeField] Player _player;

        public override void Spawned()
        {
            if (HasStateAuthority)
            {
                _playerNameTagObj.SetActive(false);
                PlayCam.gameObject.SetActive(true);
            }
            else
            {
                _playerNameTagObj.SetActive(true);
                PlayCam.gameObject.SetActive(false);
            }
            if (_carModel == null)
            {
                SetCarModel();
                SetCarColor();
                SetTagName();
            }

            _engineAudio.Play();
            _player.Controller.OnDashed += Dashed;
            _player.Controller.OnJumped += Jumped;
        }


        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) return;

            RPC_SetEngineSound(_player.Controller.Speed / _player.Controller.maxSpeed);

            if (_player.Controller.Speed > 0.1f && !_isMoving)
            {
                _isMoving = !_isMoving;
                RPC_IsMoving(_isMoving);
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_IsMoving(bool isMoving)
        {
            if (_isMoving) _onMoving.Invoke();
            else _onStopping.Invoke();
        }


        #region Init
        public void SetView(PlayerConfig config)
        {
            CarModelIndex = config.carModelIndex;
            PlayerName = config.name;
            CarColor = config.color;

            //RPC_SetCarModel(config.carModelIndex);
            //RPC_SetCarColor(config.color);
            //RPC_SetTagName(config.name);
        }

        private void SetTagName() => _playerNameTagTxt.text = PlayerName;

        private void SetCarColor() => _renderer.material.color = CarColor;

        private void SetCarModel()
        {
            if (_carModel != null) Destroy(_carModel);
         
            _carModel = Instantiate(_models.Models[CarModelIndex], _player.transform);
            _renderer = _carModel.GetComponent<MeshRenderer>();
            if (_renderer == null)
                _renderer = _carModel.GetComponent<RendererAccesor>().renderer;
        }
        #endregion

        #region Engine
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_SetEngineSound(float speed)
        {
            if (speed < 0.05f) _engineAudio.volume = 0;
            else _engineAudio.volume = Mathf.Lerp(0, _maxEngineVolume, speed);
        }

        #endregion
        private void Jumped() => RPC_OnJump();

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_OnJump()
        {
            _onJumping.Invoke();
        }

        private void Dashed() => RPC_OnDash();

        //[Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_OnDash()
        {
            _onDashing.Invoke();
        }
        internal void Activate(bool active)
        {
            if (active)
            {
                _engineAudio.Pause();
            }
            else
            {
                _engineAudio.UnPause();
            }
        }
    }
}