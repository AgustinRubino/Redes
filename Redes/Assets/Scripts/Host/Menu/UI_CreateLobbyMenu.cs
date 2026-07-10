using Host;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_CreateLobbyMenu : MonoBehaviour
{
    [SerializeField] TMP_InputField _input;
    [SerializeField] Button _hostBTN;
    [SerializeField] Button _backBTN;

    private void Awake()
    {
        _hostBTN.onClick.AddListener(() => GetComponentInParent<MenuRunnerHandler>().CreateGame(_input.text, SceneIndex.Game));
        _backBTN.onClick.AddListener(() => GetComponentInParent<UI_LobbyMenu>().OpenLobby());
    }
}
