using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CWindow;

public class NoticeInfo : BaseWindow {

	public void Init(bool showConfirmButton)
	{
		ConfirmButton.gameObject.SetActive(showConfirmButton);
	}

}
