using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Host
{
    public class MenuRunnerHandler : MonoBehaviour, INetworkRunnerCallbacks
    {
        private NetworkRunner _runner;
        public event Action<List<SessionInfo>> OnSessionListUpdate;
        public event Action OnLobbyNotFound;
        public event Action OnLobbyFound;

        //[SerializeField] private NetworkPrefabRef _playerPrefab;
        //private Dictionary<PlayerRef, NetworkObject> _activePlayers = new Dictionary<PlayerRef, NetworkObject>();

        HostInputHandler _inputHandler;

        //async void StartGame(GameMode mode)
        //{
        //    // Create the Fusion runner and let it know that we will be providing user input
        //    _runner = gameObject.AddComponent<NetworkRunner>();
        //    _runner.ProvideInput = true;

        //    // Create the NetworkSceneInfo from the current scene
        //    var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        //    var sceneInfo = new NetworkSceneInfo();
        //    if (scene.IsValid)
        //    {
        //        sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        //    }

        //    // Start or join (depends on gamemode) a session with a specific name
        //    await _runner.StartGame(new StartGameArgs()
        //    {
        //        GameMode = mode,
        //        SessionName = "TestRoom",
        //        Scene = scene,
        //        SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        //    });
        //}
        public void JoinLobby()
        {
            if (_runner == null)
                _runner = new GameObject("Runner").AddComponent<NetworkRunner>();
                _runner.AddCallbacks(this);

            JoinLobbyAsync();
        }
        async void JoinLobbyAsync()
        {
            var result = await _runner.JoinSessionLobby(SessionLobby.Custom, "Normal lobby");

            if (!result.Ok)
            {
                Debug.LogError($"[Custom Error] Unable to Join Lobby");

                OnLobbyNotFound?.Invoke();
            }
            else
            {
                Debug.Log($"[Custom Msg] Joined Lobby");

                OnLobbyFound?.Invoke();
            }
        }

        public async void LeaveLobbyAsync()
        {
            await _runner.Shutdown(false);
        }


        void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            //if (runner.IsServer)
            //{
            //    // Create a unique position for the player
            //    Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
            //    NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            //    // Keep track of the player avatars for easy access
            //    _activePlayers.Add(player, networkPlayerObject);
            //}
        }

        void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            //if (_activePlayers.TryGetValue(player, out NetworkObject networkObject))
            //{
            //    runner.Despawn(networkObject);
            //    _activePlayers.Remove(player);
            //}
        }
        void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input)
        {
            //var data = new InputData();

            //if (Input.GetKey(KeyCode.W))
            //    data.forward += 1;
            //if (Input.GetKey(KeyCode.S))
            //    data.forward -= 1;
            //if (Input.GetKey(KeyCode.D))
            //    data.right += 1;
            //if (Input.GetKey(KeyCode.A))
            //    data.right -= 1;

            //data.Buttons.Set(InputData.MouseButton0, _mouseButton0);
            //_mouseButton0 = false;

            input.Set(_inputHandler.GetData());
        }


        #region StartGame
        public void CreateGame(string sessionName, string sceneName)
        {
            int sceneIndex = SceneUtility.GetBuildIndexByScenePath("Scenes/LevelScene");
            CreateGame(sessionName, sceneIndex);
        }
        public async void CreateGame(string sessionName, int sceneIndex)
        {
            //await InitializeGame(GameMode.Host, sessionName, SceneUtility.GetBuildIndexByScenePath($"Scenes/{sceneName}"));
            
            if (sceneIndex < 0)
            {
                Debug.LogError("[Custom Error] LevelScene not found in Build Settings.");
                return;
            }
            await InitializeGame(GameMode.Host, sessionName, sceneIndex);
        }

        public async void JoinGame(SessionInfo sessionInfo)
        {
            await InitializeGame(GameMode.Client, sessionInfo.Name, SceneManager.GetActiveScene().buildIndex);
        }

        async Task InitializeGame(GameMode gameMode, string sessionName, int sceneIndex)
        {
            _runner.ProvideInput = true;

            var result = await _runner.StartGame(new StartGameArgs()
            {
                GameMode = gameMode,
                Scene = SceneRef.FromIndex(sceneIndex),
                SessionName = sessionName,
                PlayerCount = 8
            });

            if (!result.Ok)
            {
                Debug.LogError($"[Custom Error] Unable to Start Game");
            }
            else
            {
                Debug.Log($"[Custom Msg] Game Started");
            }
        }

        #endregion

        //private void OnGUI()
        //{
        //    if (_runner == null)
        //    {
        //        if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
        //        {
        //            StartGame(GameMode.Host);
        //        }

        //        if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
        //        {
        //            StartGame(GameMode.Client);
        //        }
        //    }
        //}

        #region Network Runner Callbacks
        void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) {
            OnSessionListUpdate?.Invoke(sessionList);
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
}