using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Towel : InteractiveObject {

	private void Start()
	{
		OnInteractive += delegate {
			InventoryManager.Instance.inventory.AddItem(this);
			this.gameObject.SetActive(false);
		};

	}
}
