using Host;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class UI_SessionMenu : MonoBehaviour
{
    [SerializeField] MenuRunnerHandler _runnerHandler;
    [SerializeField] UI_LobbyMenu _lobbyMenu;
    [SerializeField] UI_SessionList _sessionList;

    [SerializeField] Button refreshBTN;
    [SerializeField] Button _createLobbyBTN;
    [SerializeField] Button _dataBTN;
    [SerializeField] Button _backBTN;

    private void Awake()
    {
        _runnerHandler = _runnerHandler == null ? GetComponentInParent<MenuRunnerHandler>() : _runnerHandler;
        _lobbyMenu = _runnerHandler == null ? GetComponentInParent<UI_LobbyMenu>() : _lobbyMenu;

        refreshBTN.onClick.AddListener(Refresh);
        Action action = () =>
        {
            _sessionList.enabled = true;
            refreshBTN.interactable = true;
        };
        _runnerHandler.OnLobbyFound += action;
        //_runnerHandler.OnLobbyNotFound += action;

        _createLobbyBTN.onClick.AddListener(_lobbyMenu.OpenCreate);
        _dataBTN.onClick.AddListener(_lobbyMenu.OpenData);
        _backBTN.onClick.AddListener(Back);
    }

    private void Refresh()
    {
        refreshBTN.interactable = false;
        _sessionList.enabled = false;
        _runnerHandler.JoinLobby();
    }

    private void Back()
    {
        SceneManager.LoadScene(SceneIndex.MainMenu);
    }
}

public static class SceneIndex
{
    public static int MainMenu => 0;
    public static int LobbyMenu => 1;
    public static int Game => 2;
}
