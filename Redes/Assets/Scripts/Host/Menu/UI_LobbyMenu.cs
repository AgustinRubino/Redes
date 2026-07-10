using Host;
using UnityEngine;

public class UI_LobbyMenu : MonoBehaviour
{
    [SerializeField] MenuRunnerHandler _runnerHandler;
    [Space(10)]
    [SerializeField] GameObject _userDataMenu;
    [SerializeField] GameObject _findLobbyMenu;
    [SerializeField] GameObject _createLobbyMenu;
    [SerializeField] GameObject _loadingMenu;
    //[SerializeField] GameObject _joinningLobbyMenu;

    public MenuRunnerHandler RunnerHandler => _runnerHandler;

    private void Awake()
    {
        CloseAll();
        if (PlayerInfo.Data == null)
        {
            new GameObject("Player Info").AddComponent<PlayerInfo>();
        }
    }

    private void Start()
    {
        if (string.IsNullOrEmpty(PlayerInfo.Data.name))
        {
            _userDataMenu.SetActive(true);
            _userDataMenu.GetComponent<UI_SetPlayerData>().ShowNewUserLabel();
        }
        else _findLobbyMenu.SetActive(true);
    }

    private void CloseAll()
    {
        _userDataMenu.SetActive(false);
        _findLobbyMenu.SetActive(false);
        _createLobbyMenu.SetActive(false);
        _loadingMenu.SetActive(false);
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
    public void LoadingScreen()
    {
        CloseAll();
        _loadingMenu.SetActive(true);
    }

}
