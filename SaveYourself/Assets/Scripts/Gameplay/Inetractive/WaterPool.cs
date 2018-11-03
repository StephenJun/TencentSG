﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPool : InteractiveObject {

	private void Start()
	{
		OnInteractive += delegate
		{
			ItemUI preTowel = InventoryManager.Instance.inventory.CheckIfHasTypeOfItem<Towel>();
			if (preTowel)
			{
				preTowel.interObj.GetComponent<Towel>().isWet = true;
				preTowel.interObj.GetComponent<Towel>().itemName = "WetTowel";
				preTowel.UpdateIcon();
			}
		};
	}
}