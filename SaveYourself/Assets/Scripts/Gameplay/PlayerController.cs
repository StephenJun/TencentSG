using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CWindow;
public class PlayerController : Singleton<PlayerController>
{
    [System.Serializable]
    public class PlayerParameter
    {
        public float hp = 100;
    }

    public PlayerParameter _playerPara = new PlayerParameter();
    public List<PlayerAction> playerActions = new List<PlayerAction>();

    [SerializeField]
    Camera viewCamera;
    NavMeshAgent navMeshAgent;

    protected override void Awake()
    {
        base.Awake();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        GameManager.Instance.player = this;
        GameManager.Instance.viewCamera = viewCamera;
    }
    public void MoveTo(Vector3 destination)
    {
        navMeshAgent.SetDestination(destination);
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
        UIManager.PopWindow(WindowName.ParentsCenter, targetObj.detailInfo);
        if (targetObj.isCollectable)
        {
            InventoryManager.Instance.inventory.ShowTimerWhenInteracting(targetObj);
        }

        yield return new WaitForSeconds(2f);
        UIManager.CloseWindow(WindowName.ParentsCenter);
        
    }


    #region PlayerParaStatus
    public void OnHurt(float damage)
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