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
        //모바일 빌드시 dataPath를 persistentDataPath로 변경할 것
        _playerDataPath = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        _mainEventDataPath = Path.Combine(Application.dataPath, "Presets/Event/Main");
        _subEventDataPath = Path.Combine(Application.dataPath, "Presets/Event/Sub");
        _defaultEventDataPath = Path.Combine(Application.dataPath, "Presets/Event");
    }

    private void LoadEventJsonFileNames()
    {
        _mainEventDataFileNameList = new List<string>();
        _subEventDataFileNameList = new List<string>();

        #region 모바일 빌드 시 앞으로 추가할 내용 
        //1. Json파일들이 저장되는 폴더가 존재하는지 검사하고 없다면 만들어줘야함.
        //2. 폴더 안에 Json 파일들을 작성해줘야함. 에셋번들 즉, 패치를 다운받아야 한다.
        //2-1. 무료 서버 찾아서 이용해보자. 
        #endregion
        
        // 메인 이벤트
        bool isMainDirectoryExists = Directory.Exists(_mainEventDataPath);
        if (!isMainDirectoryExists)
        {
            Debug.Log("메인 이벤트 저장 폴더 존재하지 않음!");
        }
        else
        {
            DirectoryInfo directory = new DirectoryInfo(_mainEventDataPath);
            foreach (var file in directory.GetFiles())
            {
                string fullPath = Path.Combine(_mainEventDataPath, file.Name);
                bool isJsonFile = Path.GetExtension(fullPath) == ".json"; //meta파일과 json파일중 json파일만
                if (!isJsonFile)
                {
                    continue;
                }
                _mainEventDataFileNameList.Add(file.Name);
            }
        }

        // 서브 이벤트
        bool isSubDirectoryExists = Directory.Exists(_subEventDataPath);
        if (!isSubDirectoryExists)
        {
            Debug.Log("서브 이벤트 저장 폴더 존재하지 않음!");
        }
        else
        {
            DirectoryInfo directory = new DirectoryInfo(_subEventDataPath);
            foreach (var file in directory.GetFiles())
            {
                string fullPath = Path.Combine(_subEventDataPath, file.Name);
                bool isJsonFile = Path.GetExtension(fullPath) == ".json"; //meta파일과 json파일중 json파일만
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
                    eventName = _mainEventDataFileNameList[MainEventCursor]; // 메인 이벤트 커서는 메인 이벤트를 통과하는 선택지를 눌렀을 때 증가하도록 처리
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
        // 파일 읽고 덮어쓰기
        string path = DecideNextEventPath(eventType, nextEventId);
        string jsonData = File.ReadAllText(path);
        EventData = JsonUtility.FromJson<EventData>(jsonData);

        //진행상황 데이터 및 플레이어 데이터 저장
        SaveEventProgressData();
        SavePlayerData();
    }
}
