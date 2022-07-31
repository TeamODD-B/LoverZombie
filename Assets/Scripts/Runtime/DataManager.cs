using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : SingletonGeneric<DataManager>
{
    private PlayerData _player = new PlayerData();
    private string path;

    protected override void Awake()
    {
        base.Awake();
        path = Application.persistentDataPath + "/save";
    }

    public void Save()
    {
        string data = JsonUtility.ToJson(_player);
        File.WriteAllText(path, data);
    }

    public void Load()
    {
        string data = File.ReadAllText(path);
        _player = JsonUtility.FromJson<PlayerData>(data);
    }
}
