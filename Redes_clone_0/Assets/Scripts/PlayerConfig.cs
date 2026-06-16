using System;
using UnityEngine;

[Serializable]
public class PlayerConfig
{
    public string name = "default";
    public Color color = Color.white;
    public int carModelIndex;
    public int destroyedSoundIndex;
    public int winSoundIndex;
}
