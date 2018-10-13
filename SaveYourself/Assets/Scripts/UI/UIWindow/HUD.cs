using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CWindow;

public class HUD : BaseWindow {

	public Image playerHpBar;
	public Text playerHpText;

	private void Update()
	{
		if (PlayerController.Instance)
		{
			playerHpBar.fillAmount = PlayerController.Instance._playerPara.hp / 100;
			playerHpText.text = PlayerController.Instance._playerPara.hp.ToString("00") + " / 100";
		}		
	}

}
