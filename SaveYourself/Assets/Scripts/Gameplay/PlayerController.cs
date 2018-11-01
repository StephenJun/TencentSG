using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CWindow;
using System;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : Singleton<PlayerController>
{
    [System.Serializable]
    public class PlayerParameter
    {
        public float hp = 100;
		public float speed = 5;
    }

    public PlayerParameter _playerPara = new PlayerParameter();
    public List<PlayerAction> playerActions = new List<PlayerAction>();

    [SerializeField]
    Camera viewCamera;

    private NavMeshAgent navMeshAgent;
	private CharacterController charController;

    protected override void Awake()
    {
        base.Awake();
        navMeshAgent = GetComponent<NavMeshAgent>();
		charController = GetComponent<CharacterController>();
    }
    void Start()
    {
        GameManager.Instance.player = this;
        GameManager.Instance.viewCamera = viewCamera;
    }

	private void Update()
	{
		float horizontalInput = Input.GetAxis("Horizontal");
		float vertivalInput = Input.GetAxis("Vertical");
		Vector3 charDir = new Vector3(horizontalInput, 0, vertivalInput);
		charController.Move(charDir * _playerPara.speed);
		if(charDir != Vector3.zero)
		{
			Quaternion tempRot = Quaternion.LookRotation(charDir, Vector3.up);
			transform.rotation = tempRot;
		}
	}


	public void MoveTo(Vector3 destination)
    {
        navMeshAgent.SetDestination(new Vector3(destination.x,0,destination.z));
    }
    public void MoveTo(InteractiveObject targetObj)
    {
        StartCoroutine(MoveToObject(targetObj));
        
    }
    IEnumerator MoveToObject(InteractiveObject targetObj)
    {
        navMeshAgent.SetDestination(targetObj.transform.position);
        Vector3 target = targetObj.transform.position;
        while (Vector3.Distance(target,transform.position) > 4.0f)
        {
            //Debug.Log(Vector3.Distance(target, transform.position));
            yield return null;
        }
        navMeshAgent.SetDestination(transform.position);
		BaseWindow bw = UIManager.PopWindow(WindowName.ParentsCenter, targetObj.detailInfo);
		bw.GetComponent<RectTransform>().anchoredPosition = viewCamera.WorldToScreenPoint(targetObj.transform.position);
		if (bw is NoticeInfo)
		{
			bw.GetComponent<NoticeInfo>().Init(targetObj.isCollectable && LevelController.Instance.gameState == GameState.EscapeState);
		}
		if (LevelController.Instance.gameState == GameState.EscapeState)
        {

			bw.confirm += delegate
			{
				InventoryManager.Instance.inventory.ShowTimerWhenInteracting(targetObj.timeNeedToCollect, targetObj.OnInteractive);
			};
		}

        //yield return new WaitForSeconds(2f);
        //UIManager.CloseWindow(WindowName.ParentsCenter);
        
    }


    #region PlayerParaStatus
    public void DamageReceiver(float damage)
    {
        _playerPara.hp -= damage;
        if(_playerPara.hp <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        _playerPara.hp = 0;
		LevelController.Instance.SwitchGameState(GameState.GameOver);
        Debug.Log("GameOver");
    }
    #endregion
}


public class PlayerAction
{
    public string description;
    public Vector3 position;

    public PlayerAction()
    {
        //description = 
    }
    static public PlayerAction extinguisher;// = new PlayerAction(;
}