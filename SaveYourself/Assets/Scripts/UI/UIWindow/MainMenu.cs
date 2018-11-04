using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CWindow;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using DG.Tweening;
public class MainMenu : BaseWindow {

    [SerializeField]Button[] buttons = new Button[3];
    
    public void OnStartButton()
    {
        UIManager.CloseWindow(WindowName.MainMenu, 0);
        UIManager.PopWindow(WindowName.HUD, 0, 0f);
        UIManager.currentWindow = null;
		AudioManager.Instance.PlayUIAudioClip(UIAudioClip.buttonClick1);
        SceneManager.LoadScene("Level01");
        
        //GameManager.Instance.GameStart();
    }

    public void OnSettingOpenButton()
    {
        UIManager.PopWindow(WindowName.SettingMenu);
		AudioManager.Instance.PlayUIAudioClip(UIAudioClip.buttonClick1);
	}

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public override void Close(float time = 0.1F)
    {
        base.Close(time);
		AudioManager.Instance.PlayUIAudioClip(UIAudioClip.buttonClick1);
	}
    protected override void Start()
    {
        base.Start();
        actions[0] = OnStartButton;
        actions[1] = OnSettingOpenButton;
        actions[2] = OnQuitButton;
    }
    int currentSelection = 0;
    Action[] actions = new Action[3];
    public void Update()
    {
        if (isOpen)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                currentSelection--;
                if (currentSelection < 0) currentSelection = 2;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                currentSelection++;
                if (currentSelection > 2) currentSelection = 0;
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                actions[currentSelection]();
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                UIManager.currentWindow.Close();
            }
            foreach (var item in buttons)
            {
                item.targetGraphic.color = Color.white;
            }
            DOTween.To(() => buttons[currentSelection].targetGraphic.color, x => buttons[currentSelection].targetGraphic.color = x, new Color32(180, 180, 180, 255), 0.3f);
        }

    }
}
