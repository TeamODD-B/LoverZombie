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
        // _sfxCursor�� ���ϴ� ����
        _sfxCursor = (_sfxCursor + 1) % _sfxPlayers.Length;
        
        // Ŭ�� ��ü
        switch (action)
        {
            case "Shotgun":
                _sfxPlayers[_sfxCursor].clip = ShotgunClip;
                break;
        }

        // ���
        _sfxPlayers[_sfxCursor].Play();
    }
}
