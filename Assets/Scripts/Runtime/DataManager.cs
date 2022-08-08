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
        _subEventDataPath = Path.Combine(Application.dataPath, "Presets/Event/Sub"); //모바일 빌드시 persistentDataPath로 변경할 것
        _subTitleEventDataPath = Path.Combine(Application.dataPath, "Presets/Event/Sub/SubTitle"); //모바일 빌드시 persistentDataPath로 변경할 것
    }

    private void LoadEventJsonFileNames()
    {
        _subEventDataFileNameList = new List<string>();
        _subTitleEventDataFileNameList = new List<string>();

        #region 모바일 빌드 시 앞으로 추가할 내용 
        //1. Json파일들이 저장되는 폴더가 존재하는지 검사하고 없다면 만들어줘야함.
        //2. 폴더 안에 Json 파일들을 작성해줘야함. 에셋번들 즉, 패치를 다운받아야 한다.
        //2-1. 무료 서버 찾아서 이용해보자. 
        #endregion
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

        // 서브 타이틀 이벤트
        bool isSubTitleDirectoryExists = Directory.Exists(_subTitleEventDataPath);
        if (!isSubTitleDirectoryExists)
        {
            Debug.Log("서브 타이틀 이벤트 저장 폴더 존재하지 않음!");
        }
        else
        {
            DirectoryInfo directory = new DirectoryInfo(_subTitleEventDataPath);
            foreach (var file in directory.GetFiles())
            {
                string fullPath = Path.Combine(_subTitleEventDataPath, file.Name);
                bool isJsonFile = Path.GetExtension(fullPath) == ".json"; //meta파일과 json파일중 json파일만
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
        // 다음 이벤트 결정
        switch (type)
        {
            case "Main":
                // 여기에 메인 이벤트 함수 호출 로직 작성
                break;
            case "Sub":
                // 다음 이벤트 랜덤으로 결정
                int eventRandomNumber = Random.Range(0, _subEventDataFileNameList.Count);
                eventName = _subEventDataFileNameList[eventRandomNumber];
                path = Path.Combine(_subEventDataPath, eventName);
                break;
            case "SubTitle":
                // SubTitle List의 파일 이름을 순차적으로 검색
                int jsonSearchIndex = 0;
                while ((nextId + ".json") != eventName)
                {
                    eventName = _subTitleEventDataFileNameList[jsonSearchIndex];
                    jsonSearchIndex++;
                }
                path = Path.Combine(_subTitleEventDataPath, eventName);
                break;
        }

        // 파일 읽고 덮어쓰기
        string jsonData = File.ReadAllText(path);
        EventData = JsonUtility.FromJson<EventData>(jsonData);
    }
}
