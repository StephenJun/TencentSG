using System.Collections;
using UnityEngine;
using DG.Tweening;
public class WallChunk : FracturedWreckage {

    public float minScale = 0.8f;
    public float maxScale = 1f;

    protected override void Start()
    {
        base.Start();
        //m_transfrom.DOLocalMove(m_transfrom.localPosition * 5f, 0.8f).From();

        float lastRandom = 1;
        m_transfrom.DOLocalMove(m_transfrom.localPosition * 5f - Vector3.back * 100, 0.8f).From();
        lastRandom = Random.Range(0.8f, 1f);
        transform.localScale = new Vector3(transform.localScale.x, lastRandom * transform.localScale.y, transform.localScale.z);
    }

    public override void CollisionEnter(string trigger)
    {
        if (isTriggered)
            return;
        if (isMovable)
        {
            if (trigger == "Player")
            {
                Collapse(PlayerDamageLevel);
            }
            else if (trigger == "Projectile")
            {
                Collapse(ProjectileDamageLevel);
            }
        }
    }
    public override void Collapse(int recursionTimes = 2)
    {
        if (recursionTimes-- <= 0)
            return;
        for (int i = 0; i < AdjacencyWreckages.Count; i++)
        {
            AdjacencyWreckages[i].Collapse(recursionTimes);
        }
        isTriggered = true;
        rg.constraints = RigidbodyConstraints.FreezeRotation;
        StartCoroutine(Vanish());
    }


    public IEnumerator Vanish()
    {
        rg.AddRelativeForce(m_transfrom.localPosition * 5, ForceMode.Impulse);
        //m_transfrom.parent = null;
        yield return new WaitForSeconds(3.0f);
        //m_transfrom.DOScale(0, 6f);
        //m_transfrom.DOMoveY(-2f, 5f);

        //Destroy(gameObject,3f);
    }
}
