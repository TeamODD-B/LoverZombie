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
    private string _mainInsideEventDataPath;
    private string _subEventDataPath;
    private string _subInsideEventDataPath;
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
        _mainInsideEventDataPath = Path.Combine(Application.dataPath, "Presets/Event/Main/Inside");
        _subEventDataPath = Path.Combine(Application.dataPath, "Presets/Event/Sub"); 
        _subInsideEventDataPath = Path.Combine(Application.dataPath, "Presets/Event/Sub/Inside");
    }

    private void LoadEventJsonFileNames()
    {
        _mainEventDataFileNameList = new List<string>();
        _mainInsideEventDataFileNameList = new List<string>();
        _subEventDataFileNameList = new List<string>();
        _subInsideEventDataFileNameList = new List<string>();

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

        // ���� �λ��̵� �̺�Ʈ
        bool isMainInsideDirectoryExists = Directory.Exists(_mainInsideEventDataPath);
        if (!isMainInsideDirectoryExists)
        {
            Debug.Log("���� �λ��̵� �̺�Ʈ ���� ���� �������� ����!");
        }
        else
        {
            DirectoryInfo directory = new DirectoryInfo(_mainInsideEventDataPath);
            foreach (var file in directory.GetFiles())
            {
                string fullPath = Path.Combine(_mainInsideEventDataPath, file.Name);
                bool isJsonFile = Path.GetExtension(fullPath) == ".json"; //meta���ϰ� json������ json���ϸ�
                if (!isJsonFile)
                {
                    continue;
                }
                _mainInsideEventDataFileNameList.Add(file.Name);
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

        // ���� �λ��̵� �̺�Ʈ
        bool isSubTitleDirectoryExists = Directory.Exists(_subInsideEventDataPath);
        if (!isSubTitleDirectoryExists)
        {
            Debug.Log("���� �λ��̵� �̺�Ʈ ���� ���� �������� ����!");
        }
        else
        {
            DirectoryInfo directory = new DirectoryInfo(_subInsideEventDataPath);
            foreach (var file in directory.GetFiles())
            {
                string fullPath = Path.Combine(_subInsideEventDataPath, file.Name);
                bool isJsonFile = Path.GetExtension(fullPath) == ".json"; //meta���ϰ� json������ json���ϸ�
                if (!isJsonFile)
                {
                    continue;
                }
                _subInsideEventDataFileNameList.Add(file.Name);
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
        SavePlayerData();
    }

    public string DecideNextEvent(string eventType, string nextEventId = "")
    {
        string eventName = "";
        string path = "";
        int searchIndex = 0;

        // ���� �̺�Ʈ ����
        switch (eventType)
        {
            case "Main":
                //Ŀ���� �̿��� ���������� �����̺�Ʈ ����
                eventName = _mainEventDataFileNameList[MainEventCursor];
                path = Path.Combine(_mainEventDataPath, eventName);
                MainEventCursor++;
                TotalEventProgressCount++;
                break;
            case "MainInside":
                // MainInside List�� ���� �̸��� ���������� �˻�
                while ((nextEventId + ".json") != eventName)
                {
                    eventName = _mainInsideEventDataFileNameList[searchIndex];
                    searchIndex++;
                }
                path = Path.Combine(_mainInsideEventDataPath, eventName);
                break;
            case "Sub":
                //����
                int eventRandomNumber = Random.Range(0, _subEventDataFileNameList.Count);
                eventName = _subEventDataFileNameList[eventRandomNumber];
                path = Path.Combine(_subEventDataPath, eventName);
                TotalEventProgressCount++;
                break;
            case "SubInside":
                // SubInside List�� ���� �̸��� ���������� �˻�
                while ((nextEventId + ".json") != eventName)
                {
                    eventName = _subInsideEventDataFileNameList[searchIndex];
                    searchIndex++;
                }
                path = Path.Combine(_subInsideEventDataPath, eventName);
                break;
        }

        return path;
    }

    public void LoadEventData(string eventType, string nextEventId = "")
    {
        // ���� �а� �����
        string path = DecideNextEvent(eventType, nextEventId);
        string jsonData = File.ReadAllText(path);
        EventData = JsonUtility.FromJson<EventData>(jsonData);

        //�����Ȳ ������ �� �÷��̾� ������ ����
        SaveEventProgressData();
    }

}
