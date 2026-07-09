using Host;
using UnityEngine;

public class UI_LobbyMenu : MonoBehaviour
{
    [SerializeField] MenuRunnerHandler _runnerHandler;
    [Space(10)]
    [SerializeField] GameObject _userDataMenu;
    [SerializeField] GameObject _findLobbyMenu;
    [SerializeField] GameObject _createLobbyMenu;
    [SerializeField] GameObject _joinningLobbyMenu;

    private void Awake()
    {
        CloseAll();
    }

    private void Start()
    {
        if (PlayerInfo.Data.name is null or "")
        {
            _userDataMenu.SetActive(true);
            _userDataMenu.GetComponent<UI_SetPlayerData>().ShowNewUserLabel();
        }
        _findLobbyMenu.SetActive(true);
    }

    private void CloseAll()
    {
        _userDataMenu.SetActive(false);
        _findLobbyMenu.SetActive(false);
        _createLobbyMenu.SetActive(false);
        _joinningLobbyMenu.SetActive(false);
    }


    public void OpenData() {
        CloseAll();
        _userDataMenu.SetActive(true);
    }
    public void OpenLobby() {
        CloseAll();
        _findLobbyMenu.SetActive(true);
    }
    public void OpenCreate() { 
        CloseAll();
        _createLobbyMenu.SetActive(true);
    }
    public void OpenJoin() {
        CloseAll();
        _joinningLobbyMenu.SetActive(true);
    }

}
