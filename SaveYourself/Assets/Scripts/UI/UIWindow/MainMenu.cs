﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CWindow;
using UnityEngine.SceneManagement;

public class MainMenu : BaseWindow {

	
    public void OnStartButton()
    {
        UIManager.CloseWindow(WindowName.MainMenu, 0);
        UIManager.PopWindow(WindowName.HUD, 0, 0f);
        AudioManager.Instance.PlayButtonClickedAudio();
        SceneManager.LoadScene("MainGame");
        //GameManager.Instance.GameStart();
    }

    public void OnSettingOpenButton()
    {
        UIManager.PopWindow(WindowName.SettingMenu);
        AudioManager.Instance.PlayButtonClickedAudio();
    }


    public override void Close(float time = 0.1F)
    {
        base.Close(time);
        AudioManager.Instance.PlayButtonClickedAudio();
    }


}
