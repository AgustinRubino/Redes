using Fusion;
using System.Collections.Generic;
using UnityEngine;

namespace Redes
{
    public class PlayerManager : NetworkBehaviour
    {
        [SerializeField] GameObject _playerPrefab;
        [SerializeField] PlayerConfigSO[] _configs;
        [SerializeField] Transform[] _startPositions;

        //private Dictionary<PlayerRef, Player> _players;
        //public Dictionary<PlayerRef, Player> Players => _players;

        private void OnEnable()
        {
            PlayerDetector.OnPlayerJoined += SpawnPlayer;
            PlayerDetector.OnPlayerLeft += DespawnPlayer;
        }
        private void OnDisable()
        {
            PlayerDetector.OnPlayerJoined -= SpawnPlayer;
            PlayerDetector.OnPlayerLeft -= DespawnPlayer;
        }

        private void SpawnPlayer(PlayerRef player)
        {
            if (player == Runner.LocalPlayer)
            {
                int count = Runner.SessionInfo.PlayerCount;
                var p = Runner.Spawn(_playerPrefab, _startPositions[count - 1].position, _startPositions[count - 1].rotation)
                    .GetComponent<Player>();
                ReferenceManager.Player = p;

                p.SetPlayerConfig(_configs[count - 1].config);
                Runner.SetPlayerObject(player, p.Object);
                //RPC_AddPlayer(player);
            }
        }
        private void DespawnPlayer(PlayerRef player)
        {
            if (player == Runner.LocalPlayer)
            {
                var p = Runner.GetPlayerObject(player);
                //RPC_RemovePlayer(player);
                Runner.Despawn(p);
            }
        }

        //[Rpc]
        //public void RPC_AddPlayer(PlayerRef id, Player p)
        //{
        //    if (_players == null) _players = new();
        //    if (_players.ContainsKey(id)) return;

        //    _players[id] = ReferenceManager.Player;
        //}
        //[Rpc]
        //public bool RPC_RemovePlayer(PlayerRef id)
        //{
        //    if (_players == null) return false;
        //    return _players.Remove(id);
        //}
    }
}
