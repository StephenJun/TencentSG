using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FracturedWreckage : MonoBehaviour {

    public bool isMovable = true;
    public bool isTriggered = false;

    public int PlayerDamageLevel = 2;
    public int ProjectileDamageLevel = 2;
    
    static public AnimationCurve restoringCurve;
    public List<FracturedWreckage> AdjacencyWreckages;

    Vector3 originPosition;
    Quaternion originRotation;
    protected Transform m_transfrom;
    protected Rigidbody rg;
    protected virtual void Start()
    {
        rg = GetComponent<Rigidbody>();
        m_transfrom = GetComponent<Transform>();
        originPosition = m_transfrom.localPosition;
        originRotation = m_transfrom.localRotation;
    }

    public virtual void CollisionEnter(string trigger)
    {
        if (FracturedWreckageManager.effect_Enter)
            Destroy(Instantiate(FracturedWreckageManager.effect_Enter,GameObject.FindWithTag("Player").transform.position + Vector3.down * 1.5f, m_transfrom.rotation),1f);
    }
    public virtual void CollisionExit(string trigger)
    {
        if (FracturedWreckageManager.effect_Exit)
            Destroy(Instantiate(FracturedWreckageManager.effect_Exit, GameObject.FindWithTag("Player").transform.position + Vector3.down * 1.5f, m_transfrom.rotation),1f);
    }



    public IEnumerator Restore()
    {
        Vector3 startPosition = m_transfrom.localPosition;
        Quaternion startRotaton = m_transfrom.localRotation;
        //float deltaTime = Mathf.Log(Vector3.Distance(startPosition, originPosition) + 1,2);
        float deltaTime = Mathf.Sqrt(Vector3.Distance(startPosition, originPosition)) / 1.2f;
        float timer = 0;
        rg.constraints = RigidbodyConstraints.FreezeAll;
        while (timer < deltaTime)
        {
            m_transfrom.localPosition = Vector3.Lerp(startPosition, originPosition, restoringCurve.Evaluate(timer / deltaTime));
            m_transfrom.localRotation = Quaternion.Slerp(startRotaton, originRotation, restoringCurve.Evaluate(timer / deltaTime));
            timer += Time.deltaTime;
            yield return null;
        }
        m_transfrom.localPosition = originPosition;
        m_transfrom.localRotation = originRotation;
        isTriggered = false;
    }
    [ContextMenu("collapse")]
    public void collapse()
    {
        Collapse();
    }

    public virtual void Collapse(int recursionTimes = 2)
    {
        if (recursionTimes-- <= 0)
            return;
        for (int i = 0; i < AdjacencyWreckages.Count; i++)
        {
            AdjacencyWreckages[i].Collapse(recursionTimes);
        }
        isTriggered = true;
        rg.constraints = RigidbodyConstraints.None;
    }
    

    #region Test
    //public List<Transform> path = new List<Transform>();
    //[ContextMenu("CollapseTestStart")]
    //public void CollapseTestStart()
    //{
    //    path.Clear();
    //    CollapseTest(path, PlayerDamageLevel);
    //}
    //public void CollapseTest(List<Transform> path ,int recursionTimes = 2)
    //{
    //    Debug.Log("Enter " + recursionTimes);
    //    path.Add(m_transfrom);
    //    if (recursionTimes-- <= 0)
    //    {
    //        Debug.Log("Exit " + recursionTimes + 1);
    //        return;
    //    }

    //    GetComponent<MeshRenderer>().material = change;
    //    rg.constraints = RigidbodyConstraints.None;
    //    int maxIndex = AdjacencyWreckages.Count;
    //    for (int i = 0; i < maxIndex; i++)
    //    {
    //        AdjacencyWreckages[i].CollapseTest(path,recursionTimes);
    //    }
    //    //AdjacencyWreckages.Clear();
    //    Debug.Log("Exit " + (recursionTimes + 1));

    //}
    #endregion


}
