using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Expression : MonoBehaviour {

	private Camera mainCam;
	private SpriteRenderer sr;

	// Use this for initialization
	void Start () {
		mainCam = Camera.main;
		sr = GetComponent<SpriteRenderer>();
		HideExpression();
	}
	
	// Update is called once per frame

	public void ShowExpression(ExpressionType eType)
	{
		switch (eType)
		{
			case ExpressionType.Search:
				sr.sprite = Resources.Load<Sprite>("ExpressionIcon/E_Search");
				break;
			case ExpressionType.Shock:
				sr.sprite = Resources.Load<Sprite>("ExpressionIcon/E_Shock");
				break;
			case ExpressionType.Sleep:
				sr.sprite = Resources.Load<Sprite>("ExpressionIcon/E_Sleep");
				break;
		}
		sr.DOFade(1, 0.2f);

		DOVirtual.DelayedCall(2.0f, () => HideExpression());
	}

	public void HideExpression()
	{
		sr.DOFade(0, 0.5f);
	}
}

public enum ExpressionType
{
	Search = 0,
	Shock = 1,
	Sleep = 2
}
