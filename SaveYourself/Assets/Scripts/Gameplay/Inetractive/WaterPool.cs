using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPool : InteractiveObject {

	private void Start()
	{
		OnInteractive += delegate
		{
			AudioManager.Instance.PlayGameplayAudioClip(GamePlayAudioClip.Water);
			Towel preTowel = InventoryManager.Instance.inventory.CheckIfHasTypeOfItem<Towel>().interObj as Towel;
			if (preTowel)
			{
				preTowel.defenderProvided = preTowel.wetDefender;
				preTowel.itemName = "WetTowel";
				InventoryManager.Instance.inventory.CheckIfHasTypeOfItem<Towel>().UpdateIcon();
			}
		};
	}
}
