using Fusion;
using Fusion.Sockets;
using Redes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Host
{ 
    public class PlayerManager : NetworkBehaviour, INetworkRunnerCallbacks
    {
        public static Action<PlayerRef> OnPlayerJoined;
        public static Action<PlayerRef> OnPlayerLeft;

        [SerializeField] private NetworkPrefabRef _playerPrefab;
        [SerializeField] private PlayerView _playerViewPrefab;
        private Dictionary<PlayerRef, PlayerManagerData> _activePlayers = new();
        [SerializeField] CarModels _models;
        [SerializeField] Transform[] _startPositions;

        public static PlayerManager Instance { get; private set; }
        HostInputHandler _inputHandler;


        void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                _activePlayers.Add(player, new());
                OnPlayerJoined?.Invoke(player);
            }
        }

        void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (_activePlayers.TryGetValue(player, out var data))
            {
                runner.Despawn(data.PlayerObj.Object);
                _activePlayers.Remove(player);
                OnPlayerLeft?.Invoke(player);
            }
        }
        void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input)
        {
            input.Set(_inputHandler.GetData());
        }
        [Rpc(RpcSources.StateAuthority, RpcTargets.StateAuthority)]
        public void RPC_SpawnAll()
        {
            foreach (var (player, data) in _activePlayers)
            {
                RPC_Spawn(player);
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.StateAuthority)]
        public void RPC_Spawn(PlayerRef player)
        {
            if (!_activePlayers.TryGetValue(player, out var pData)) return;

            var pos = _startPositions[player.AsIndex].position;
            var rot = _startPositions[player.AsIndex].rotation;

            pData.PlayerObj = Runner.Spawn(_playerPrefab, pos, rot, player).GetComponent<Player>();

            SetView(player, pData);
        }
        public void SetView(PlayerRef player, PlayerManagerData data)
        {
            var view = Runner.Spawn(_playerViewPrefab, Vector3.zero, Quaternion.identity, player);
            view.transform.SetParent(_activePlayers[player].PlayerObj.transform);
            view.CarModelIndex = data.CarModel;
            view.CarColor = data.CarColor;
            view.PlayerName = data.Name;
        }

        #region Data Setter
        [Rpc(RpcSources.StateAuthority, RpcTargets.StateAuthority)]
        public void RPC_SetPlayerName(PlayerRef player, string name)
        {
            if (_activePlayers.ContainsKey(player))
            {
                _activePlayers[player].Name = name;
            }
        }
        [Rpc(RpcSources.StateAuthority, RpcTargets.StateAuthority)]
        public void RPC_SetPlayerCar(PlayerRef player, int carIndex)
        {
            if (_activePlayers.ContainsKey(player))
            {
                _activePlayers[player].CarModel = carIndex;
            }
        }
        [Rpc(RpcSources.StateAuthority, RpcTargets.StateAuthority)]
        public void RPC_SetPlayerColor(PlayerRef player, Color color)
        {
            if (_activePlayers.ContainsKey(player))
            {
                _activePlayers[player].CarColor = color;
            }
        }
        #endregion
        public Dictionary<PlayerRef, PlayerManagerData> GetPlayerList()
        {
            return _activePlayers;
        }

        public void ForEach(Action<PlayerRef, PlayerManagerData> action)
        {
            foreach(var (player, data) in _activePlayers)
            {
                action(player, data);
            }
        }

        #region MonoBehaviour
        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            _inputHandler = new HostInputHandler();
        }

        private void Update()
        {
            _inputHandler.UpdateInputs();
        }
        #endregion
        #region Network Runner Callbacks
        void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
           
        }

        void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
        void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner) { }
        void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
        void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
        void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
        void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner) { }
        void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner) { }
        void INetworkRunnerCallbacks.OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        void INetworkRunnerCallbacks.OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
        void INetworkRunnerCallbacks.OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

        #endregion
    }

    public class PlayerManagerData
    {
        public string Name { get; set; }
        public int CarModel { get; set; }
        public Color CarColor { get; set; }
        public Player PlayerObj { get; set; }
    }
}