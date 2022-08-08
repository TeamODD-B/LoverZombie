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
    [SerializeField] private int health; // ü��
    [SerializeField] private int bullet; // �Ѿ�
    // --- Item
    [SerializeField] private bool crowBar; // ��������(����)
    [SerializeField] private bool shotgun; // ��ź��
    [SerializeField] private bool rainCoat; // ���
    [SerializeField] private bool umbrella; // ���
    //  --- Skill
    [SerializeField] private bool dexterity; // ������
    [SerializeField] private bool strength; // �ٷ�
    [SerializeField] private bool shootingSkill; // ��ݼ�


    // Properties
    #region Life
    public int Health // ü��
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

    public int Bullet // �Ѿ�
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
    public bool CrowBar //��������(����)
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

    public bool Shotgun // ��ź��
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

    public bool RainCoat //���
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

    public bool Umbrella // ���
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
    public bool Dexterity // ������
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

    public bool Strength // �ٷ�
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

    public bool ShootingSkill // ��ݼ�
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
