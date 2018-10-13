using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiremanSpawner : MonoBehaviour {

	private void OnEnable()
	{
		StartCoroutine(DisableItself());
	}

	private IEnumerator DisableItself()
	{
		yield return new WaitForSeconds(5.0f);
		gameObject.SetActive(false);
	}

	private void OnTriggerEnter(Collider other)
	{
		PlayerController pc = other.GetComponent<PlayerController>();
		if (pc && pc._playerPara.hp > 0)
		{
			StopAllCoroutines();
			LevelController.Instance.SwitchGameState(GameState.AllClear);
		}
	}
}
