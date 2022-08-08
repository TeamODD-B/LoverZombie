using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class GameManager : SingletonGeneric<GameManager>
{
    [Header("----------Image")]
    [SerializeField] private Image _mainImage;

    [Header("----------Text")]
    [SerializeField] private Text _mainScript;
    private List<string> _sentences;
    private bool _isTypingNow;
    [SerializeField] [Range(0.005f, 0.1f)] private float _typingSpeed = 0.03f;
    [SerializeField] private int _maxLetterInOneLine = 29;

    [Header("----------Option")]
    [SerializeField] private GameObject _optionBox;
    [SerializeField] private Button[] _options;
    [SerializeField] private Text[] _optionTexts;
    [SerializeField] private Text[] _optionItemTexts;
    [SerializeField] private Text[] _optionSkillTexts;
    [SerializeField] private Color _itemTextColor;
    [SerializeField] private Color _skillTextColor;

    [Header("----------Panel")]
    [SerializeField] private GameObject _posterPanel;
    [SerializeField] private GameObject _saveLoadPanel;
    [SerializeField] private GameObject _inGamePanel;
    [SerializeField] private Button _loadGameButton;

    [Header("----------Var")]
    private bool _isDrawingNow;
    private float _optionBoxDelayTime = 0.2f;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        _sentences = new List<string>();

        // 세이브 데이터가 없다면 이어하기 버튼 비활성화
        CheckSaveFileExists();
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

    public void IncreaseDrawSpeed() 
    {
        if (_isDrawingNow && _isTypingNow)
        {
            _typingSpeed = 0.005f;
        }
    }

    public void Draw()
    {
        if (_isDrawingNow)
        {
            return;
        }
        _isDrawingNow = true;

        // 메인 스크립트 초기화
        _sentences.Clear();
        _mainScript.text = "";
        string [] lines = DataManager.Instance.EventData.Script;
        foreach (string line in lines)
        {
            _sentences.Add(line);
        }

        // Start Draw
        StartCoroutine(DrawRoutine());
    }

    private IEnumerator DrawRoutine()
    {
        // Image
        string imageName = DataManager.Instance.EventData.ImgName;
        byte[] byteTexture = System.IO.File.ReadAllBytes($"Assets/Art/Sprites/{imageName}.png"); //나중에 경로 변수로 바꿔주기
        if (byteTexture.Length > 0)
        {
            Texture2D texture = new Texture2D(0, 0);
            texture.LoadImage(byteTexture);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            _mainImage.sprite = sprite;
        }

        // Script
        _isTypingNow = true;
        int letterCount = 0;
        for (int i = 0; i < _sentences.Count; i++)
        {
            char[] letters = _sentences[i].ToCharArray(); 
            foreach (char letter in letters)
            {
                letterCount++;
                if (letterCount > _maxLetterInOneLine)
                {
                    _mainScript.text = _mainScript.text + "\n";
                    letterCount = 0;
                }

                _mainScript.text += letter;
                yield return new WaitForSeconds(_typingSpeed);
            }

            letterCount = 0;
            _mainScript.text += "\n\n";
        }
            
        _typingSpeed = 0.03f;
        yield return new WaitForSeconds(_optionBoxDelayTime);
        _isTypingNow = false;    
        _isDrawingNow = false;

        OptionBoxToggle();
        PopUpOptionBox();
    }

    public void OptionBoxToggle()
    {
        if (_isDrawingNow)
        { 
            return;
        }

        bool isOptionBoxActiveNow = _optionBox.activeSelf;
        if (isOptionBoxActiveNow)
        {
            _optionBox.SetActive(false);
        }
        else
        {
            _optionBox.SetActive(true);
        }
    }

    private void PopUpOptionBox()
    {
        EventData eventData = DataManager.Instance.EventData;
        int optionCount = eventData.OptionCount;
        
        //초기화
        for (int i = 0; i < 4; i++)
        {
            _options[i].interactable = true;
            _options[i].onClick.RemoveAllListeners();
            _options[i].gameObject.SetActive(false);
        }

        //버튼에 값 할당 로직
        for (int i = 0; i < optionCount; i++)
        {
            _options[i].gameObject.SetActive(true);
            Option optionButton = null;
            if (i == 0)
            {
                optionButton = eventData.Option1;
            }
            else if (i == 1)
            {
                optionButton = eventData.Option2;
            }
            else if (i == 2)
            {
                optionButton = eventData.Option3;
            }
            else if (i == 3)
            {
                optionButton = eventData.Option4;
            }

            UpdateButton(optionButton, i); 
            bool isItemOk = CheckItemRequired(optionButton.ItemRequired);
            bool isSkillOk = CheckSkillRequired(optionButton.SkillRequired);
            if (!(isItemOk && isSkillOk))
            {
                _options[i].interactable = false;
                continue;
            }
            AddButtonFunction(optionButton.OptionId, optionButton.NextEventId, i);
        }
    }

    private void AddButtonFunction(string optionId, string nextEventId, int index)
    {
        string id = optionId;
        string nextId = nextEventId;
        
        //액션
        switch (id)
        {
            case "Zombie_3":
                _options[index].onClick.AddListener(() => EventManager.Instance.ShootAShotgun());
                break;
        }

        //다음 이벤트 결정
        if (nextId == "") // 지정 이벤트 없음. 바로 랜덤으로 다음 이벤트 재생
        {
            _options[index].onClick.AddListener(() => EventManager.Instance.LoadNextEvent("Sub"));
        }
        else // 지정 이벤트 있음. 지정 이벤트 재생
        {
            _options[index].onClick.AddListener(() => EventManager.Instance.LoadNextEvent("SubTitle", nextId));
        }
    }

    private bool CheckItemRequired(string[] itemRequired)
    {
        string[] items = itemRequired;
        bool itemExists = false;

        if (items.Length == 0) // 아이템 요구조건이 없기때문에 true 반환
        {
            return true;
        }
        else // 아이템 요구조건이 1개 이상이라면
        {
            for (int i = 0; i < items.Length; i++)
            {
                string name = items[i];
                switch (name)
                {
                    case "빠루":
                        itemExists = DataManager.Instance.PlayerData.CrowBar;
                        break;
                    case "산탄총":
                        itemExists = DataManager.Instance.PlayerData.Shotgun;
                        break;
                    case "우비":
                        itemExists = DataManager.Instance.PlayerData.RainCoat;
                        break;
                    case "우산":
                        itemExists = DataManager.Instance.PlayerData.Umbrella;
                        break;
                }

                if (!itemExists) // 아이템 요구사항 중 1개라도 불충분하다면 false 반환
                {
                    return false;
                }
                else if (i < items.Length - 1 && itemExists) // 첫번째 아이템 요구사항이 만족됐고, 2번째 아이템 요구사항을 검사하지 않았다면
                {
                    continue;
                }
            }
            return true; // 반복문을 정상적으로 마쳤다면
        }
    }

    private bool CheckSkillRequired(string[] skillRequired)
    {
        string[] skills = skillRequired;
        bool skillExists = false;

        if (skills.Length == 0) // 기술 요구조건이 없기때문에 true 반환
        {
            return true;
        }
        else // 기술 요구조건이 1개 이상이라면
        {
            for (int i = 0; i < skills.Length; i++)
            {
                string name = skills[i];
                switch (name)
                {
                    case "날렵함":
                        skillExists = DataManager.Instance.PlayerData.Dexterity;
                        break;
                    case "근력":
                        skillExists = DataManager.Instance.PlayerData.Strength;
                        break;
                    case "사격술":
                        skillExists = DataManager.Instance.PlayerData.ShootingSkill;
                        break;
                }

                if (!skillExists)  // 기술 요구사항 중 1개라도 불충분하다면 false 반환
                {
                    return false;
                }
                else if (i < skills.Length - 1 && skillExists) // 첫번째 기술 요구사항이 만족됐고, 2번째 기술 요구사항을 검사하지 않았다면
                {
                    continue;
                }
            }
            return true; // 반복문을 정상적으로 마쳤다면
        }
    }

    private void UpdateButton(Option optionButton, int index)
    {
        Option targetOptionButton = optionButton; 
        string[] items = targetOptionButton.ItemRequired;
        string[] skills = targetOptionButton.SkillRequired;

        //Item
        if (items.Length == 2) //아이템 요구사항 2개
        {
            _optionItemTexts[index].text = items[0];
            _optionItemTexts[index].color = _itemTextColor;

            _optionSkillTexts[index].text = items[1];
            _optionSkillTexts[index].color = _itemTextColor;
        }
        //Skill
        else if (skills.Length == 2) //스킬 요구사항 2개
        {
            _optionItemTexts[index].text = skills[0];
            _optionItemTexts[index].color = _skillTextColor;

            _optionSkillTexts[index].text = skills[1];
            _optionSkillTexts[index].color = _skillTextColor;
        }
        else // 그외 요구사항이 하나씩이거나, 아이템 혹은 기술 요구사항만 있을 때
        {
            //Item
            if (items.Length == 0)
            {
                _optionItemTexts[index].text = "";
            }
            else { 
                _optionItemTexts[index].text = items[0];
                _optionItemTexts[index].color = _itemTextColor;
            }

            //Skill
            if (skills.Length == 0)
            {
                _optionSkillTexts[index].text = "";
            }
            else
            {
                _optionSkillTexts[index].text = skills[0];
                _optionSkillTexts[index].color = _skillTextColor;
            }
        }

        //Button Script
        string buttonScript = targetOptionButton.Text;
        _optionTexts[index].text = buttonScript;
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
        Draw();
    }

    public void StartNewGame()
    {
        DataManager.Instance.SavePlayerData();

        SaveLoadPanelOff();

        DataManager.Instance.LoadEventData("Sub");// 나중에 매개변수를 Main으로 바꿔주기 
        Draw();
    }
}
