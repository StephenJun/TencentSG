using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public string detailInfo;
    public string itemName;
    public int maxStack = 1;
    public float timeNeedToCollect;
	public float durability = 100;
	public float depletionSpeed = 8.0f;
	public string confirmButtonName = "Pick";
    public GameState stateCanInteract = GameState.EscapeState;

	public Color defaultColor = Color.white;
	public Color highlightColor = Color.red;
    public Action OnInteractive;
    public bool IsHighLight;
    public void HighlightOn()
    {
		GetComponentInChildren<MeshRenderer>().material.color = highlightColor;
    }

    public void HighlightOff()
    {
		GetComponentInChildren<MeshRenderer>().material.color = defaultColor;
    }

	public virtual void OnUsing()
	{
		durability -= depletionSpeed * Time.deltaTime;
		if(durability < 0)
		{
			Destroy(this.gameObject);
		}
	}

	public virtual void OnUnLoad()
	{
		gameObject.SetActive(false);
	}

}
