using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CWindow;

public class Extinguisher : InteractiveObject
{
    private void Start()
    {
        OnInteractive += delegate {

			InventoryManager.Instance.inventory.AddItem(this);
			//PlayerController.Instance.playerActions.Add(new PlayerAction());
			this.gameObject.SetActive(false);
        };

    }

}
