using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CarButton : MonoBehaviour
{
    [SerializeField] TMP_Text _carText;
    [SerializeField] Image _buttonImg;
    [SerializeField] Button _btn;

    [SerializeField] Color _selectedColor;
    [SerializeField] Color _notSelectedColor;

    int _index;

    private void Awake()
    {
        _btn.onClick.AddListener(Select);
    }

    private void Select()
    {
        _buttonImg.color = _selectedColor;
    }

    public void SetIndex(int index) => _index = index;
    public int GetIndex() => _index;
    public void Deselect()
    {
        _buttonImg.color = _notSelectedColor;
    }
}
