using Fusion;
using Redes;
using UnityEngine;
using UnityEngine.Events;

namespace Host
{
    public class PlayerView : NetworkBehaviour
    {
        [Header("PLAYER VIEW")]

        #region Model Variables
        [Header("NETWORKED FIELDS")]
        [Networked, OnChangedRender(nameof(CarModelRender))] public int CarModelIndex { get; set; }
        [Networked, OnChangedRender(nameof(CarColorRender))] public Color CarColor { get; set; }
        [Networked, OnChangedRender(nameof(CarNameRender))] public string PlayerName { get; set; }
        [Space(10)]
        [SerializeField] CarModels _models;
        [SerializeField] GameObject _carModel;
        [SerializeField] MeshRenderer _renderer;
        [Space(5)]
        [SerializeField] public Camera PlayCam;
        [field: Space(5)]
        [SerializeField] GameObject _playerNameTagObj;
        [SerializeField] TMPro.TMP_Text _playerNameTagTxt;
        #endregion
        [Space(15)]
        [SerializeField] AudioSource _engineAudio;
        [SerializeField] float _maxEngineVolume = 1;
        [Space(10)]
        [SerializeField] UnityEvent _onMoving;
        [SerializeField] UnityEvent _onStopping;
        [SerializeField] UnityEvent _onDashing;
        [SerializeField] UnityEvent _onJumping;
        [Space(10)]
        [SerializeField] Player _player;


        public override void Spawned()
        {
            _playerNameTagObj.SetActive(!HasInputAuthority);
            PlayCam.gameObject.SetActive(HasInputAuthority);

            _player = GetComponentInParent<Player>();

            _engineAudio.Play();
            //_player.Controller.OnDashed += Dashed;
            //_player.Controller.OnJumped += Jumped;
        }

        #region Init
        void CarModelRender()
        {
            if (_carModel != null) Destroy(_carModel);
            _carModel = Instantiate(_models.Models[CarModelIndex], transform);
        }
        void CarColorRender()
        {
            _renderer = _carModel.GetComponent<MeshRenderer>();
            if (_renderer == null)
                _renderer = _carModel.GetComponent<RendererAccesor>().renderer;

            _renderer.material.color = CarColor;
        }
        void CarNameRender()
        {
            _playerNameTagTxt.text = PlayerName;
            Object.name = PlayerName;
        }
        #endregion
    }
}