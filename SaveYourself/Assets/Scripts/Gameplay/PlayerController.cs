using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CWindow;
using System;

//[RequireComponent(typeof(CharacterController))]
public class PlayerController : Singleton<PlayerController>
{
	[Header("HOT KEY")]
	[SerializeField] private KeyCode ConfirmKey = KeyCode.J;
	[SerializeField] private KeyCode CancelKey = KeyCode.K;
	[SerializeField] private KeyCode SwitchItem = KeyCode.Q;
	[SerializeField] private KeyCode UsingItem = KeyCode.LeftShift;
	[SerializeField] private KeyCode InteractKey = KeyCode.E;

	[System.Serializable]
    public class PlayerParameter
    {
        public float hp = 100;
		public float speed = 5;
	}

    public PlayerParameter _playerPara = new PlayerParameter();
    public List<PlayerAction> playerActions = new List<PlayerAction>();

	public Transform CarrierTrans;

	public float damageMultiplier = 1;
	private InteractiveObject currentEquipped;

    [SerializeField]
    private Camera viewCamera;
    private NavMeshAgent navMeshAgent;
	private CharacterController charController;
	private Rigidbody rb;
	private GameObject lastObject;

    protected override void Awake()
    {
        base.Awake();
        navMeshAgent = GetComponent<NavMeshAgent>();
		charController = GetComponent<CharacterController>();
		rb = GetComponent<Rigidbody>();
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
		//charController.Move(charDir * _playerPara.speed);		
		if (charDir != Vector3.zero)
		{
			rb.velocity = charDir * _playerPara.speed;
			Quaternion tempRot = Quaternion.LookRotation(charDir, Vector3.up);
			transform.rotation = tempRot;
		}

		//~~~~~~Interactive Input Handler~~~~~~~
		if (Input.GetKeyDown(ConfirmKey))
		{
			UIManager.currentWindow.ConfirmAction();
		}

		if (Input.GetKeyDown(CancelKey))
		{
			UIManager.currentWindow.CancelAction();
		}

		if (Input.GetKeyDown(SwitchItem))
		{
			InventoryManager.Instance.inventory.SwitchItem();
		}

		if (Input.GetKey(UsingItem))
		{
			currentEquipped = InventoryManager.Instance.inventory.EquippedItem();
			if(currentEquipped != null)
			{
				currentEquipped.OnUsing();
			}
			if (currentEquipped is Extinguisher)
			{
				for (int i = 1; i < 5; i++)
				{
					int posX = Mathf.CeilToInt((transform.position + transform.forward * i).x);
					int posY = Mathf.CeilToInt((transform.position + transform.forward * i).z);
					FloorManager.Instance.fd[posX, posY].Extinguish();
				}
	
			}else if(currentEquipped is Towel)
			{
				damageMultiplier = currentEquipped.GetComponent<Towel>().defenderProvided;
			}
		}

		if (Input.GetKeyUp(UsingItem))
		{
			if(CarrierTrans.childCount != 0)
			{
				currentEquipped = CarrierTrans.GetChild(0).GetComponent<InteractiveObject>();
			}	
			if (currentEquipped != null)
			{
				currentEquipped.OnUnLoad();
			}
			damageMultiplier = 1;
		}


	}

	private void FixedUpdate()
	{
		RaycastHit hitInfo;
		if (Physics.Raycast(transform.position + transform.up * 0.5f, transform.forward, out hitInfo, 1.0f) && hitInfo.collider.tag == "InteractiveObject")
		{
			if (lastObject != hitInfo.collider.gameObject)
			{
				lastObject = hitInfo.collider.gameObject;
				lastObject.GetComponent<InteractiveObject>().HighlightOn();
				
			}
			if (Input.GetKeyDown(InteractKey))
			{
				InteractiveObject io = hitInfo.collider.GetComponent<InteractiveObject>();
				if (io)
				{
					Vector3 tempView = viewCamera.WorldToViewportPoint(io.transform.position);
					Vector3 positionInViewPoint = new Vector3(Mathf.Clamp01(tempView.x) * 1920, Mathf.Clamp01(tempView.y) * 1080, 0);
					BaseWindow bw = UIManager.PopWindow(WindowName.ParentsCenter, io.detailInfo);
					bw.GetComponent<RectTransform>().anchoredPosition = positionInViewPoint;

					if (bw is NoticeInfo)
					{
						bool temp = io.stateCanInteract == LevelController.Instance.gameState;
						bw.GetComponent<NoticeInfo>().Init(temp, io.confirmButtonName);
						bw.GetComponent<NoticeInfo>().UpdateState(io.transform);
						if (temp)
						{
							bw.confirm += delegate
							{
								InventoryManager.Instance.inventory.ShowTimerWhenInteracting(io.timeNeedToCollect, io.OnInteractive, io.transform);
							};
						}
					}
				}
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
			bw.GetComponent<NoticeInfo>().Init(LevelController.Instance.gameState == GameState.EscapeState, "Pick");
		}
		if (LevelController.Instance.gameState == GameState.EscapeState)
        {

			bw.confirm += delegate
			{
				InventoryManager.Instance.inventory.ShowTimerWhenInteracting(targetObj.timeNeedToCollect, targetObj.OnInteractive, targetObj.transform);
			};
		}

        //yield return new WaitForSeconds(2f);
        //UIManager.CloseWindow(WindowName.ParentsCenter);
        
    }


    #region PlayerParaStatus
    public void DamageReceiver(float damage)
    {
        _playerPara.hp -= damage * damageMultiplier;
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