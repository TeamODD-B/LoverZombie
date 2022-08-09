using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventLibraryManager :  SingletonGeneric<EventLibraryManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public void ShootAShotgun()
    {
        DataManager.Instance.PlayerData.Bullet -= 10;
        AudioManager.Instance.PlaySfx("Shotgun");
        Debug.Log($"���� �Ѿ� ���� : {DataManager.Instance.PlayerData.Bullet}");
    }
}
