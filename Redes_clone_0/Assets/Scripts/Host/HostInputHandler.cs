using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

namespace Host
{
    public class HostInputHandler
    {
        bool _jumpInput;
        bool _dashInput;
        private InputData _data;

        public HostInputHandler() => _data = new InputData();

        public void UpdateInputs()
        {
            (float x, float y) axis = (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            _data.forward = axis.y > 0 ? (sbyte)1 : axis.y < 0 ? (sbyte)-1 : (sbyte)0;
            _data.right = axis.x > 0 ? (sbyte)1 : axis.x < 0 ? (sbyte)-1 : (sbyte)0;

            if (!_jumpInput)
            {
                _jumpInput = Input.GetKeyDown(KeyCode.Space);
            }
            if (!_dashInput)
            {
                _dashInput = Input.GetKeyDown(KeyCode.LeftShift);
            }
        }

        public InputData GetData() => _data;
    }
}
