using Fusion;
using Redes;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_ReadyScreen : NetworkBehaviour
{
    public event Action OnPlayersReady;

    [SerializeField] UI_PlayerItem _prefab;
    [Space(10)]
    [SerializeField] Button _colorBTN;
    [SerializeField] Button _carBTN;
    [SerializeField] Button _quitBTN;
    [SerializeField] Button _readyBTN;
    [Space(10)]
    [SerializeField] UI_ColorMenu _colorMenu;
    [SerializeField] UI_CarMenu _carMenu;
    [Space(10)]
    [SerializeField] TMP_Text _playerName;
    [SerializeField] TMP_Text _readyLeft;
    [SerializeField] Transform _grid;
    [SerializeField] CarModels _carModels;
    [SerializeField] GameObject WaitScreen;
    [Networked, OnChangedRender(nameof(OnReadyPlayersRender))] public int ReadyPlayersCount { get; set; }

    Dictionary<PlayerRef, UI_PlayerItem> _playerItems;
    public override void Spawned()
    {
        _playerItems = new();

        _readyBTN.onClick.AddListener(() => RPC_Ready(Runner.LocalPlayer));
        _quitBTN.onClick.AddListener(() => SceneManager.LoadScene(SceneIndex.MainMenu));
        _colorBTN.onClick.AddListener(OpenColorMenu);
        _carBTN.onClick.AddListener(OpenCarMenu);

        if (!Runner.IsServer) return;


        if(Host.PlayerManager.Instance != null)
        {
            foreach (var (player, data) in Host.PlayerManager.Instance.GetPlayerList())
            {
                var item = Runner.Spawn(_prefab, Vector3.zero, Quaternion.identity);
                var trans = item.GetComponent<NetworkTransform>().transform;
                trans.parent = _grid;
                trans.localPosition = Vector3.zero;
                trans.localScale = Vector3.zero;
                trans.localRotation = Quaternion.identity;

                item.Name = data.Name;
                item.Car = _carModels.Models[data.CarModel].name;
                item.Color = data.CarColor;
                item.Ready = false;
                _playerItems.Add(player, item);
            }
        }

        CheckPlayersReady();
        RPC_Update();

    }

    private void OnEnable()
    {
        Host.PlayerManager.OnPlayerJoined += joined;
        Host.PlayerManager.OnPlayerLeft += Left;
    }
    private void OnDisable()
    {
        Host.PlayerManager.OnPlayerJoined -= joined;
        Host.PlayerManager.OnPlayerLeft -= Left;
    }

    private void Left(PlayerRef player)
    {
        if (!Runner.IsServer) return;

        if (_playerItems.TryGetValue(player, out var item))
        {
            Runner.Despawn(item.Object);
            _playerItems.Remove(player);
        }
        RPC_Update();
    }

    private void joined(PlayerRef player)
    {
        if (!Runner.IsServer) return;

        Debug.Log("added player: " + player);
        if (!_playerItems.ContainsKey(player))
        {
            var item = Runner.Spawn(_prefab, Vector3.zero, Quaternion.identity);
            var trans = item.GetComponent<NetworkTransform>().transform;
            trans.parent = _grid;
            trans.localPosition = Vector3.zero;
            trans.localScale = Vector3.one;
            trans.localRotation = Quaternion.identity;

            var data = Host.PlayerManager.Instance.GetPlayerList()[player];
            item.Name = data.Name;
            item.Car = _carModels.Models[data.CarModel].name;
            item.Color = data.CarColor;
            item.Ready = false;
            _playerItems.Add(player, item);
        }

        RPC_Update();
    }

    #region Menu
    public void OpenColorMenu() {
        EnableReadyMenu(false);
        _colorMenu.gameObject.SetActive(true);
        _colorMenu.Activate(Color.white, CloseColorMenu);
    }
    public void OpenCarMenu() {
        EnableReadyMenu(false);
        _carMenu.gameObject.SetActive(true);
        _carMenu.Activate(0,CloseCarMenu);
    }

    private void CloseColorMenu(Color color) {
        RPC_Color(Runner.LocalPlayer, color);
        EnableReadyMenu(true);
    }
    private void CloseCarMenu(int index) { 
    
        RPC_CarModel(Runner.LocalPlayer, index);
        _carMenu.gameObject.SetActive(false);
        EnableReadyMenu(true);
    }

    private void EnableReadyMenu(bool enable)
    {
        _colorBTN.interactable = enable;
        _carBTN.interactable = enable;
        _quitBTN.interactable = enable;
        _readyBTN.interactable = enable;
    }
    #endregion
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_CarModel(PlayerRef player, int index)
    {
        Host.PlayerManager.Instance.RPC_SetPlayerCar(player, index);

    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_Color(PlayerRef player, Color color)
    {
        Host.PlayerManager.Instance.RPC_SetPlayerColor(player, color);
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_Name(PlayerRef player, string name)
    {
        Host.PlayerManager.Instance.RPC_SetPlayerName(player, name);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_Ready(PlayerRef player)
    {
        _playerItems[player].Ready = !_playerItems[player].Ready;
        CheckPlayersReady();
    }

    private void CheckPlayersReady()
    {
        int counter = 0;
        foreach (var item in _playerItems.Values)
        {
            if (item.Ready) counter++; 
        }
        ReadyPlayersCount = counter;

        if (counter < 2) return;
        if (counter >= _playerItems.Count) 
        {
            OnPlayersReady?.Invoke();
            RPC_WaitScreenDesable();
        }
            
    }

    [Rpc]
    public void RPC_WaitScreenDesable()
    {
        WaitScreen.gameObject.SetActive(false);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_Update()
    {
        if (Host.Player.Local != null)
        {
            _playerName.text = Host.Player.Local.View.PlayerName;
        }
        _readyLeft.text = $"players ready: {ReadyPlayersCount} / {Mathf.Max(_playerItems.Count, 2)}";
    }

    void OnReadyPlayersRender()
    {
        _readyLeft.text = $"players ready: {ReadyPlayersCount} / {Mathf.Max(_playerItems.Count, 2)}";
    }
}
