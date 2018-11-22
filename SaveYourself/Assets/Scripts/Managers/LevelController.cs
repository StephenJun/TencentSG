using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CWindow;

public class LevelController : Singleton<LevelController> {

	[HideInInspector]
	public GameState gameState;
	[HideInInspector]
	public int startTime;
	public int levelIndex = 0;
	private Text timerTxt;
	[SerializeField]
	private float TimeOfEacapeState = 60.0F;
	[SerializeField]
	private float timingOfFiremanAppear = 55.0F;
	[SerializeField]
	private GameObject fireman1;
	[SerializeField]
	private GameObject fireman2;

	[SerializeField]
	private GameObject[] doors;
	[SerializeField]
	private GameObject[] beforeAndAfter;

	private bool hasFiremanAppeared;
    private Transform fireSpawnerRoot;
    private List<Transform> fireSpawners = new List<Transform>();
    public LevelData levelData;
    private void Start()
    {
		timerTxt = GameManager.Instance.timer.transform.Find("Text_Timeleft").GetComponent<Text>();
        //fireSpawnerRoot = GameObject.Find("FireSpawnerRoot").transform;
        //for (int i = 0; i < fireSpawnerRoot.childCount; i++)
        //{
        //    fireSpawners.Add(fireSpawnerRoot.GetChild(i));
        //}
        InitLevel();
    }
	public void SmartDoorHandler(int time)
	{
		foreach (var door in doors)
		{
			door.SetActive(false);
		}
		if(time <= 6)
		{
			doors[0].SetActive(true);
		}else if (time <= 12)
		{
			doors[1].SetActive(true);
		}else if (time <= 18)
		{
			doors[2].SetActive(true);
		}
		else
		{
			doors[3].SetActive(true);
		}
	}


	#region GameStateHandler
	public void InitLevel()
	{
        JsonHandler.LoadLevelData(ref levelData, levelIndex);
        PlaybackManager.Instance.OnLevelInit(levelIndex);
        InventoryManager.Instance.inventory.Initialization();
		UIManager.PopWindow(WindowName.HUD, 0, 0f);
		UIManager.currentWindow = null;
		MapManager.Instance.UseMap(levelIndex);
		GameManager.Instance.currentLevel = levelIndex;
		SwitchGameState(GameState.SearchState);
	}

	public void SwitchGameState(GameState newGameState)
	{
		gameState = newGameState;
		switch (gameState)
		{
			case GameState.SearchState:
				StartCoroutine(StartSearchState());
				break;
			case GameState.EscapeState:
				StartCoroutine(StartEscapeState());
				break;
			case GameState.AllClear:
				StartCoroutine(StartAllClearState());
				break;
			case GameState.GameOver:
				StartCoroutine(StartGameOver());
				break;
		}
	}

	private IEnumerator StartSearchState()
	{
		InputManager.Instance.canControl = true;
		GameManager.Instance.timer.SetActive(false);
		AudioManager.Instance.PlayExploreBGM();
		PlaybackManager.Instance.PushPose(PoseType.Watch);
		PlayerController.Instance.expression.ShowExpression(ExpressionType.Music, 3.0f);
		yield return null;
	}

	private IEnumerator StartEscapeState()
	{
		if (levelIndex == 1)
		{
			SmartDoorHandler(startTime);
		}
		if (beforeAndAfter.Length > 0)
		{
			beforeAndAfter[0].SetActive(false);
			beforeAndAfter[1].SetActive(true);
		}
		FloorManager.Instance.GameStart();
		GameManager.Instance.timer.SetActive(true);
		AudioManager.Instance.PlayEscapeBGM();
		CameraController.Instance.SetSmokeShaderActive(true);
		CameraController.Instance.SetRGBShaderActive(Color.white, 0.5f);
		PlayerController.Instance.expression.ShowExpression(ExpressionType.Shock, 1.0f);
		InputManager.Instance.canControl = true;
		float initTime = TimeOfEacapeState;
		while (initTime > 0)
		{
			yield return null;
			initTime -= Time.deltaTime;
			float seconds = initTime % 60;
			float minutes = (initTime - seconds) / 60;
			timerTxt.text = (minutes).ToString("00") + " : " + (initTime % 60).ToString("00");
			PlayerController.Instance.DamageReceiver(1f * Time.deltaTime);
			if(initTime < timingOfFiremanAppear && !hasFiremanAppeared)
			{
				fireman1.SetActive(true);
				fireman2.SetActive(true);
				hasFiremanAppeared = true;
			}
		}
		SwitchGameState(GameState.GameOver);
	}

    private IEnumerator StartAllClearState()
    {
		StopCoroutine("StartEscapeState");
		AudioManager.Instance.PlayGameplayAudioClip(GamePlayAudioClip.Success);
		InputManager.Instance.canControl = false;
		BaseWindow popup = UIManager.PopWindow(WindowName.GenericPopup, "You survived!! ^_^");
        PlaybackManager.Instance.CheckPoseTypes();
        popup.GetComponent<GenericPopup>().Init(false, "win");
		popup.confirm += delegate
		{
			UIManager.CloseWindow(WindowName.HUD, 0f);
			SceneManager.LoadSceneAsync("Gym").completed += LevelController_completed;
		};
        yield return null;
    }

    private IEnumerator StartGameOver()
    {
		StopCoroutine("StartEscapeState");
		AudioManager.Instance.PlayGameplayAudioClip(GamePlayAudioClip.Fail);
		InputManager.Instance.canControl = false;
		BaseWindow popup = UIManager.PopWindow(WindowName.GenericPopup, "You Died");
        PlaybackManager.Instance.CheckPoseTypes();
        popup.GetComponent<GenericPopup>().Init(false, "fail");
		popup.confirm += delegate
        {
			UIManager.CloseWindow(WindowName.HUD, 0f);
            SceneManager.LoadSceneAsync("Gym").completed += LevelController_completed;
        };
        yield return null;
    }

    private void LevelController_completed(AsyncOperation obj)
    {
        GameEffectManager.Instance.Init();
        PlaybackManager.Instance.StartPlayback();
		AudioManager.Instance.PlayPlaybackBGM();
    }
    #endregion

}
