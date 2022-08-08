using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : SingletonGeneric<DataManager>
{
    // Data
    public PlayerData PlayerData = new PlayerData();
    public EventData EventData = new EventData();
    // Path
    private string _playerDataPath;
    public string PlayerDataPath 
    {
        get
        {
            return _playerDataPath;
        }
    }
    private string _subEventDataPath;
    private string _subTitleEventDataPath;
    // File Name List
    private List<string> _subEventDataFileNameList;
    private List<string> _subTitleEventDataFileNameList;

    protected override void Awake()
    {
        base.Awake();

        //Player Data Init
        PlayerData.Init();

        //Path Init
        InitPath();

        //Json File Name Load
        LoadEventJsonFileNames();
    }

    private void InitPath()
    {
        _playerDataPath = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        _subEventDataPath = Path.Combine(Application.dataPath, "Presets/Event/Sub"); //����� ����� persistentDataPath�� ������ ��
        _subTitleEventDataPath = Path.Combine(Application.dataPath, "Presets/Event/Sub/SubTitle"); //����� ����� persistentDataPath�� ������ ��
    }

    private void LoadEventJsonFileNames()
    {
        _subEventDataFileNameList = new List<string>();
        _subTitleEventDataFileNameList = new List<string>();

        #region ����� ���� �� ������ �߰��� ���� 
        //1. Json���ϵ��� ����Ǵ� ������ �����ϴ��� �˻��ϰ� ���ٸ� ����������.
        //2. ���� �ȿ� Json ���ϵ��� �ۼ��������. ���¹��� ��, ��ġ�� �ٿ�޾ƾ� �Ѵ�.
        //2-1. ���� ���� ã�Ƽ� �̿��غ���. 
        #endregion
        // ���� �̺�Ʈ
        bool isSubDirectoryExists = Directory.Exists(_subEventDataPath);
        if (!isSubDirectoryExists)
        {
            Debug.Log("���� �̺�Ʈ ���� ���� �������� ����!");
        }
        else
        {
            DirectoryInfo directory = new DirectoryInfo(_subEventDataPath);
            foreach (var file in directory.GetFiles())
            {
                string fullPath = Path.Combine(_subEventDataPath, file.Name);
                bool isJsonFile = Path.GetExtension(fullPath) == ".json"; //meta���ϰ� json������ json���ϸ�
                if (!isJsonFile)
                {
                    continue;
                }
                _subEventDataFileNameList.Add(file.Name);
            }
        }

        // ���� Ÿ��Ʋ �̺�Ʈ
        bool isSubTitleDirectoryExists = Directory.Exists(_subTitleEventDataPath);
        if (!isSubTitleDirectoryExists)
        {
            Debug.Log("���� Ÿ��Ʋ �̺�Ʈ ���� ���� �������� ����!");
        }
        else
        {
            DirectoryInfo directory = new DirectoryInfo(_subTitleEventDataPath);
            foreach (var file in directory.GetFiles())
            {
                string fullPath = Path.Combine(_subTitleEventDataPath, file.Name);
                bool isJsonFile = Path.GetExtension(fullPath) == ".json"; //meta���ϰ� json������ json���ϸ�
                if (!isJsonFile)
                {
                    continue;
                }
                _subTitleEventDataFileNameList.Add(file.Name);
            }
        }
    }

    public void SavePlayerData()
    {
        string data = JsonUtility.ToJson(PlayerData);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
        string code = System.Convert.ToBase64String(bytes);
        File.WriteAllText(_playerDataPath, code);
    }

    public void LoadPlayerData()
    {
        string code = File.ReadAllText(_playerDataPath);
        byte[] bytes = System.Convert.FromBase64String(code);
        string data = System.Text.Encoding.UTF8.GetString(bytes);
        PlayerData = JsonUtility.FromJson<PlayerData>(data);
    }

    public void LoadEventData(string eventType, string nextEventId = "")
    {
        string type = eventType;
        string nextId = nextEventId;
        string eventName = "";
        string path = "";
        // ���� �̺�Ʈ ����
        switch (type)
        {
            case "Main":
                // ���⿡ ���� �̺�Ʈ �Լ� ȣ�� ���� �ۼ�
                break;
            case "Sub":
                // ���� �̺�Ʈ �������� ����
                int eventRandomNumber = Random.Range(0, _subEventDataFileNameList.Count);
                eventName = _subEventDataFileNameList[eventRandomNumber];
                path = Path.Combine(_subEventDataPath, eventName);
                break;
            case "SubTitle":
                // SubTitle List�� ���� �̸��� ���������� �˻�
                int jsonSearchIndex = 0;
                while ((nextId + ".json") != eventName)
                {
                    eventName = _subTitleEventDataFileNameList[jsonSearchIndex];
                    jsonSearchIndex++;
                }
                path = Path.Combine(_subTitleEventDataPath, eventName);
                break;
        }

        // ���� �а� �����
        string jsonData = File.ReadAllText(path);
        EventData = JsonUtility.FromJson<EventData>(jsonData);
    }
}
