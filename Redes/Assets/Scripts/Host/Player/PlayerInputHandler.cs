using Host;
using UnityEngine;

namespace Host
{
    public class PlayerInputHandler
    {
        bool _dashInput;
        bool _jumpInput;

        public int Vertical { get; private set; }
        public int Horizontal { get; private set; }
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


        public void GetInputData(InputData data)
        {
            Vertical = data.forward;
            Horizontal = data.right;

            _dashInput |= data.dash;
            _jumpInput |= data.jump;
        }
    }
}