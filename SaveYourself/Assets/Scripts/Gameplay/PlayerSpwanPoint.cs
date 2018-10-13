using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CWindow;

public class PlayerSpwanPoint : MonoBehaviour {


    private void OnTriggerEnter(Collider other)
    {
        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc)
        {
			UIManager.PopWindow(WindowName.GenericPopup, "Go to sleep?").confirm += delegate
			{
				LevelController.Instance.SwitchGameState(GameState.EscapeState);
				pc.transform.rotation = transform.rotation;
				pc.transform.position = transform.position;
				gameObject.SetActive(false);
			};
            Debug.Log("The game start right away~~");
        }
    }
}
