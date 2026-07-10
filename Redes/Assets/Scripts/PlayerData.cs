using System;
using UnityEditor;
using UnityEngine;

public class PlayerData : IStorableData
{
    public string DataName => "uData.dat";

    public string name = "";
    public int carIndex = 0;
    public Color carColor = Color.red;
}