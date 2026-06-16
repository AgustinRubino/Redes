using Fusion;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Redes
{
    public class PlayerView : NetworkBehaviour
    {
        [Header("Car View")]
        [SerializeField] CarModels _models;
        [SerializeField] GameObject _carModel;
        [SerializeField] MeshRenderer _renderer;
        [Space(5)]
        [SerializeField] public Camera PlayCam;
        [Space(5)]
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
                
            }
            else
            {
                _playerNameTagObj.SetActive(true);
            }

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
        public void RPC_SetView(PlayerConfig config)
        {

            RPC_SetCarModel(config.carModelIndex);
            RPC_SetCarColor(config.color);
            RPC_SetTagName(config.name);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_SetTagName(string name) => _playerNameTagTxt.text = name;
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_SetCarColor(Color color) => _renderer.material.color = color;
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_SetCarModel(int index)
        {
            if (_carModel != null) Destroy(_carModel);
            _carModel = Instantiate(_models.Models[index], _player.transform);
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
            _engineAudio.volume = Mathf.Lerp(0, _maxEngineVolume, speed);
        }

        #endregion
        private void Jumped() => RPC_OnJump();

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_OnJump()
        {
            _onJumping.Invoke();
        }

        private void Dashed() => RPC_OnDash();

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_OnDash()
        {
            _onDashing.Invoke();
        }
    }
}