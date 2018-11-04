using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CWindow;

public class LevelController : Singleton<LevelController> {

	[HideInInspector]
	public GameState gameState;

	private Text timerTxt;
	[SerializeField]
	private float TimeOfEacapeState = 60.0F;
	[SerializeField]
	private float timingOfFiremanAppear = 55.0F;
	[SerializeField]
	private GameObject fireman1;
	[SerializeField]
	private GameObject fireman2;

	private bool hasFiremanAppeared;
    private Transform fireSpawnerRoot;
    private List<Transform> fireSpawners = new List<Transform>();

    private void Start()
    {
		timerTxt = GameManager.Instance.timer.transform.Find("Text_Timeleft").GetComponent<Text>();
        //fireSpawnerRoot = GameObject.Find("FireSpawnerRoot").transform;
        //for (int i = 0; i < fireSpawnerRoot.childCount; i++)
        //{
        //    fireSpawners.Add(fireSpawnerRoot.GetChild(i));
        //}
		GameStart();
    }


	private void SpawnFireEffect()
	{
		GameEffectManager.Instance.AddWorldEffect("FlyingFlame",fireSpawnerRoot.position, 1, -1);
		foreach (var spwaner in fireSpawners)
		{
			GameEffectManager.Instance.AddWorldEffect("Fire", spwaner.position, 1, -1);
			GameEffectManager.Instance.AddWorldEffect("VFX_Smoke", spwaner.position, Random.Range(0.5f, 4.0f), -1);
		}
	}

	#region GameStateHandler
	public void GameStart()
	{
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
		PlayerController.Instance.expression.ShowExpression(ExpressionType.Music, 3.0f);
		yield return null;
	}

	private IEnumerator StartEscapeState()
	{
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
		InputManager.Instance.canControl = false;
		StopCoroutine("StartEscapeState");
		BaseWindow popup = UIManager.PopWindow(WindowName.GenericPopup, "You survived!! ^_^");
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
		InputManager.Instance.canControl = false;
        StopCoroutine("StartEscapeState");
		BaseWindow popup = UIManager.PopWindow(WindowName.GenericPopup, "You Died");
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
