using Fusion;
using Host;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SessionList : MonoBehaviour
{
    [SerializeField] private UI_SessionItem _sessionItemPrefab;

    [SerializeField] private MenuRunnerHandler _networkRunnerHandler;

    [SerializeField] private TMP_Text _statusText;
    [SerializeField] private GameObject _findingText;

    [SerializeField] private VerticalLayoutGroup _verticalLayout;
    private void Awake()
    {
        if (_networkRunnerHandler == null)
            _networkRunnerHandler = GetComponentInParent<MenuRunnerHandler>();

        //_networkRunnerHandler.OnSessionListUpdate += ReceiveSessionList;
        _networkRunnerHandler.OnSessionListUpdate += ReceiveSessionList;
        _networkRunnerHandler.OnLobbyFound += Found;
        //_networkRunnerHandler.OnLobbyNotFound += NotFound;
    }
    private void OnEnable()
    {
        _networkRunnerHandler.JoinLobby();
        _findingText.SetActive(true);
    }
    private void OnDisable()
    {
        _findingText.SetActive(true);
        _statusText.gameObject.SetActive(false);
        ClearBrowser();
        //_networkRunnerHandler.OnSessionListUpdate -= ReceiveSessionList;
    }

    private void OnDestroy()
    {
        _networkRunnerHandler.OnLobbyFound -= Found;
        //_networkRunnerHandler.OnLobbyNotFound -= NotFound;
        _networkRunnerHandler.OnSessionListUpdate -= ReceiveSessionList;
    }

    private void Found()
    {
        _findingText.SetActive(false);
        _statusText.gameObject.SetActive(true);
        _statusText.text = "no sessions found";
    }

    void ReceiveSessionList(List<SessionInfo> sessionList)
    {
        if (sessionList.Count == 0)
        {
            _statusText.gameObject.SetActive(true);
            return;
        }
        _statusText.gameObject.SetActive(false);

        foreach (var sessionInfo in sessionList)
        {
            AddToSessionBrowser(sessionInfo);
        }
    }

    void ClearBrowser()
    {
        foreach (Transform child in _verticalLayout.transform)
        {
            Destroy(child.gameObject);
        }

        _statusText.gameObject.SetActive(false);
    }

    void AddToSessionBrowser(SessionInfo sessionInfo)
    {
        var sessionItem = Instantiate(_sessionItemPrefab, _verticalLayout.transform);
        sessionItem.Initialize(sessionInfo);
        sessionItem.OnJoinSession += JoinSelectedSession;
    }

    void JoinSelectedSession(SessionInfo sessionInfo)
    {
        _networkRunnerHandler.JoinGame(sessionInfo);
    }
}
