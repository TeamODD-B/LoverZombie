using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventLibraryManager : SingletonGeneric<EventLibraryManager>
{
    [SerializeField] private Image _bulletMark;
    private float bulletMarkFadeOutTime = 2f;

    protected override void Awake()
    {
        base.Awake();
    }

    public void DecreaseBullet(int bulletCount)
    {
        DataManager.Instance.PlayerData.Bullet -= bulletCount;
    }

    public void IncreaseBullet(int bulletCount)
    {
        DataManager.Instance.PlayerData.Bullet += bulletCount;
    }

    public void PlaySfx(string type)
    {
        AudioManager.Instance.PlaySfx(type);
    }

    public void ShootAShotgun()
    {
        DecreaseBullet(10);
        PlaySfx("Shotgun");
        StartCoroutine(bulletMarkFadeInOut());
        Debug.Log($"남은 총알 갯수 : {DataManager.Instance.PlayerData.Bullet}");
    }

    private IEnumerator bulletMarkFadeInOut()
    {
        Color fadeInColor = new Color(1, 1, 1, 1);
        Color fadeOutColor = new Color(1, 1, 1, 0);

        _bulletMark.color = fadeInColor;
        yield return new WaitForSeconds(bulletMarkFadeOutTime);

        while (_bulletMark.color.a > 0)
        {
            _bulletMark.color = Color.Lerp(_bulletMark.color, fadeOutColor, 0.05f);
            yield return null;
        }
    }
}
