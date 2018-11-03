using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Towel : InteractiveObject {

	[HideInInspector]
	public float defenderProvided;
	public bool isWet = false;

	[SerializeField]
	private float dryDefender = 0.8f;
	[SerializeField]
	private float wetDefender = 0.4f;

	private void Start()
	{
		OnInteractive += delegate
		{
			InventoryManager.Instance.inventory.AddItem(this);
			this.gameObject.SetActive(false);
		};
	}

	private void Update()
	{
		defenderProvided = isWet ? wetDefender : dryDefender;
	}

}
