using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public void Init()
    {
        //Record
        LastEventId = "";
        LastEventType = "";
        TotalEventProgressCount = 0;
        MainEventCursor = 0;
        // Life
        Health = 100;
        Bullet = 50;
        // Item
        CrowBar = false;
        Shotgun = true;
        RainCoat = true;
        Umbrella = false;
        // Skill
        Strength = false;
        Dexterity = false;
        ShootingSkill = true;
    }

    // Fields
    public int[] skill;
    // --- Record Event Progress Data
    [SerializeField] private string lastEventType;
    [SerializeField] private string lastEventId;
    [SerializeField] private int totalEventProgressCount;
    [SerializeField] private int mainEventCursor;
    // --- Record Settings Data
    [SerializeField] private float backgroundVolume;
    [SerializeField] private float effectVolume;
    // --- Life
    [SerializeField] private int health; // 체력
    [SerializeField] private int bullet; // 총알
    // --- Item
    [SerializeField] private bool crowBar; // 쇠지렛대(빠루)
    [SerializeField] private bool shotgun; // 산탄총
    [SerializeField] private bool rainCoat; // 우비
    [SerializeField] private bool umbrella; // 우산
    //  --- Skill
    [SerializeField] private bool dexterity; // 날렵함
    [SerializeField] private bool strength; // 근력
    [SerializeField] private bool shootingSkill; // 사격술

    // Properties
    #region Record Event Progress Data
    public string LastEventType
    {
        get
        {
            return lastEventType;
        }
        set
        {
            lastEventType = value;
        }
    }
    public int TotalEventProgressCount
    {
        get
        {
            return totalEventProgressCount;
        }
        set
        {
            totalEventProgressCount = value;
        }
    }
    public int MainEventCursor
    {
        get
        {
            return mainEventCursor;
        }
        set
        {
            mainEventCursor = value;
        }
    }

    public string LastEventId
    {
        get
        {
            return lastEventId;
        }
        set
        {
            lastEventId = value;
        }
    }

    #endregion

    #region Settings Data
    public float BackgroundVolume
    {
        get
        {
            return backgroundVolume;
        }
        set
        {
            backgroundVolume = value;
        }
    }

    public float EffectVolume
    {
        get
        {
            return effectVolume;
        }
        set
        {
            effectVolume = value;
        }
    }
    #endregion

    #region Life
    public int Health // 체력
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

    public int Bullet // 총알
    {
        get
        {
            return bullet;
        }
        set
        {
            bullet = value;
        }
    }
    #endregion

    #region Item
    public bool CrowBar //쇠지렛대(빠루)
    {
        get
        {
            return crowBar;
        }
        set
        {
            crowBar = value;
        }
    }

    public bool Shotgun // 산탄총
    {
        get
        {
            return shotgun;
        }
        set
        {
            shotgun = value;
        }
    }

    public bool RainCoat //우비
    {
        get
        {
            return rainCoat;
        }
        set
        {
            rainCoat = value;
        }
    }

    public bool Umbrella // 우산
    {
        get
        {
            return umbrella;
        }
        set
        {
            umbrella = value;
        }
    }
    #endregion

    #region Skill
    public bool Dexterity // 날렵함
    {
        get
        {
            return dexterity;
        }
        set
        {
            dexterity = value;
        }
    }

    public bool Strength // 근력
    {
        get
        {
            return strength;
        }
        set
        {
            strength = value;
        }
    }

    public bool ShootingSkill // 사격술
    {
        get
        {
            return shootingSkill;
        }
        set
        {
            shootingSkill = value;
        }
    }
    #endregion
}
