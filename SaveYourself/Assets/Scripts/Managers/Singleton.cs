using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    static public T Instance;
    protected virtual void Awake()
    {
        if (Instance == null)
        {            
            Instance = GetComponent<T>();
        }
    }

}
