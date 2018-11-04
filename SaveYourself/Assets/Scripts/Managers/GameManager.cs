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
    }
    void Update()
    {
        //GetMouseInput();
        //HighLightController();
    }

    void GetMouseInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (player == null)
            {
                Debug.Log("PLayer has not been registed");
                return;
            }
            RaycastHit hitInfo;
            if (Physics.Raycast(viewCamera.ScreenPointToRay(Input.mousePosition), out hitInfo) )
            {
                if (hitInfo.collider.tag == "InteractiveObject")
                {
                    player.MoveTo(hitInfo.collider.GetComponent<InteractiveObject>());
                }
                else
                {
                    player.MoveTo(hitInfo.point);
                    Instantiate(playerMovePointEffect, hitInfo.point, Quaternion.identity);
                }
            }
            Debug.Log(hitInfo.collider.gameObject.name);
            Debug.DrawLine(viewCamera.transform.position,hitInfo.point,Color.red);

            
        }
    }
    #region HighLight
    GameObject lastObject;

    void HighLightController()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(viewCamera.ScreenPointToRay(Input.mousePosition), out hitInfo) && hitInfo.collider.tag == "InteractiveObject")
        {
            if (lastObject != hitInfo.collider.gameObject)
            {
                lastObject = hitInfo.collider.gameObject;
                lastObject.GetComponent<InteractiveObject>().HighlightOn();
            }
        }
        else
        {
            if (lastObject)
            {
                lastObject.GetComponent<InteractiveObject>().HighlightOff();
                lastObject = null;
            }
        }
    }
    #endregion

    public void NextLevel()
    {
        int levelNum = currentLevel + 1;
        string levelName = levelNum.ToString();
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
