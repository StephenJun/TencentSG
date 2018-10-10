using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour {

    public Material defaultMat;
    public Material highlightMat;
    public bool IsHighLight;
    public string detailInfo;

	public void HighlightOn()
    {
        GetComponent<MeshRenderer>().material = highlightMat;
    }
    public void HighlightOff()
    {
        GetComponent<MeshRenderer>().material = defaultMat;
    }
}
