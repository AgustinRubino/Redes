using System;
using UnityEditor;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerData Data { get; private set; }

    private void Awake()
    {
        if (Data != null)
        {
            Destroy(gameObject);
            return;
        }
        Data = SaveManager.Load<PlayerData>();
        DontDestroyOnLoad(gameObject);
    }

    private void OnDisable()
    {
        if ( Data != null ) SaveManager.Save(Data);
    }
    private void OnEnable()
    {
        if (Data == null) Data = SaveManager.Load<PlayerData>();
    }
}

public class PlayerData : IStorableData
{
    public string DataName => "uData.dat";

    public string name = "";
    public int carIndex = 0;
    public Color carColor = Color.white;
}