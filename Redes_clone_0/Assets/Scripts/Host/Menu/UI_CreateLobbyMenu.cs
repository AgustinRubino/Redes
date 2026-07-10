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
    [SerializeField] TMP_Text _warningTxt;

    const string LENGTH_EXCEPTION = "The size of the lobby's name has to be above 6 letters";
    const string NULL_EXCEPTION = "the name cannot be empty";
    private void Awake()
    {
        _warningTxt.text = "";
        _hostBTN.onClick.AddListener(Host);
        _backBTN.onClick.AddListener(() => GetComponentInParent<UI_LobbyMenu>().OpenLobby());
    }

    private void OnEnable()
    {
        _hostBTN.interactable = true;
    }

    private void Host()
    {
        if (string.IsNullOrEmpty(_input.text))
        {
            _warningTxt.text = $"<color=red>{NULL_EXCEPTION}";
            return;
        }
        if (_input.text.Length <= 6)
        {
            _warningTxt.text = $"<color=red>{LENGTH_EXCEPTION}";
            return;
        }
        _warningTxt.text = "";

        _hostBTN.interactable = false;
        GetComponentInParent<UI_LobbyMenu>().LoadingScreen();
        GetComponentInParent<MenuRunnerHandler>().CreateGame(_input.text, SceneIndex.Game);
    }
}
