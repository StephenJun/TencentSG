using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CWindow;

public class Extinguisher : InteractiveObject
{
	public Transform jetTrans;

	private float shootTimer = 0;

    private void Start()
    {
        OnInteractive += delegate {

			InventoryManager.Instance.inventory.AddItem(this);
			transform.SetParent(GameManager.Instance.player.CarrierTrans, true);
			transform.position = GameManager.Instance.player.CarrierTrans.position;
			transform.rotation = GameManager.Instance.player.CarrierTrans.rotation;
			//PlayerController.Instance.playerActions.Add(new PlayerAction());
			this.gameObject.SetActive(false);
        };
    }

	public override void OnUsing()
	{
		shootTimer += Time.deltaTime;
		if(shootTimer > 0.1f)
		{
			GameEffectManager.Instance.AddEffect("ExtinguisherPuff", jetTrans.gameObject, jetTrans.position, 1, 0.5f);
			shootTimer = 0;
		}
		gameObject.SetActive(true);
		base.OnUsing();
	}

}
