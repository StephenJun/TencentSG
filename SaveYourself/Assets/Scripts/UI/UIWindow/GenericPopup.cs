using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CWindow;

public class GenericPopup : BaseWindow {

	public Image img_Title;

	public void Init(bool showCancelButton, string title)
	{
		CancelButton[0].gameObject.SetActive(showCancelButton);
		img_Title.sprite = Resources.Load<Sprite>("UI/Ribbon_" + title);
	}

}
