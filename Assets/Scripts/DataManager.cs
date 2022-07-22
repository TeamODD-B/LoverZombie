using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    PlayerData Player = new PlayerData();
    string path;

    //싱글톤
    public static DataManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject); //오브젝트가 파괴되지 않고 유지

        path = Application.persistentDataPath + "/save"; //현재 프로젝트의 개별 경로 할당
    }
    public void Save()
    {
        string data = JsonUtility.ToJson(Player); //PlayerData -> Json
        File.WriteAllText(path, data);
    }
    public void Load()
    {
        string data = File.ReadAllText(path);
        Player = JsonUtility.FromJson<PlayerData>(data); //Json -> PlayeData
    }
}
