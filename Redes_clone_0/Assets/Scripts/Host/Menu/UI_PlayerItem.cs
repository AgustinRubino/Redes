using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerItem : NetworkBehaviour
{
    [SerializeField] TMP_Text _nameTxt;
    [SerializeField] TMP_Text _carTxt;
    [SerializeField] TMP_Text _readyTxt;
    [SerializeField] Image _colorIMG;

    [Networked, OnChangedRender(nameof(NameChanged))] public string Name { get; set; }
    [Networked, OnChangedRender(nameof(CarChanged))] public string Car { get; set; }
    [Networked, OnChangedRender(nameof(ColorChanged))] public Color Color { get; set; }
    [Networked, OnChangedRender(nameof(ReadyChanged))] public bool Ready { get; set; }


    void NameChanged() {
        Object.name = Name;
        _nameTxt.text = Name;
    }
    
    void CarChanged() {
        _carTxt.text = Car;
    }
    void ColorChanged() {
        _colorIMG.color = Color;
    }
    void ReadyChanged() {
        _readyTxt.text = Ready ? "raedy" : "not ready";
        _readyTxt.color = Ready ? Color.green : Color.red;
    }
}
