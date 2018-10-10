using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CWindow;
public class PlayerController : MonoBehaviour
{

    [SerializeField]
    Camera viewCamera;
    NavMeshAgent navMeshAgent;
    public List<PlayerAction> playerActions = new List<PlayerAction>();

    void Awake()
    {
        
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
            Debug.Log(Vector3.Distance(target, transform.position));
            yield return null;
        }
        navMeshAgent.SetDestination(transform.position);
        UIManager.PopWindow(WindowName.ParentsCenter, targetObj.detailInfo);
        
        Debug.Log("Reached");
        yield return new WaitForSeconds(2f);
        UIManager.CloseWindow(WindowName.ParentsCenter);
        
    }
}


public class PlayerAction
{
    public string description;
    public Vector3 position;
}