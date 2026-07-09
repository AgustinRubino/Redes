using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SetPlayerData : MonoBehaviour
{
    [SerializeField] GameObject _newUserLabel;
    [SerializeField] TMP_Text _current;
    [SerializeField] TMP_InputField _input;
    [SerializeField] Button _backButton;

    private void OnEnable()
    {
        _backButton.onClick.AddListener(Back);
        _current.text = $"current name: {PlayerInfo.Data.name}";
    }

    public void ShowNewUserLabel()
    {
        if (_newUserLabel == null) return;
        _newUserLabel.SetActive(true);
    }

    private void OnDisable()
    {
        _backButton.onClick.RemoveListener(Back);

        if (_newUserLabel == null) return;
        _newUserLabel.SetActive(false);
    }

    private void Back()
    {
        var name = _input.text.Replace(' ', '_');
        if (name.Length >= 16) name.Remove(16);
        PlayerInfo.Data.name = name;

        GetComponentInParent<UI_LobbyMenu>().OpenLobby();
    }
}
