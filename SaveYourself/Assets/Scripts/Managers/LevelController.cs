using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

	private IEnumerator StartEscapeState()
	{
		//SpawnFireEffect();
		GameManager.Instance.timer.SetActive(true);
		float initTime = TimeOfEacapeState;
		while (initTime > 0)
		{
			yield return null;
			initTime -= Time.deltaTime;
			float minutes = initTime / 60;
			float seconds = initTime - minutes * 60;
			timerTxt.text = (minutes).ToString("00") + " : " + (initTime % 60).ToString("00");
			GameManager.Instance.player.DamageReceiver(1 * Time.deltaTime * (1 - InventoryManager.Instance.inventory.TotalDefender / 100));
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
		UIManager.PopWindow(WindowName.GenericPopup, "You survived!! ^_^").confirm += delegate
		{
			//GameVictory
		};
		yield return null;
	}

	private IEnumerator StartGameOver()
	{
		StopCoroutine("StartEscapeState");
		UIManager.PopWindow(WindowName.GenericPopup, "You Died").confirm += delegate
		{
			//Gameover
		};
		yield return null;
	}
	#endregion

}
