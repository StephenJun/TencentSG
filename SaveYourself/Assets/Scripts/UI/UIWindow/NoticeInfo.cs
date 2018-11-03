using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CWindow;
using UnityEngine.UI;

public class NoticeInfo : BaseWindow {

	public void Init(bool showConfirmButton, string btnName)
	{
		ConfirmButton.gameObject.SetActive(showConfirmButton);
		ConfirmButton.GetComponentInChildren<Text>().text = btnName;
	}

	public void UpdateState(Transform obj)
	{
		StartCoroutine(UpdateWindowState(obj));
	}

	private IEnumerator UpdateWindowState(Transform obj)
	{
		while(true)
		{
			yield return null;
			if (obj == null || Vector3.Distance(obj.transform.position, GameManager.Instance.player.transform.position) > 2.0f)
			{
				Close();
				yield break;
			}
		}
	}

}
