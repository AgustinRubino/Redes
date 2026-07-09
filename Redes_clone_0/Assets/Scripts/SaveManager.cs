using System.IO;
using UnityEngine;

public static class SaveManager
{
    public static void Save<T>(T data) where T : IStorableData
    {
        string dataPath = Path.Combine(Application.persistentDataPath, data.DataName);
        string result = JsonUtility.ToJson(data, true);
        Debug.Log(result);
        File.WriteAllText(dataPath, result);
    }

    public static T Load<T>() where T : IStorableData, new()
    {
        T data = new();
        string dataPath = Path.Combine(Application.persistentDataPath, data.DataName);

        if (File.Exists(dataPath))
        {
            string json = File.ReadAllText(dataPath);
            data = JsonUtility.FromJson<T>(json);
            Debug.Log("Succesful load");
        }
        else Debug.Log($"Cant load data from '{dataPath}, dont exist'");

        return data;
    }
}

public interface IStorableData
{
    public string DataName { get; }
}