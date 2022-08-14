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
    private string _mainEventDataPath;
    private string _subEventDataPath;
    private string _defaultEventDataPath;
    // File Name List
    private List<string> _mainEventDataFileNameList;
    private List<string> _mainInsideEventDataFileNameList;
    private List<string> _subEventDataFileNameList;
    private List<string> _subInsideEventDataFileNameList;
    //Var
    public int MainEventCursor = 0;
    public int TotalEventProgressCount = 0;

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
        //����� ����� dataPath�� persistentDataPath�� ������ ��
        _playerDataPath = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        _mainEventDataPath = Path.Combine(Application.dataPath, "Presets/Event/Main");
        _subEventDataPath = Path.Combine(Application.dataPath, "Presets/Event/Sub");
        _defaultEventDataPath = Path.Combine(Application.dataPath, "Presets/Event");
    }

    private void LoadEventJsonFileNames()
    {
        _mainEventDataFileNameList = new List<string>();
        _subEventDataFileNameList = new List<string>();

        #region ����� ���� �� ������ �߰��� ���� 
        //1. Json���ϵ��� ����Ǵ� ������ �����ϴ��� �˻��ϰ� ���ٸ� ����������.
        //2. ���� �ȿ� Json ���ϵ��� �ۼ��������. ���¹��� ��, ��ġ�� �ٿ�޾ƾ� �Ѵ�.
        //2-1. ���� ���� ã�Ƽ� �̿��غ���. 
        #endregion
        
        // ���� �̺�Ʈ
        bool isMainDirectoryExists = Directory.Exists(_mainEventDataPath);
        if (!isMainDirectoryExists)
        {
            Debug.Log("���� �̺�Ʈ ���� ���� �������� ����!");
        }
        else
        {
            DirectoryInfo directory = new DirectoryInfo(_mainEventDataPath);
            foreach (var file in directory.GetFiles())
            {
                string fullPath = Path.Combine(_mainEventDataPath, file.Name);
                bool isJsonFile = Path.GetExtension(fullPath) == ".json"; //meta���ϰ� json������ json���ϸ�
                if (!isJsonFile)
                {
                    continue;
                }
                _mainEventDataFileNameList.Add(file.Name);
            }
        }

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

    public void SaveEventProgressData()
    {
        PlayerData.LastEventType = EventData.Type;
        PlayerData.LastEventId = EventData.EventId;
        PlayerData.TotalEventProgressCount = TotalEventProgressCount;
        PlayerData.MainEventCursor = MainEventCursor;
    }

    public string DecideNextEventPath(string eventType, string nextEventId = "")
    {
        string eventName = nextEventId;
        if (eventName == "")
        {
            switch (eventType)
            {
                case "Main":
                    eventName = _mainEventDataFileNameList[MainEventCursor]; // ���� �̺�Ʈ Ŀ���� ���� �̺�Ʈ�� ����ϴ� �������� ������ �� �����ϵ��� ó��
                    break;
                case "Sub":
                    int eventRandomNumber = Random.Range(0, _subEventDataFileNameList.Count);
                    eventName = _subEventDataFileNameList[eventRandomNumber];
                    break;
            }
        }
        else {
            eventName += ".json";
        }

        string path = Path.Combine(_defaultEventDataPath, eventType, eventName);
        TotalEventProgressCount++;

        return path;
    }

    public void LoadEventData(string eventType, string nextEventId = "")
    {
        // ���� �а� �����
        string path = DecideNextEventPath(eventType, nextEventId);
        string jsonData = File.ReadAllText(path);
        EventData = JsonUtility.FromJson<EventData>(jsonData);

        //�����Ȳ ������ �� �÷��̾� ������ ����
        SaveEventProgressData();
        SavePlayerData();
    }
}
