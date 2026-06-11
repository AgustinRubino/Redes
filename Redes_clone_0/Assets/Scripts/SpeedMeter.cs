using TMPro;
using UnityEngine;

public class SpeedMeter : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    ForceMovement _movement;
    private void OnEnable()
    {
        _movement = PlayerManager.Instance.localPlayerModel.movement;
    }

    private void FixedUpdate()
    {
        _text.text = _movement.Speed.ToString();
    }
}
