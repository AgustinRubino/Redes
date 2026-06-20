using Fusion;
using UnityEngine;
using System;

namespace Redes
{
    public class PlayerDetector : SimulationBehaviour, IPlayerJoined, IPlayerLeft
    {
        public static event Action<PlayerRef> OnPlayerJoined = delegate {};
        public static event Action<PlayerRef> OnPlayerLeft = delegate {};

        public void PlayerJoined(PlayerRef player)
        {
            Debug.Log("Player " + player + " Joined!");
            OnPlayerJoined(player);
        }

        public void PlayerLeft(PlayerRef player)
        {
            OnPlayerLeft(player);
        }
    }
}