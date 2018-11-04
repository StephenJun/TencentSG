using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBox : InteractiveObject {

	private void Start()
	{
		OnInteractive += delegate {
			InventoryManager.Instance.inventory.AddItem(this);
			AudioManager.Instance.PlayGameplayAudioClip(GamePlayAudioClip.GetMoney);
		};

	}
}
