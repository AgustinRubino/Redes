using System;
using UnityEngine;

namespace Host
{
    [Serializable]
    public class PlayerController
    {
        public event Action OnDashed;
        public event Action OnJumped;

        [Header("Movement")]
        [SerializeField] public float maxSpeed;
        [SerializeField] float _acceleration = 2;
        [SerializeField] float _deacceleration = 4;
        [SerializeField] float _breakForce = 4;

        [Space(10), Header("Steering")]
        [SerializeField] float _steerRotation = 0.1f;
        [SerializeField] Vector2 _steeringForce;
        [SerializeField] LayerMask _gronud;

        [Space(10), Header("Jump")]
        [SerializeField] float _jumpForce = 100;
        [SerializeField] LayerMask _groundLayer = 1 << 8;

        [Space(10), Header("Dash")]
        [SerializeField] float _dashForce = 100f;
        [SerializeField] float _dashCooldown;


        [field: Space(15), Header("Result Values")]
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float Steering { get; private set; }
        [field: SerializeField] public float CurrentRotation { get; private set; }
        [field: SerializeField] public Vector3 CurrentVelocity { get; private set; }
        [field: SerializeField] public int MoveDirection { get; private set; }
        [field: SerializeField] public Vector3 TargetVelocity { get; private set; }
        [field: SerializeField] public float DashTimer { get; private set; }

        PlayerInputHandler _input;
        Player _player;
        public void Set(Player player, PlayerInputHandler input)
        {
            _player = player;
            _input = input;
        }

        public void UpdateController()
        {
            SetVariables();
            HandleSteering();
            HandleAcceleration();
            HandleJump();
            HandleDash();
        }

        private void HandleJump()
        {
            if (_input.JumpInput)
            {
                if (!IsGrounded()) return;
                _player.Body.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
                OnJumped?.Invoke();
            }
        }

        private void HandleDash()
        {
            if (_input.DashInput)
            {
                if (DashTimer >= _dashCooldown)
                {
                    DashTimer = 0;
                    _player.Body.AddForce(CurrentVelocity.normalized * _dashForce, ForceMode.Impulse);
                    OnDashed?.Invoke();
                }
            }
            DashTimer += _player.Runner.DeltaTime;
        }

        private void SetVariables()
        {
            if (_player.transform.position.y < 0)
            {
                _player.Body.MovePosition(_player.transform.position.With(y: 0));
            }
            Speed = _player.Body.linearVelocity.magnitude;
            CurrentVelocity = _player.Body.linearVelocity.With(y: 0);
            MoveDirection = MathF.Sign(Vector3.Dot(_player.transform.forward, CurrentVelocity));
        }
        private void HandleAcceleration()
        {
            bool isAccelerating = MoveDirection == MathF.Sign(_input.Vertical);

            if (_input.Vertical != 0)
            {
                if (isAccelerating || Speed < 0.05f) Accelerate();
                else Breaking();
            }
            else if (Speed > 0) Deaccelerate();
        }

        private void HandleSteering()
        {
            var speed = Mathf.Abs(Speed) / maxSpeed;
            var targetRotation = MoveDirection * Mathf.Lerp(_steeringForce.x * speed, _steeringForce.y, speed);
            CurrentRotation = Mathf.Lerp(CurrentRotation, _input.Horizontal * targetRotation, _steerRotation);
            CurrentRotation = Mathf.Abs(CurrentRotation) < 0.05f ? 0 : CurrentRotation;

            var a = CurrentRotation * Time.fixedDeltaTime;
            var rot = Quaternion.Euler(0, a, 0);
            _player.Body.MoveRotation(_player.Body.rotation * rot);


            var result = _player.transform.forward * Speed * MoveDirection;
            result = result - CurrentVelocity;

            _player.Body.AddForce(result * 1 / Time.fixedDeltaTime, ForceMode.Acceleration);
        }

        private bool IsGrounded()
        {
            return Physics.Raycast(_player.transform.position + Vector3.up * 0.2f, Vector3.down, 0.2f, _groundLayer);
        }

        private void Breaking()
        {
            if (Speed < 0.05f) return;
            _player.Body.AddForce(-_player.transform.forward * _breakForce * -_input.Vertical, ForceMode.Acceleration);
        }

        private void Accelerate()
        {
            if (Speed > maxSpeed) return;
            _player.Body.AddForce(_player.transform.forward * _acceleration * _input.Vertical, ForceMode.Acceleration);
        }
        private void Deaccelerate()
        {
            if (Speed < 0.05f)
            {
                _player.Body.linearVelocity = Vector3.zero;
                return;
            }
            _player.Body.AddForce(-CurrentVelocity.normalized * _deacceleration, ForceMode.Acceleration);
        }

        public void GetHit(Vector3 position, Vector3 force)
        {
            _player.Body.AddForceAtPosition(force, position, ForceMode.Impulse);
        }
    }
}