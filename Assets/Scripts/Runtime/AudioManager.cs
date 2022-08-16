using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class AudioManager : SingletonGeneric<AudioManager>
{
    //Audio Source
    [SerializeField] private AudioSource _backgroundPlayer;
    [SerializeField] private AudioSource [] _sfxPlayers;

    //Audio Setting Slider
    [SerializeField] private Slider backgroundSlider;
    [SerializeField] private Slider EffectSlider;

    //Audio Clip
    public AudioClip ShotgunClip;

    //Var
    private int _sfxCursor = 0;
    private float _initBackgroundVolume = 1f;
    private float _initEffectVolume = 1f;
    
    protected override void Awake()
    {
        base.Awake();
    }
    public void VolumeInitSetting()
    {
        PlayerData playerData = DataManager.Instance.PlayerData;
        //Record
        playerData.BackgroundVolume = _initBackgroundVolume;
        playerData.EffectVolume = _initEffectVolume;

        //Volume
        _backgroundPlayer.volume = _initBackgroundVolume;
        foreach (AudioSource sfxPlayer in _sfxPlayers)
        {
            sfxPlayer.volume = _initEffectVolume;
        }

        //Slider Value
        backgroundSlider.value = _initBackgroundVolume;
        EffectSlider.value = _initEffectVolume;
    }

    public void SaveAudioData()
    {
        DataManager.Instance.SavePlayerData();
        GameManager.Instance.SettingsPanelToggle();
    }

    public void ResetAudioData()
    {
        _backgroundPlayer.volume = _initBackgroundVolume;
        foreach (AudioSource sfxPlayer in _sfxPlayers)
        {
            sfxPlayer.volume = _initEffectVolume;
        }

        backgroundSlider.value = _initBackgroundVolume;
        EffectSlider.value = _initEffectVolume;
    }

    public void LoadAudioData()
    {
        PlayerData playerData = DataManager.Instance.PlayerData;

        //Volume
        _backgroundPlayer.volume = playerData.BackgroundVolume;
        foreach (AudioSource sfxPlayer in _sfxPlayers)
        {
            sfxPlayer.volume = playerData.EffectVolume;
        }

        //Slider Value
        backgroundSlider.value = playerData.BackgroundVolume;
        EffectSlider.value = playerData.EffectVolume;
    }

    public void UpdateBackgroundVolume(float value) 
    {
        _backgroundPlayer.volume = value;
        DataManager.Instance.PlayerData.BackgroundVolume = value;
    }

    public void UpdateEffectVolume(float value)
    {
        foreach (AudioSource sfxPlayer in _sfxPlayers)
        {
            sfxPlayer.volume = value;
        }
        DataManager.Instance.PlayerData.EffectVolume = value;
    }

    public void PlayBackground()
    {
        _backgroundPlayer.Play();
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
