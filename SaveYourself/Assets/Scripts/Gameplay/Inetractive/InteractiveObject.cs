using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public string detailInfo;
    public string itemName;
    public int maxStack = 1;
    public bool isCollectable;
    public float timeNeedToCollect;
	public float durability = 100;
	public float depletionSpeed = 8.0f;
	public float defenderProvided = 0.6f;


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

	public void OnUsing()
	{
		durability -= depletionSpeed * Time.deltaTime;
	}

}
