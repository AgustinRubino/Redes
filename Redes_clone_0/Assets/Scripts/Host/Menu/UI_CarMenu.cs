using Redes;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UI_CarMenu : MonoBehaviour
{
    [SerializeField] CarModels _models;
    [SerializeField] Transform _grid;
    [SerializeField] Button _backBTN;

    [Space(10), SerializeField] UI_CarButton _carButtonPrefab;
    Action<int> _callback;

    UI_CarButton _current;
    UI_CarButton[] _buttons;
    private void Awake()
    {
        _backBTN.onClick.AddListener(Back);
    }
    private void OnEnable()
    {
        if (_buttons != null) return;

        _buttons = new UI_CarButton[_models.Models.Count];
        for (int i = 0; i < _models.Models.Count; i++)
        {
            var button = Instantiate(_carButtonPrefab, _grid);
            button.SetIndex(i);
            _buttons[i] = button;
        }
    }

    public void Activate(int carIndex, Action<int> callback)
    {
        _callback = callback;
        for (int i = 0; i < _buttons.Length; i++)
        {
            if (i == carIndex)
            {
                _current?.Deselect();
                _current = _buttons[i];
                return;
            }
        }
        _current = _buttons[0];
    }

    void Back()
    {
        _callback?.Invoke(_current.GetIndex());
        _callback = null;
    }
}
