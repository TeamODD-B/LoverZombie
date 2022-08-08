using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public void Init()
    {
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
    [SerializeField] private int health; // Ã¼·Â
    [SerializeField] private int bullet; // ÃÑ¾Ë
    // --- Item
    [SerializeField] private bool crowBar; // ¼èÁö·¿´ë(ºü·ç)
    [SerializeField] private bool shotgun; // »êÅºÃÑ
    [SerializeField] private bool rainCoat; // ¿ìºñ
    [SerializeField] private bool umbrella; // ¿ì»ê
    //  --- Skill
    [SerializeField] private bool dexterity; // ³¯·ÆÇÔ
    [SerializeField] private bool strength; // ±Ù·Â
    [SerializeField] private bool shootingSkill; // »ç°Ý¼ú


    // Properties
    #region Life
    public int Health // Ã¼·Â
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

    public int Bullet // ÃÑ¾Ë
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
    public bool CrowBar //¼èÁö·¿´ë(ºü·ç)
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

    public bool Shotgun // »êÅºÃÑ
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

    public bool RainCoat //¿ìºñ
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

    public bool Umbrella // ¿ì»ê
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
    public bool Dexterity // ³¯·ÆÇÔ
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

    public bool Strength // ±Ù·Â
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

    public bool ShootingSkill // »ç°Ý¼ú
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
