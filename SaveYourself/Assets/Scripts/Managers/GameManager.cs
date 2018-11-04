using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

	public GameObject timer;
    public int currentLevel = 1;
    [HideInInspector]
    public Camera viewCamera;
    Vector3 currentTargetPositon;

    public PlayerController player;
    [SerializeField]
    GameObject playerMovePointEffect;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.transform.root.gameObject);
    }

    void Start () {
        UIManager.PopWindow(CWindow.WindowName.MainMenu, 0, 0f);
		AudioManager.Instance.PlayMenuBGM();
    }
    void Update()
    {
        //GetMouseInput();
        //HighLightController();
    }

    public void NextLevel()
    {
        Start();
        int levelNum = currentLevel + 1;
        string levelName = levelNum.ToString();
        if (levelNum == 2)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }
        if (currentLevel < 10)
        {
            levelName = "0" + levelName;
        }
        SceneManager.LoadScene("Level" + levelName);
    }
    public void RestartLevel()
    {
        int levelNum = currentLevel;
        string levelName = levelNum.ToString();
        if (currentLevel < 10)
        {
            levelName = "0" + levelName;
        }
        SceneManager.LoadScene("Level" + levelName);
    }
}


public enum GameState
{
    SearchState,
    EscapeState,
    GameOver,
    AllClear
}
