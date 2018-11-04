using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CWindow;
using UnityEngine.UI;
using DG.Tweening;
public class ExtinguisherGame : BaseWindow{

    Image[] images;
    public void OnPickUp()
    {
        InputManager.Instance.canSwitch = false;
        images = GetComponentsInChildren<Image>();
        foreach (var item in images)
        {
            item.color = new Color(1, 1, 1, 0);
            item.DOFade(1, 1);
        }
        Pop();
        UIManager.currentWindow = null;
    }
    int i = 0;
    void Update () {
        if (isOpen)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
				AudioManager.Instance.PlayGameplayAudioClip(GamePlayAudioClip.Extinguisher_Excute);
                if (i == 0) images[0].DOFade(0, 1);
                if (i == 1) images[2].DOFade(0, 1);
                if (i == 2) images[1].DOFade(0, 1);
                i++;
            }

            if (i > 2)
            {
                isOpen = false;
                Close();
                Extinguisher ex = InventoryManager.Instance.inventory.EquippedItem() as Extinguisher;
                ex.canBeUsed = true;
                InputManager.Instance.canSwitch = true;
                i = 0;
            }
        }

        
	}
}
