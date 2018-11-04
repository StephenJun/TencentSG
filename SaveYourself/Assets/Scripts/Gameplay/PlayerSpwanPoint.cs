using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CWindow;
using DG.Tweening;

public class PlayerSpwanPoint : MonoBehaviour {


    private void OnTriggerEnter(Collider other)
    {
        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc)
        {
			BaseWindow popup = UIManager.PopWindow(WindowName.GenericPopup, "Go to sleep?");
			popup.GetComponent<GenericPopup>().Init(true);
			popup.confirm += delegate
			{
				gameObject.SetActive(false);
				InputManager.Instance.canControl = false;
				pc.expression.ShowExpression(ExpressionType.Sleep);
				DOVirtual.DelayedCall(4.0f, () => {
					LevelController.Instance.SwitchGameState(GameState.EscapeState);
					pc.transform.rotation = transform.rotation;
					pc.transform.position = transform.position;
				});		
			};
            Debug.Log("The game start right away~~");
        }
    }
}
