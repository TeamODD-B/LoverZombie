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

        DataManager.Instance.LoadEventData("Sub");// ���߿� ���������� �ϴ� �̺�Ʈ ������ �Ű������� �ٲ��ֱ� 
        EventManager.Instance.Draw();
    }

    public void StartNewGame()
    {
        DataManager.Instance.SavePlayerData();

        SaveLoadPanelOff();

        DataManager.Instance.LoadEventData("Sub");// ���߿� �Ű������� Main���� �ٲ��ֱ� 
        EventManager.Instance.Draw();
    }
}
