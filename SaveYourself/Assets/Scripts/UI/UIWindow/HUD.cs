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
			playerHpText.text = PlayerController.Instance._playerPara.hp.ToString("00") + " %";
			float tempHp = PlayerController.Instance._playerPara.hp;
			int tempIndex = PlayerController.Instance.currentCharIndex;
			string charPortraitName = "";
			switch (tempIndex)
			{
				case 0:
					charPortraitName = "HeadPortrait/UI_Ninja_";
					break;
				case 1:
					charPortraitName = "HeadPortrait/UI_Thief_";
					break;
				case 2:
					charPortraitName = "HeadPortrait/UI_Bear_";
					break;
			}
			if (tempHp < 33)
			{
				HeadPortrait.sprite = Resources.Load<Sprite>(charPortraitName + "Fail");
			}else if (tempHp < 66)
			{
				HeadPortrait.sprite = Resources.Load<Sprite>(charPortraitName + "Hurry");
			}
			else
			{
				HeadPortrait.sprite = Resources.Load<Sprite>(charPortraitName + "Success");
			}
		}		
	}

}
