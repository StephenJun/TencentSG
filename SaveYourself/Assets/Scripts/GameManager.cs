using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Manager<GameManager> {

    public GameState gameState;

    [SerializeField]
    public Camera viewCamera;
    Vector3 currentTargetPositon;

    public PlayerController player;
    [SerializeField]
    GameObject playerMovePointEffect;

    void Start () {
        
    }
    void Update()
    {
        GetMouseInput();
        HighLightController();
    }

    void GetMouseInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (player == null)
            {
                Debug.Log("PLayer has not been registered");
                return;
            }
            RaycastHit hitInfo;
            if (Physics.Raycast(viewCamera.ScreenPointToRay(Input.mousePosition), out hitInfo) )
            {
                if (hitInfo.collider.tag == "InteractiveObject")
                {
                    player.MoveTo(hitInfo.collider.GetComponent<InteractiveObject>());
                }
                else
                {
                    player.MoveTo(hitInfo.point);
                    Instantiate(playerMovePointEffect, hitInfo.point, Quaternion.identity);
                }
            }

            
        }
    }


    #region HighLight
    GameObject lastObject;

    void HighLightController()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(viewCamera.ScreenPointToRay(Input.mousePosition), out hitInfo) && hitInfo.collider.tag == "InteractiveObject")
        {
            if (lastObject != hitInfo.collider.gameObject)
            {
                lastObject = hitInfo.collider.gameObject;
                lastObject.GetComponent<InteractiveObject>().HighlightOn();
            }
        }
        else
        {
            if (lastObject)
            {
                lastObject.GetComponent<InteractiveObject>().HighlightOff();
                lastObject = null;
            }
        }
    }
    #endregion
}
public enum GameState
{
    SearchState,
    EscapeState,
    GameOver,
    AllClear
}
