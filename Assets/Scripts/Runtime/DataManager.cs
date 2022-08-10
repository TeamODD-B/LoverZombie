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
        //모바일 빌드시 dataPath를 persistentDataPath로 변경할 것
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

        // 메인 인사이드 이벤트
        bool isMainInsideDirectoryExists = Directory.Exists(_mainInsideEventDataPath);
        if (!isMainInsideDirectoryExists)
        {
            Debug.Log("메인 인사이드 이벤트 저장 폴더 존재하지 않음!");
        }
        else
        {
            DirectoryInfo directory = new DirectoryInfo(_mainInsideEventDataPath);
            foreach (var file in directory.GetFiles())
            {
                string fullPath = Path.Combine(_mainInsideEventDataPath, file.Name);
                bool isJsonFile = Path.GetExtension(fullPath) == ".json"; //meta파일과 json파일중 json파일만
                if (!isJsonFile)
                {
                    continue;
                }
                _mainInsideEventDataFileNameList.Add(file.Name);
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

        // 서브 인사이드 이벤트
        bool isSubTitleDirectoryExists = Directory.Exists(_subInsideEventDataPath);
        if (!isSubTitleDirectoryExists)
        {
            Debug.Log("서브 인사이드 이벤트 저장 폴더 존재하지 않음!");
        }
        else
        {
            DirectoryInfo directory = new DirectoryInfo(_subInsideEventDataPath);
            foreach (var file in directory.GetFiles())
            {
                string fullPath = Path.Combine(_subInsideEventDataPath, file.Name);
                bool isJsonFile = Path.GetExtension(fullPath) == ".json"; //meta파일과 json파일중 json파일만
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

        // 다음 이벤트 결정
        switch (eventType)
        {
            case "Main":
                //커서를 이용해 순차적으로 메인이벤트 결정
                eventName = _mainEventDataFileNameList[MainEventCursor];
                path = Path.Combine(_mainEventDataPath, eventName);
                MainEventCursor++;
                TotalEventProgressCount++;
                break;
            case "MainInside":
                // MainInside List의 파일 이름을 순차적으로 검색
                while ((nextEventId + ".json") != eventName)
                {
                    eventName = _mainInsideEventDataFileNameList[searchIndex];
                    searchIndex++;
                }
                path = Path.Combine(_mainInsideEventDataPath, eventName);
                break;
            case "Sub":
                //랜덤
                int eventRandomNumber = Random.Range(0, _subEventDataFileNameList.Count);
                eventName = _subEventDataFileNameList[eventRandomNumber];
                path = Path.Combine(_subEventDataPath, eventName);
                TotalEventProgressCount++;
                break;
            case "SubInside":
                // SubInside List의 파일 이름을 순차적으로 검색
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
        // 파일 읽고 덮어쓰기
        string path = DecideNextEvent(eventType, nextEventId);
        string jsonData = File.ReadAllText(path);
        EventData = JsonUtility.FromJson<EventData>(jsonData);

        //진행상황 데이터 및 플레이어 데이터 저장
        SaveEventProgressData();
    }

}
