using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : SingletonGeneric<EventManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public void LoadNextEvent(string eventType, string nextEventId = "")
    {
        DataManager.Instance.SavePlayerData();
        GameManager.Instance.OptionBoxToggle();
        DataManager.Instance.LoadEventData(eventType, nextEventId);
        GameManager.Instance.Draw();
    }

    public void ShootAShotgun()
    {
        DataManager.Instance.PlayerData.Bullet -= 10;
        AudioManager.Instance.PlaySfx("Shotgun");
        Debug.Log($"³²Àº ÃÑ¾Ë °¹¼ö : {DataManager.Instance.PlayerData.Bullet}");
    }
}
