using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    PlayerData Player = new PlayerData();
    string path;

    //�̱���
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
        DontDestroyOnLoad(this.gameObject); //������Ʈ�� �ı����� �ʰ� ����

        path = Application.persistentDataPath + "/save"; //���� ������Ʈ�� ���� ��� �Ҵ�
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
