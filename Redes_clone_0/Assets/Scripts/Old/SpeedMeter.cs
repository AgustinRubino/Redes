using Host;
using TMPro;
using UnityEngine;

public class SpeedMeter : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    PlayerController _movement;


    private void OnEnable()
    {
        _movement = Host.Player.Local.Controller;
    }

    private void FixedUpdate()
    {
        _text.text =_movement.Speed.ToString(format: "0");
    }
}
