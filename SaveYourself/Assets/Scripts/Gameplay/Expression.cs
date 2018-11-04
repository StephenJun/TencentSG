using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Expression : MonoBehaviour {

	private Camera mainCam;
	private SpriteRenderer sr;

	void Awake () {
		mainCam = Camera.main;
		sr = GetComponent<SpriteRenderer>();
		HideExpression();
	}
	

	public void ShowExpression(ExpressionType eType, float duration)
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
			case ExpressionType.Music:
				sr.sprite = Resources.Load<Sprite>("ExpressionIcon/E_Music");
				break;
		}
		AudioManager.Instance.PlayGameplayAudioClip(GamePlayAudioClip.Expression);
		sr.DOFade(1, 0.2f);

		DOVirtual.DelayedCall(duration, () => HideExpression());
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
	Sleep = 2,
	Music = 3
}
