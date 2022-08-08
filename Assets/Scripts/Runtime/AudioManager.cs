using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonGeneric<AudioManager>
{
    //Audio Source
    [SerializeField] private AudioSource _backgroundPlayer;
    [SerializeField] private AudioSource [] _sfxPlayers;

    //Audio Clip
    public AudioClip ShotgunClip;

    //Var
    private int _sfxCursor = 0;
    
    protected override void Awake()
    {
        base.Awake();
    }
    
    public void PlaySfx(string action)
    {
        // _sfxCursor을 정하는 계산식
        _sfxCursor = (_sfxCursor + 1) % _sfxPlayers.Length;
        
        // 클립 교체
        switch (action)
        {
            case "Shotgun":
                _sfxPlayers[_sfxCursor].clip = ShotgunClip;
                break;
        }

        // 재생
        _sfxPlayers[_sfxCursor].Play();
    }
}
