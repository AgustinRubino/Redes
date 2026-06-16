using Fusion;
using UnityEngine;

public class SingletonManager : MonoBehaviour
{
    public static SingletonManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    [field: SerializeField] public PlayerModel Player { get; set; }
    [field: SerializeField] public NetworkRunner Runner { get; set; }
}
