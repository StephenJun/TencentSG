using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CWindow;

public class GenericPopup : BaseWindow {

	public void Init(bool showCancelButton)
	{
		CancelButton[0].gameObject.SetActive(showCancelButton);
	}

}
