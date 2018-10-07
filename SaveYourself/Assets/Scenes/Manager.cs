using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager<T> : MonoBehaviour where T : Manager<T>
{
    static public T Instance;
    void Awake()
    {
        if (Instance == null)
        {            
            Instance = GetComponent<T>();
        }
    }

}
