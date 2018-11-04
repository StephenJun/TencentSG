﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
	public List<AudioClip> gameplayAudioBank = new List<AudioClip>();
	private Dictionary<GamePlayAudioClip, AudioClip> gameplayAudioIndex = new Dictionary<GamePlayAudioClip, AudioClip>();
	public List<AudioClip> UIAudioBank = new List<AudioClip>();
	private Dictionary<UIAudioClip, AudioClip> UIAudioIndex = new Dictionary<UIAudioClip, AudioClip>();


	private float _bgmVolume;
	private float _uiVolume;
	private float _gameplayVolume;
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
	public float UIVolume
	{
		get
		{
			_uiVolume = PlayerPrefs.GetFloat("UIVolume", 1.0f);
			return _uiVolume;
		}
		set
		{
			_uiVolume = value;
			PlayerPrefs.SetFloat("UIVolume", _uiVolume);
			UIAudioSource.volume = _uiVolume;
		}
	}
	public float GameplayVolume
	{
		get
		{
			_gameplayVolume = PlayerPrefs.GetFloat("GameplayVolume", 1.0f);
			return _gameplayVolume;
		}
		set
		{
			_gameplayVolume = value;
			PlayerPrefs.SetFloat("GameplayVolume", _gameplayVolume);
			gameplayAudioSource.volume = _gameplayVolume;
		}
	}


	public void InitAudioClip()
	{
		int i = 0;
		foreach (var gameClipName in Enum.GetValues(typeof(GamePlayAudioClip)))
		{
			if (i == -1) { i++; continue; }
			gameplayAudioIndex.Add((GamePlayAudioClip)gameClipName, gameplayAudioBank[i]);
			i++;
		}
		int j = 0;
		foreach (var uiClipName in Enum.GetValues(typeof(UIAudioClip)))
		{
			if (j == -1) { j++; continue; }
			UIAudioIndex.Add((UIAudioClip)uiClipName, UIAudioBank[j]);
			j++;
		}
	}
	public void InitVol()
	{
		bgmAudioSource.volume = BGMVolume;
		UIAudioSource.volume = UIVolume;
		gameplayAudioSource.volume = GameplayVolume;
	}

	public AudioClip menuBGM;
	public AudioClip battleBGM;
	[SerializeField]
	private AudioSource bgmAudioSource;
	[SerializeField]
	private AudioSource UIAudioSource;
	[SerializeField]
	private AudioSource gameplayAudioSource;


	void Start()
	{
		//InitVol();
		InitAudioClip();
	}

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

	public void PlayUIAudioClip(UIAudioClip clip)
	{
		UIAudioSource.PlayOneShot(UIAudioIndex[clip]);
	}

	public void PlayGameplayAudioClip(GamePlayAudioClip clip)
	{
		gameplayAudioSource.PlayOneShot(gameplayAudioIndex[clip]);
		print(1);
	}

	public void PlayGameplayAudioClip(AudioClip clip)
	{
		gameplayAudioSource.PlayOneShot(clip);
	}
}

public enum GamePlayAudioClip
{
	playerSpawnClip,
	playerDodgeClip,
	playerHurtClip,
	playerDeathClip,
	switchWeaponClip,
	DiscardWeaponClip,
	PickupItemClip,
	ChestSpawnClip,
	EnemyHurtClip,
	EnemyDeathClip,
	EnemyAttackClip
}

public enum UIAudioClip
{
	buttonClick1,
	buttonClick2,
	buttonClick3
}
