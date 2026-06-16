using UnityEngine;

namespace Redes
{
    public class InputHandler
    {
        bool _jumpInput;
        bool _dashInput;

        public float Vertical { get; private set; }
        public float Horizontal { get; private set;  }
        public bool JumpInput
        {
            get
            {
                if (_jumpInput)
                {
                    _jumpInput = false;
                    return true;
                }
                return false;
            }
        }
        public bool DashInput
        {
            get
            {
                if (_dashInput)
                {
                    _dashInput = false;
                    return true;
                }
                return false;
            }
        }

        public void UpdateInputs()
        {
            Vertical = Input.GetAxis("Vertical");
            Horizontal = Input.GetAxis("Horizontal");

            if (!_jumpInput)
            {
                _jumpInput = Input.GetKeyDown(KeyCode.Space);
            }
            if (!_dashInput)
            {
                _dashInput = Input.GetKeyDown(KeyCode.LeftShift);
            }
        }
    }
}