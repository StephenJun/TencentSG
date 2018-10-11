using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> {

    public GameState gameState;

    [SerializeField]
    private GameObject timer;
    [SerializeField]
    private Text timerTxt;
    [SerializeField]
    private float TimeOfEacapeState = 60.0F;

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
        GetMouseInput();
        HighLightController();
    }

    #region GameStateHandler
    public void GameStart()
    {
        SwitchGameState(GameState.EscapeState);
    }

    public void SwitchGameState(GameState newGameState)
    {
        gameState = newGameState;
        switch (gameState)
        {
            case GameState.SearchState:
                break;
            case GameState.EscapeState:
                StartCoroutine(StartEscapeState());
                break;
            case GameState.AllClear:
                break;
            case GameState.GameOver:
                break;
        }
    }

    private IEnumerator StartEscapeState()
    {
        timer.SetActive(true);
        float initTime = TimeOfEacapeState;
        while(initTime > 0)
        {
            yield return null;
            initTime -= Time.deltaTime;
            timerTxt.text = "TimeLeft: " + (initTime / 60).ToString("00") + " : " + (initTime % 60).ToString("00");
        }
    }

    private IEnumerator StartAllClearState()
    {
        StopCoroutine("StartEscapeState");
        yield return null;
    }
    #endregion

    void GetMouseInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (player == null)
            {
                Debug.Log("PLayer has not been registered");
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
}


public enum GameState
{
    SearchState,
    EscapeState,
    GameOver,
    AllClear
}
