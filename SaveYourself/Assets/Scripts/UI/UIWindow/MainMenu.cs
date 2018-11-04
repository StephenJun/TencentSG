using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CWindow;
using UnityEngine.SceneManagement;

public class MainMenu : BaseWindow {

	
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


}
