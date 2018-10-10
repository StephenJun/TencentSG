using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CWindow;
public class Extinguisher : InteractiveObject {

    private void Start()
    {
        OnInteractive += delegate {
            UIManager.PopWindow(WindowName.ParentsCenter);
            PlayerController.Instance.playerActions.Add(new PlayerAction());
            };
    }
}
