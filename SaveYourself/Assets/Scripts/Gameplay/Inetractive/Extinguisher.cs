using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CWindow;

public class Extinguisher : InteractiveObject
{
	public Transform jetTrans;
    public bool canBeUsed;
	private float shootTimer = 0;

    private void Start()
    {
        OnInteractive += delegate {

			InventoryManager.Instance.inventory.AddItem(this);
			transform.SetParent(PlayerController.Instance.CarrierTrans, true);
			transform.position = PlayerController.Instance.CarrierTrans.position;
			transform.rotation = PlayerController.Instance.CarrierTrans.rotation;
			//PlayerController.Instance.playerActions.Add(new PlayerAction());
			this.gameObject.SetActive(false);
            ExtinguisherGame ex = UIManager.PopWindow(WindowName.extinguisherGame) as ExtinguisherGame;
            ex.OnPickUp();
            canBeUsed = false;

        };

    }

	public override void OnUsing()
	{
        if (!canBeUsed) return;
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
