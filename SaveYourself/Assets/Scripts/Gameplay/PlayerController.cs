using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CWindow;

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
		public GameObject[] charModels;
	}

    public PlayerParameter _playerPara = new PlayerParameter();
    public List<PlayerAction> playerActions = new List<PlayerAction>();

	public Transform CarrierTrans;
	public Expression expression;

	public float damageMultiplier = 1;
	private InteractiveObject currentEquipped;
	[HideInInspector]public int currentCharIndex;

    [SerializeField]
    private Camera viewCamera;
    private NavMeshAgent navMeshAgent;
	private CharacterController charController;
	private Rigidbody rb;
	private Animator anim;
	private GameObject lastObject;

    protected override void Awake()
    {
		navMeshAgent = GetComponent<NavMeshAgent>();
		charController = GetComponent<CharacterController>();
		rb = GetComponent<Rigidbody>();
		anim = GetComponentInChildren<Animator>();
		base.Awake();
    }
    void Start()
    {
		GameManager.Instance.player = this;
        GameManager.Instance.viewCamera = viewCamera;
		GetRandomCharacter();
    }

	private void GetRandomCharacter()
	{
		foreach (var item in _playerPara.charModels)
		{
			item.SetActive(false);
		}
		currentCharIndex = Random.Range(0, _playerPara.charModels.Length);
		_playerPara.charModels[currentCharIndex].SetActive(true);
		anim = _playerPara.charModels[currentCharIndex].GetComponent<Animator>();
	}

	private void Update()
	{
		if (InputManager.Instance.canControl)
		{
			float horizontalInput = Input.GetAxis("Horizontal");
			float vertivalInput = Input.GetAxis("Vertical");
			Vector3 charDir = new Vector3(horizontalInput, 0, vertivalInput);
			anim.SetFloat("Speed", charDir.magnitude);
			//charController.Move(charDir * _playerPara.speed);		
			if (charDir != Vector3.zero)
			{
				rb.velocity = charDir * _playerPara.speed;
				Quaternion tempRot = Quaternion.LookRotation(charDir, Vector3.up);
				transform.rotation = tempRot;
			}
		}

		//~~~~~~Interactive Input Handler~~~~~~~
		if (Input.GetKeyDown(ConfirmKey))
		{
			if(UIManager.currentWindow != null)
			{
				UIManager.currentWindow.ConfirmAction();
			}		
		}

		if (Input.GetKeyDown(CancelKey))
		{
			if (UIManager.currentWindow != null)
				UIManager.currentWindow.CancelAction();
		}

		if (Input.GetKeyDown(SwitchItem) && InputManager.Instance.canSwitch)
		{
			InventoryManager.Instance.inventory.SwitchItem();
			AudioManager.Instance.PlayGameplayAudioClip(GamePlayAudioClip.Switch);
		}

		if (Input.GetKeyDown(UsingItem))
		{
			currentEquipped = InventoryManager.Instance.inventory.EquippedItem();
			if (currentEquipped is Extinguisher)
			{
				anim.SetBool("HoldOrNot", true);
				PlaybackManager.Instance.PushPose(PoseType.Extinguisher);
				AudioManager.Instance.PlayGameplayAudioClip(GamePlayAudioClip.Extinguisher);
			}
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
				PlaybackManager.Instance.PushPose(PoseType.Smoke);
				damageMultiplier = currentEquipped.GetComponent<Towel>().defenderProvided;
			}
		}

		if (Input.GetKeyUp(UsingItem))
		{
			anim.SetBool("HoldOrNot", false);
			AudioManager.Instance.StopGameplayAudio();
			if (CarrierTrans.childCount != 0)
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
				expression.ShowExpression(ExpressionType.Search, 1.5f);
			}
			if (Input.GetKeyDown(InteractKey) && !InventoryManager.Instance.inventory.isInventoryFull)
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
				expression.HideExpression();
				lastObject = null;
			}
		}
	}


    #region PlayerParaStatus
    public void DamageReceiver(float damage)
    {
		if(_playerPara.hp > 0)
		{
			_playerPara.hp -= damage * damageMultiplier;
			CameraController.Instance.SetShockShaderActive(true);
		}    
        if(_playerPara.hp <= 0 && InputManager.Instance.canControl)
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