using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private float _bgmVolume;
    private float _FXVolume;

    public float BGMVolume
    {
        get
        {
            _bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
            return _bgmVolume;
        }
        set
        {
            _bgmVolume = value;
            PlayerPrefs.SetFloat("BGMVolume", _bgmVolume);
            bgmAudioSource.volume = _bgmVolume;
        }
    }
    public float FXVolume
    {
        get
        {
            _FXVolume = PlayerPrefs.GetFloat("FXVolume", 1.0f);
            return _FXVolume;
        }
        set
        {
            _FXVolume = value;
            PlayerPrefs.SetFloat("FXVolume", _FXVolume);
            FXAudioSource.volume = _FXVolume;
        }
    }


    public void InitVol()
    {
        bgmAudioSource.volume = BGMVolume;
        FXAudioSource.volume = FXVolume;
    }

    public AudioClip menuBGM;
    public AudioClip battleBGM;
    [SerializeField]
    private AudioSource bgmAudioSource;
    [SerializeField]
    private AudioSource FXAudioSource;
    [SerializeField]
    private AudioClip buttonClick;

    private GameManager _GM;

    protected override void Awake()
    {
        base.Awake();
        InitVol();
        _GM = GetComponentInParent<GameManager>();
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayBattleBGM()
    {
        bgmAudioSource.clip = battleBGM;
        bgmAudioSource.loop = true;
        bgmAudioSource.Play();
    }

    public void PlayMenuBGM()
    {
        bgmAudioSource.clip = menuBGM;
        bgmAudioSource.loop = true;
        bgmAudioSource.Play();
    }


    public void PlayButtonClickedAudio()
    {
        FXAudioSource.PlayOneShot(buttonClick);
    }

    public void PlayFXAudioClip(AudioClip clip)
    {
        FXAudioSource.PlayOneShot(clip);
    }

}
