using Fusion;
using Fusion.Addons.Physics;
using Host;
using Redes;
using UnityEngine;

namespace Host
{
    public class Player : NetworkBehaviour
    {
        [Header("HOST PLAYER")]
        [SerializeField, ReadOnly] bool _canMove = false;
        [Space(5)]
        [SerializeField] PlayerView _view;
        [field: Space(10)]
        [field: SerializeField] public PlayerController Controller { get; private set; }
        PlayerInputHandler _inputHandler;

        NetworkRigidbody3D _rb;
        public Rigidbody Body => _rb.Rigidbody;
        public PlayerView View => _view;
        public static Player Local { get; private set; }

        #region Network Behaviour Methods
        public override void Spawned()
        {
            if (Local == null && HasInputAuthority)
                Local = this;

            _rb = GetComponent<NetworkRigidbody3D>();

            _inputHandler = new();
            Controller.Set(this, _inputHandler);
        }

        public override void FixedUpdateNetwork()
        {
            if (!_canMove) return;

            if (GetInput(out InputData data))
            {
                _inputHandler.GetInputData(data);
            }

            Controller.UpdateController();
        }
        #endregion

        public void SetView(PlayerView view)
        {
            _view = view;
            _view.transform.parent = transform;
        }
        public void CanMove(bool move)
        {
            _canMove = move;
        }
    }
}