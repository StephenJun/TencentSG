using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpwanPoint : MonoBehaviour {


    private void OnTriggerEnter(Collider other)
    {
        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc)
        {
            Debug.Log("The game start right away~~");
        }
    }
}
