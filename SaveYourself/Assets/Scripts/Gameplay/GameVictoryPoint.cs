using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameVictoryPoint : MonoBehaviour {

	public Action victory;

	[SerializeField]
	private float timerNeedToPass = 5.0F;

	private void OnTriggerEnter(Collider other)
	{
		PlayerController pc = other.GetComponent<PlayerController>();
		if (pc && LevelController.Instance.gameState == GameState.EscapeState)
		{
			victory += delegate
			{
				LevelController.Instance.SwitchGameState(GameState.AllClear);
			};
			InventoryManager.Instance.inventory.ShowTimerWhenInteracting(timerNeedToPass, victory, this.transform);
		}
	}
}
