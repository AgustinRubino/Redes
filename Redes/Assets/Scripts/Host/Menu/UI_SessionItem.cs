using System;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SessionItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _playerCount;
    [SerializeField] private Button _joinBtn;

    private SessionInfo _sessionInfo;

    public event Action<SessionInfo> OnJoinSession;

    private void Awake()
    {
        _joinBtn.onClick.AddListener(OnClick);
    }

    public void Initialize(SessionInfo sessionInfo)
    {
        _sessionInfo = sessionInfo;
        _name.text = _sessionInfo.Name;
        float a = (float)_sessionInfo.PlayerCount / _sessionInfo.MaxPlayers;
        string color = a >= 1 ? "red" : (a > 0.5f) ? "yellow" : "green";
        _playerCount.text = $"Players: <color={ color}>{_sessionInfo.PlayerCount}/{_sessionInfo.MaxPlayers}";

        _joinBtn.enabled = _sessionInfo.PlayerCount < _sessionInfo.MaxPlayers;
    }

    void OnClick()
    {
        OnJoinSession(_sessionInfo);
    }
}