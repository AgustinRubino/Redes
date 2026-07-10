using System;
using UnityEngine;

public class UI_ColorMenu : MonoBehaviour
{
    [SerializeField] UI_ColorButton _coolrButtonPrefab;
    [SerializeField] int _rows = 5;
    [SerializeField] int _columns = 10;
    [SerializeField] Transform _grid;
    UI_ColorButton[] _buttons;

    UI_ColorButton _currentButton;
    Action<Color> _callback;

    private void OnEnable()
    {
        if (_buttons != null) return;

        _buttons = new UI_ColorButton[_rows * _columns];
        float multX = 1f / _columns;
        float multY = 1f / _rows;

        for (int y = 0; y < _rows; y++)
        {
            for (int x = 0; x < _columns; x++)
            {
                float h = Mathf.Lerp(0, 1, x * multX);
                float s = Mathf.Min(1, Mathf.Lerp(0.5f, 1.4f, y * multY));
                float v = Mathf.Min(1, Mathf.Lerp(1.4f, 0.5f, y * multY));
                var button = Instantiate(_coolrButtonPrefab, _grid);
                button.SetColor(Color.HSVToRGB(h, s, v));
                button.OnColorPicked += Picked;
                _buttons[y * _columns + x] = button;
            }
        }
    }

    private void Picked(UI_ColorButton button)
    {
        _currentButton?.Deselect();
        _currentButton = button;
    }

    public void Activate(Color color, Action<Color> callback)
    {
        _callback = callback;
        foreach (var button in _buttons)
        {
            if (button.GetColor() == color)
            {
                _currentButton?.Deselect();
                _currentButton = button;
                return;
            }
        }
        _currentButton = _buttons[0];
    }

    void Back()
    {
        _callback?.Invoke(_currentButton.GetColor());
        _callback = null;
    }
}
