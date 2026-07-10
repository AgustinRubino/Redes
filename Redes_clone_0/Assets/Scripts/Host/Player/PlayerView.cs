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
        //[Networked, OnChangedRender(nameof(SetCarModel))] public int CarModelIndex { get; set; }
        [SerializeField] CarModels _models;
        [SerializeField] GameObject _carModel;
        [SerializeField] MeshRenderer _renderer;
        //[Networked, OnChangedRender(nameof(SetCarColor))] public Color CarColor { get; set; }
        [Space(5)]
        [SerializeField] public Camera PlayCam;
        [field: Space(5)]
        //[Networked, OnChangedRender(nameof(SetTagName))] public string PlayerName { get; set; }
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
    }
}