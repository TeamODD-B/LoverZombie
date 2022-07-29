using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    
    private PlayerData _player = new PlayerData();
    private string path;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

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
