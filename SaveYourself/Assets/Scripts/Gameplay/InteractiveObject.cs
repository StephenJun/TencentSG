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

    public Material defaultMat;
    public Material highlightMat;
    public Action OnInteractive;
    public bool IsHighLight;
    public void HighlightOn()
    {
        GetComponent<MeshRenderer>().material = highlightMat;
    }

    public void HighlightOff()
    {
        GetComponent<MeshRenderer>().material = defaultMat;
    }

}
