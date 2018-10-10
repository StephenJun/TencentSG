using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CWindow;
using UnityEngine.SceneManagement;

public class MainMenu : BaseWindow {

	
    public void OnStartButton()
    {
        UIManager.PopWindow(WindowName.HUD, 0, 0f);
        UIManager.CloseWindow(WindowName.MainMenu);
        SceneManager.LoadScene("MainGame");
    }

}
