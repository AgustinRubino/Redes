using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    public static event Action OnPlayerJoined;

    public GameObject PlayerPrefab;

    public GameObject cameraPivot;
    public GameObject cameraPrefab;
    public int maxPlayers = 5;
    public PlayerInfo[] playerInfo;

    public PlayerModel localPlayerModel;

    public static PlayerManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void IPlayerJoined.PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            OnPlayerJoined?.Invoke();

            var car = Runner.Spawn(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            localPlayerModel = car.GetComponent<PlayerModel>();
            SetPlayer(localPlayerModel, Runner.SessionInfo.PlayerCount -1);
        }
    }

    private void SetPlayer(PlayerModel model, int index)
    {
        if (index >= playerInfo.Length || index < 0)
        {
            model.MeshColor = Color.gray;
        }
        PlayerInfo info = playerInfo[index];

        model.MeshColor = info.color;
        model.SetPosition(info.position.position);
        model.SetRotation(info.position.rotation);

        Instantiate(cameraPrefab, model.transform);
        cameraPivot.SetActive(false);
    }

    public void PlayerLeft(PlayerRef player)
    {
        //throw new NotImplementedException();
    }

    [Serializable]
    public class PlayerInfo
    {
        public Color color = Color.gray;
        public Transform position;
    }
}