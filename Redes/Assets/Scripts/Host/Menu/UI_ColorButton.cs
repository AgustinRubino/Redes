using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_ColorButton : MonoBehaviour
{
    public event Action<UI_ColorButton> OnColorPicked;

    [SerializeField] Button _btn;
    [SerializeField] Image _colorIMG;
    [SerializeField] Image _buttonIMG;

    private void Awake()
    {
        _btn.onClick.AddListener(Select);
    }

    private void Select()
    {
        _buttonIMG.color = Color.white;
        OnColorPicked?.Invoke(this);
    }

    public void SetColor(Color color)
    {
        _colorIMG.color = color;
    }
    public void Deselect()
    {
        _buttonIMG.color = Color.black;
    }

    public Color GetColor()
    {
        return _colorIMG.color;
    }
}
