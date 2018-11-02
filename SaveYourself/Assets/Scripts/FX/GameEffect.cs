using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEffect : MonoBehaviour {

    ParticleSystem ps;
    float playTime;
    float playDuration;
    float initialSize;
    public GameObject parent;


    public void Init(string effectName)
    {
        this.name = effectName;
        ps = transform.GetComponentInChildren<ParticleSystem>();
        gameObject.SetActive(false);
        initialSize = ps.startSize;
    }

    public void Play(float scale, float time)
    {
        playTime = 0;
        playDuration = time;
        gameObject.SetActive(true);
        if (ps != null) ps.Play(true);
        SetScale(transform, scale);
    }

	public void Play()
	{
		gameObject.SetActive(true);
	}

    public void SetScale(Transform t, float scale)
    {
        //for (int i = 0; i < t.childCount; i++)
        //{
        //	SetScale(t.GetChild(i), scale);
        //	if (t.GetChild(i).GetComponent<ParticleSystem>())
        //	{
        //		t.GetChild(i).GetComponent<ParticleSystem>().startSize = initialSize * scale;
        //	}		
        //}
        t.localScale = new Vector3(scale, scale, scale);
        ps.startSize = initialSize * scale;
    }


    private void Update()
    {
        playTime += Time.deltaTime;
        if (parent != null)
        {
            transform.position = parent.transform.position;
        }
        if (playTime >= playDuration && playDuration > 0)
        {
            Die();
        }
    }

    public void SetParent(GameObject obj)
    {
        this.transform.rotation = obj.transform.rotation;
        this.parent = obj;
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }
}
