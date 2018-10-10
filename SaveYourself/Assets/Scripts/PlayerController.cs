using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CWindow;
public class PlayerController : Singleton<PlayerController>
{

    [SerializeField]
    Camera viewCamera;
    NavMeshAgent navMeshAgent;
    public List<PlayerAction> playerActions = new List<PlayerAction>();

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
        while (Vector3.Distance(target, transform.position) > 1.8f)
        {
            yield return null;
        }
        navMeshAgent.SetDestination(transform.position);


        Debug.Log("Reached");
        targetObj.OnInteractive();
        yield return new WaitForSeconds(2f);
        UIManager.CloseWindow(WindowName.ParentsCenter);

    }
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