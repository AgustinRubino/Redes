using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SimulationBehaviour, IPlayerJoined
{
    public static event Action OnPlayerJoined;

    public GameObject PlayerPrefab;
    public GameObject cameraPivot;
    public int maxPlayers = 5;
    public CarInfo[] _carInfo;

    //[Space(10)]
    //[SerializeField] private List<NetworkObject> _players = new();
    //[Networked] public List<NetworkObject> Players => _players;


    void IPlayerJoined.PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            OnPlayerJoined?.Invoke();

            var car = Runner.Spawn(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            SetPlayer(car.GetComponent<CarModel>(), Runner.SessionInfo.PlayerCount -1);
        }
    }

    private void SetPlayer(CarModel model, int index)
    {
        if (index >= _carInfo.Length || index < 0)
        {
            model.MeshColor = Color.gray;
        }
        CarInfo info = _carInfo[index];
        model.MeshColor = info.color;
        model.SetPosition(info.position.position);
        model.SetRotation(info.position.rotation);
        Instantiate(cameraPivot, model.transform);
    }

    [Serializable]
    public class CarInfo
    {
        public Color color = Color.gray;
        public Transform position;
    }
}