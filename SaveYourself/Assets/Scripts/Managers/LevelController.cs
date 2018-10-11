using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : Singleton<LevelController> {

    [SerializeField]
    private Transform fireSpawnerRoot;
    private List<Transform> fireSpawners = new List<Transform>();

    private void Start()
    {
        fireSpawnerRoot = GameObject.Find("FireSpawnerRoot").transform;
        for (int i = 0; i < fireSpawnerRoot.childCount; i++)
        {
            fireSpawners.Add(fireSpawnerRoot.GetChild(i));
        }
        foreach (var spwaner in fireSpawners)
        {
            GameEffectManager.Instance.AddWorldEffect("Fire", spwaner.position, 1, 60);
        }
    }    
}
