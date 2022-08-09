using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class GameManager : SingletonGeneric<GameManager>
{
    [Header("----------Panel")]
    [SerializeField] private GameObject _posterPanel;
    [SerializeField] private GameObject _saveLoadPanel;
    [SerializeField] private GameObject _inGamePanel;
    [SerializeField] private Button _loadGameButton;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        CheckSaveFileExists(); // 세이브 데이터가 없다면 이어하기 버튼 비활성화
    }

    private void CheckSaveFileExists()
    {
        string path = DataManager.Instance.PlayerDataPath;
        FileInfo fileInfo = new FileInfo(path);
        bool isFileExists = fileInfo.Exists;
        if (!isFileExists)
        {
            _loadGameButton.interactable = false;
        }
    }

    public void PosterOff()
    {
        _posterPanel.SetActive(false);
        _saveLoadPanel.SetActive(true);
    }

    private void SaveLoadPanelOff()
    {
        _saveLoadPanel.SetActive(false);
        _inGamePanel.SetActive(true);
    }

    public void StartLoadGame()
    {
        DataManager.Instance.LoadPlayerData();

        SaveLoadPanelOff();

        DataManager.Instance.LoadEventData("Sub");// 나중에 마지막으로 하던 이벤트 유형의 매개변수로 바꿔주기 
        EventManager.Instance.Draw();
    }

    public void StartNewGame()
    {
        DataManager.Instance.SavePlayerData();

        SaveLoadPanelOff();

        DataManager.Instance.LoadEventData("Sub");// 나중에 매개변수를 Main으로 바꿔주기 
        EventManager.Instance.Draw();
    }
}
