using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTest : MonoBehaviour {
    protected Animator anim;
    public float speed = 0.2f;
    //public bool ApplyGravity = true;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        //if (anim.layerCount >= 2)
        //anim.SetLayerWeight(0, 1);
    }
	
	// Update is called once per frame
	void Update () {
        if (anim != null && anim.isActiveAndEnabled)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                anim.SetBool("MoveOrNot", !anim.GetBool("MoveOrNot")); 
            }
            if (Input.GetButtonDown("Fire2"))
            {
                anim.SetBool("HoldOrNot", !anim.GetBool("HoldOrNot"));
            }
            if (anim.GetBool("HoldOrNot"))
            {
                anim.SetLayerWeight(1, 1);
            }
            else {
                anim.SetLayerWeight(1, 0);
            }
            
        }
    }
}
