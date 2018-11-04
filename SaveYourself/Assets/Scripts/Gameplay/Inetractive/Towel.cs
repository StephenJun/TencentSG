using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Towel : InteractiveObject {

	[HideInInspector]
	public float defenderProvided;

	public float dryDefender = 0.6f;
	public float wetDefender = 0.3f;

	private void Start()
	{
		OnInteractive += delegate
		{
			InventoryManager.Instance.inventory.AddItem(this);
			this.gameObject.SetActive(false);
		};
	}

}
