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
    [SerializeField] private GameObject _settingsPanel;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        CheckSaveFileExists(); // ���̺� �����Ͱ� ���ٸ� �̾��ϱ� ��ư ��Ȱ��ȭ
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

        AudioManager.Instance.LoadAudioData();
        AudioManager.Instance.PlayBackground();


        string lastEventType = DataManager.Instance.PlayerData.LastEventType;
        string lastEventId = DataManager.Instance.PlayerData.LastEventId;
        EventManager.Instance.LoadNextEvent(lastEventType, lastEventId, true);
    }

    public void StartNewGame()
    {
        DataManager.Instance.SavePlayerData();

        SaveLoadPanelOff();

        AudioManager.Instance.VolumeInitSetting();
        AudioManager.Instance.PlayBackground();

        EventManager.Instance.LoadNextEvent("Main");
    }

    public void SettingsPanelToggle()
    {
        bool isSettingsPanelOn = _settingsPanel.activeSelf;
        if (isSettingsPanelOn)
        {
            _settingsPanel.SetActive(false);
        }
        else 
        {
            _settingsPanel.SetActive(true);
        }
    }
}
