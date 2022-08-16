using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : SingletonGeneric<EventManager>
{
    [Header("----------Image")]
    [SerializeField] private Image _mainImage;

    [Header("----------Text")]
    [SerializeField] private Text _mainScript;
    private List<string> _sentences;
    private bool _isTypingNow;
    private float _typingSpeed = 0.03f;
    private int _maxLetterInOneLine = 30;

    [Header("----------Option")]
    [SerializeField] private GameObject _optionBox;
    [SerializeField] private Button[] _options;
    [SerializeField] private Text[] _optionTexts;
    [SerializeField] private Text[] _optionItemTexts;
    [SerializeField] private Text[] _optionSkillTexts;
    [SerializeField] private Color _itemTextColor;
    [SerializeField] private Color _skillTextColor;
    [SerializeField] private RectTransform optionBoxBackground;

    [Header("----------ScrollView")]
    [SerializeField] private RectTransform _scrollView;
    [SerializeField] private ScrollRect _scrollRect;
    private float _InitScrollViewHeight;
    [SerializeField] private Transform _content; 
    [SerializeField] private Transform _textGroup; 
    [SerializeField] private Transform _imageGroup;
    [SerializeField] private RectTransform _dummyObject;

    [Header("----------Var")]
    private bool _isDrawingNow;
    private float _optionBoxDelayTime = 0.2f;
    private int nextMainCount = 0;
    private int minNextMainCountPlus = 20;
    private int maxNextMainCountPlus = 40;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        _sentences = new List<string>();
        nextMainCount = Random.Range(minNextMainCountPlus, maxNextMainCountPlus);
        _InitScrollViewHeight = _scrollView.rect.height;
    }

    public void LoadNextEvent(string eventType, string nextEventId = "", bool isLoadGame = false)
    {
        DataManager.Instance.LoadEventData(eventType, nextEventId);
        Draw();
        if (isLoadGame)
        {
            ScrollToTop();
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

        string currentEventType = DataManager.Instance.EventData.Type;
        //�� �̺�Ʈ ���۽� ���� �̺�Ʈ�� ���� ������Ʈ�� �����
        if (currentEventType == "Main" || currentEventType == "Sub")
        {
            EventManager.Instance.ClearScrollView();
        }

        //��ũ�Ѻ� ������ ����
        UpdateScrollViewSize();

        //������Ʈ ��������
        string imageName = DataManager.Instance.EventData.ImgName;
        if (imageName != "")
        {
            _mainImage = ObjectManager.Instance.GetImage();
            _mainImage.transform.SetParent(_content);
        }
        _mainScript = ObjectManager.Instance.GetText();
        _mainScript.transform.SetParent(_content);

        //���� ������Ʈ �������� �̵�
        _dummyObject.SetAsLastSibling();

        // ���� ��ũ��Ʈ �ʱ�ȭ �� �ٽ� ���
        _mainScript.text = "";
        _sentences.Clear();
        string[] lines = DataManager.Instance.EventData.Script;
        foreach (string line in lines)
        {
            _sentences.Add(line);
        }

        //�ؽ�Ʈ ������Ʈ ������ ����
        int totalCount = 0;
        for (int i = 0; i < _sentences.Count; i++)
        {
            totalCount += (int)Mathf.Ceil(_sentences[i].Length / (float)29);
        }
        int height = 60 * totalCount;
        _mainScript.rectTransform.sizeDelta = new Vector2(_mainScript.rectTransform.rect.width, height);

        //��ũ�� ��ġ ����
        string eventType = DataManager.Instance.EventData.Type;
        if (eventType == "Main" || eventType == "Sub")
        {
            ScrollToTop();
        }
        else
        {
            ScrollToBottom();
        }

        // Start Draw
        StartCoroutine(DrawRoutine(imageName));
    }

    private IEnumerator DrawRoutine(string imageName)
    {
        // Image
        if (imageName != "")
        {
            byte[] byteTexture = System.IO.File.ReadAllBytes($"Assets/Art/Sprites/{imageName}.png"); //���߿� ��� ������ �ٲ��ֱ�
            if (byteTexture.Length > 0)
            {
                Texture2D texture = new Texture2D(0, 0);
                texture.LoadImage(byteTexture);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                _mainImage.sprite = sprite;
            }
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

            _mainScript.text += "\n";
            letterCount = 0;
        }

        _typingSpeed = 0.03f;
        yield return new WaitForSeconds(_optionBoxDelayTime);
        _isTypingNow = false;
        _isDrawingNow = false;

        //��ư Ȱ��ȭ
        OptionBoxToggle();
        UpdateButton();
    }

    private void UpdateScrollViewSize()
    {
        int optionCount = DataManager.Instance.EventData.OptionCount;
        int number = 0;
        if (optionCount == 1)
        {
            number = 3;
        }
        else if (optionCount == 2)
        {
            number = 2;
        }
        else if (optionCount == 3)
        {
            number = 1;
        }
        else if (optionCount == 4)
        {
            number = 0;
        }
        float height = _InitScrollViewHeight + (150 * number);
        _scrollView.sizeDelta = new Vector2(_scrollView.rect.width, height);
    }

    private void ScrollToTop()
    {
        _scrollRect.normalizedPosition = new Vector2(0, 1);
    }

    private void ScrollToBottom()
    {
        _scrollRect.normalizedPosition = new Vector2(0, 0);
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

    private void UpdateButton()
    {
        EventData eventData = DataManager.Instance.EventData;
        int optionCount = eventData.OptionCount;

        //�ʱ�ȭ
        for (int i = 0; i < 4; i++)
        {
            _options[i].interactable = true;
            _options[i].onClick.RemoveAllListeners();
            _options[i].gameObject.SetActive(false);
        }

        //��ư�� �� �Ҵ� ����
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

            UpdateButtonText(optionButton, i);
            bool isItemOk = CheckItemRequired(optionButton.ItemRequired);
            bool isSkillOk = CheckSkillRequired(optionButton.SkillRequired);
            if (!(isItemOk && isSkillOk))
            {
                _options[i].interactable = false;
                continue;
            }
            AddButtonFunction(optionButton, i);
        }
        UpdateOptionBoxBackgroundSize(optionCount);
    }

    private void AddButtonFunction(Option optionButton, int index)
    {
        string id = optionButton.OptionId;
        string nextId = optionButton.NextEventId;

        // �׼�
        switch (id)
        {
            case "Zombie_3":
                _options[index].onClick.AddListener(() => EventLibraryManager.Instance.ShootAShotgun());
                break;
        }

        // ���� �̺�Ʈ ����
        if (nextId == "") // ���� �̺�Ʈ ����. ���� Ƚ���� ���� ���� ���� Ȥ�� ���� ���� �̺�Ʈ ����
        {
            string eventType = "";
            int totalCount = DataManager.Instance.TotalEventProgressCount;
            if (totalCount == nextMainCount)
            {
                eventType = "Main";
                nextMainCount = Random.Range(totalCount + minNextMainCountPlus, totalCount + maxNextMainCountPlus);
            } 
            else
            {
                eventType = "Sub";
            }
            _options[index].onClick.AddListener(() => LoadNextEvent(eventType));
        }
        else // ���� �̺�Ʈ ����. ���� �̺�Ʈ ����
        {
            string eventType = optionButton.NextEventType;
            _options[index].onClick.AddListener(() => LoadNextEvent(eventType, nextId));
        }
    }

    private bool CheckItemRequired(string[] itemRequired)
    {
        string[] items = itemRequired;
        bool itemExists = false;

        if (items.Length == 0) // ������ �䱸������ ���⶧���� true ��ȯ
        {
            return true;
        }
        else // ������ �䱸������ 1�� �̻��̶��
        {
            for (int i = 0; i < items.Length; i++)
            {
                string name = items[i];
                switch (name) // ���߿� ����Ʈ ���� �˻����� �ٲ� ��
                {
                    case "����":
                        itemExists = DataManager.Instance.PlayerData.CrowBar;
                        break;
                    case "��ź��":
                        itemExists = DataManager.Instance.PlayerData.Shotgun;
                        break;
                    case "���":
                        itemExists = DataManager.Instance.PlayerData.RainCoat;
                        break;
                    case "���":
                        itemExists = DataManager.Instance.PlayerData.Umbrella;
                        break;
                }

                if (!itemExists) // ������ �䱸���� �� 1���� ������ϴٸ� false ��ȯ
                {
                    return false;
                }
                else if (i < items.Length - 1 && itemExists) // ù��° ������ �䱸������ �����ư�, 2��° ������ �䱸������ �˻����� �ʾҴٸ�
                {
                    continue;
                }
            }
            return true; // �ݺ����� ���������� ���ƴٸ�
        }
    }

    private bool CheckSkillRequired(string[] skillRequired)
    {
        string[] skills = skillRequired;
        bool skillExists = false;

        if (skills.Length == 0) // ��� �䱸������ ���⶧���� true ��ȯ
        {
            return true;
        }
        else // ��� �䱸������ 1�� �̻��̶��
        {
            for (int i = 0; i < skills.Length; i++)
            {
                string name = skills[i];
                switch (name) // ���߿� ����Ʈ ���� �˻����� �ٲ� ��
                {
                    case "������":
                        skillExists = DataManager.Instance.PlayerData.Dexterity;
                        break;
                    case "�ٷ�":
                        skillExists = DataManager.Instance.PlayerData.Strength;
                        break;
                    case "��ݼ�":
                        skillExists = DataManager.Instance.PlayerData.ShootingSkill;
                        break;
                }

                if (!skillExists)  // ��� �䱸���� �� 1���� ������ϴٸ� false ��ȯ
                {
                    return false;
                }
                else if (i < skills.Length - 1 && skillExists) // ù��° ��� �䱸������ �����ư�, 2��° ��� �䱸������ �˻����� �ʾҴٸ�
                {
                    continue;
                }
            }
            return true; // �ݺ����� ���������� ���ƴٸ�
        }
    }

    private void UpdateButtonText(Option optionButton, int index)
    {
        Option targetOptionButton = optionButton;
        string[] items = targetOptionButton.ItemRequired;
        string[] skills = targetOptionButton.SkillRequired;

        //Item
        if (items.Length == 2) //������ �䱸���� 2��
        {
            _optionItemTexts[index].text = items[0];
            _optionItemTexts[index].color = _itemTextColor;

            _optionSkillTexts[index].text = items[1];
            _optionSkillTexts[index].color = _itemTextColor;
        }
        //Skill
        else if (skills.Length == 2) //��ų �䱸���� 2��
        {
            _optionItemTexts[index].text = skills[0];
            _optionItemTexts[index].color = _skillTextColor;

            _optionSkillTexts[index].text = skills[1];
            _optionSkillTexts[index].color = _skillTextColor;
        }
        else // �׿� �䱸������ �ϳ����̰ų�, ������ Ȥ�� ��� �䱸���׸� ���� ��
        {
            //Item
            if (items.Length == 0)
            {
                _optionItemTexts[index].text = "";
            }
            else
            {
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

    private void UpdateOptionBoxBackgroundSize(int optionCount)
    {
        int height = 150 * optionCount;
        optionBoxBackground.sizeDelta = new Vector2(optionBoxBackground.rect.width, height);
    }

    public void ClearScrollView()
    {
        Transform[] children = new Transform[_content.childCount];
        for (int i = 0; i < _content.childCount; i++)
        {
            children[i] = _content.GetChild(i);
        }

        for (int i=0; i<children.Length; i++)
        {
            if (children[i].tag == "Script Text")
            {
                children[i].SetParent(_textGroup);
                children[i].gameObject.SetActive(false);
            }
            else if (children[i].tag == "Illust Image")
            {
                children[i].transform.SetParent(_imageGroup);
                children[i].gameObject.SetActive(false);
            }
        }
    }
}
