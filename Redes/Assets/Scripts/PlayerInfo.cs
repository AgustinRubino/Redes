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
        if ( Data != null ) {
            Debug.Log("Exit and Saved Data");
            SaveManager.Save(Data); }
    }
    private void OnEnable()
    {
        if (Data == null) Data = SaveManager.Load<PlayerData>();
    }
}
