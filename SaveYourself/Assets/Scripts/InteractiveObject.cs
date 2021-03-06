﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class InteractiveObject : MonoBehaviour {

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
