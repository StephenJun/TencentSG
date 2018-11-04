using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CWindow;
using DG.Tweening;

public class PlayerSpwanPoint : MonoBehaviour {

	private Transform arrow;

	public Vector3 tweenPosition = new Vector3(0, 1, 0);
	public Vector3 tweenRotation = new Vector3(0, 90, 0);
	public float duration = 1.0f;

	private void Start()
	{
		arrow = transform.Find("Arrow");
		StartMovement();
	}

	private void OnTriggerEnter(Collider other)
    {
        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc)
        {
			BaseWindow popup = UIManager.PopWindow(WindowName.GenericPopup, "Go to sleep?");
			popup.GetComponent<GenericPopup>().Init(true, "tip");
			popup.confirm += delegate
			{
				gameObject.SetActive(false);
				InputManager.Instance.canControl = false;
				AudioManager.Instance.PlayGameplayAudioClip(GamePlayAudioClip.FireEngine);
				pc.expression.ShowExpression(ExpressionType.Sleep, 2.0f);
				CameraController.Instance.SetRGBShaderActive(Color.black, 3.0f);
				DOVirtual.DelayedCall(3.0f, () => {
					LevelController.Instance.SwitchGameState(GameState.EscapeState);
					pc.transform.rotation = transform.rotation;
					pc.transform.position = transform.position;
				});		
			};
            Debug.Log("The game start right away~~");
        }
    }

	private void StartMovement()
	{
		Tweener moveTween = arrow.DOLocalMove(tweenPosition, duration);
		Tweener rotTween = arrow.DOLocalRotate(tweenRotation, duration);
		rotTween.SetLoops(-1, LoopType.Incremental);
		moveTween.SetLoops(-1, LoopType.Yoyo);
		rotTween.SetRelative(true);
		moveTween.SetRelative(true);
		rotTween.SetEase(Ease.InOutSine);
		moveTween.SetEase(Ease.InOutSine);
	}


}
