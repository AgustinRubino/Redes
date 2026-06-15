using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    public static event Action OnPlayerJoined;

    public GameObject PlayerPrefab;

    public GameObject cameraPrefab;
    public GameObject cameraPivot;
    public GameObject carCamera;

    public PlayerInfo[] playerInfo;

    public PlayerModel localPlayerModel;


    public static PlayerManager Instance { get; private set; }



    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            OnPlayerJoined?.Invoke();
            Instance = this;
            GameManager.OnGameStateChanged += OnGameStateChanged;

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

        if (GameManager.Instance != null)
            OnGameStateChanged(GameManager.Instance.GameState);
    }

    public void PlayerLeft(PlayerRef player)
    {
        //throw new NotImplementedException();
    }

    private void OnGameStateChanged(EGameState state)
    {
        if (state == EGameState.WaitingPlayers) SetPivotCamera();
        else SetGameCamera();
    }
    public void SetPivotCamera()
    {
        cameraPivot.SetActive(true);
        if (carCamera != null) carCamera.SetActive(false);
    }
    private void SetGameCamera()
    {
        if (carCamera == null)
        {
            carCamera = Instantiate(cameraPrefab, localPlayerModel.transform);
        }
        carCamera.SetActive(true);
        cameraPivot.SetActive(false);
    }

    [Serializable]
    public class PlayerInfo
    {
        public Color color = Color.gray;
        public Transform position;
    }
}