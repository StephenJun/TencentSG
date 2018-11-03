using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CWindow;

public class HUD : BaseWindow {

	public Image playerHpBar;
	public Text playerHpText;
	public Image HeadPortrait;

	private void Update()
	{
		if (PlayerController.Instance)
		{
			playerHpBar.fillAmount = PlayerController.Instance._playerPara.hp / 100;
			playerHpText.text = PlayerController.Instance._playerPara.hp.ToString("00") + " / 100";
			float tempHp = PlayerController.Instance._playerPara.hp;
			if (tempHp < 33)
			{
				HeadPortrait.sprite = Resources.Load<Sprite>("HeadPortrait/UI_Bear_Fail");
			}else if (tempHp < 66)
			{
				HeadPortrait.sprite = Resources.Load<Sprite>("HeadPortrait/UI_Bear_Hurry");
			}
			else
			{
				HeadPortrait.sprite = Resources.Load<Sprite>("HeadPortrait/UI_Bear_Success");
			}
		}		
	}

}
