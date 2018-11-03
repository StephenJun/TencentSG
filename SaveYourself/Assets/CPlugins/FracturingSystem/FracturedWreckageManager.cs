using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
public class FracturedWreckageManager : MonoBehaviour
{

    public bool SelfGenerate = false;
    public static GameObject effect_Enter;
    public static GameObject effect_Exit;
    [SerializeField]
    GameObject effect_Enter_Prefab;
    [SerializeField]
    GameObject effect_Exit_Prefab;
    public int sliceX = 0;
    public int sliceY = 0;
    public int sliceZ = 0;
    public Vector3 space = new Vector3(0, 0, 0f);
    protected Vector3 scale;
    public GameObject Tile;
    public AnimationCurve restoringCurve;
    protected FracturedWreckage[] fracturedWreckages;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            foreach (FracturedWreckage fracturedWreckage in fracturedWreckages)
            {
                fracturedWreckage.StartCoroutine("Restore");
            }
        }
    }
    protected virtual void Start()
    {
        if (SelfGenerate)
            Generate();
        if (effect_Enter_Prefab)
            effect_Enter = effect_Enter_Prefab;
        if (effect_Exit_Prefab)
            effect_Exit = effect_Exit_Prefab;
        fracturedWreckages = transform.GetComponentsInChildren<FracturedWreckage>();


    }

    public virtual void Generate()
    {
        FracturedWreckage.restoringCurve = restoringCurve;
        scale = Tile.transform.localScale;
        Vector3 adjust = new Vector3(((scale.x + space.x) * (sliceX - 1)) / 2, ((scale.y + space.y) * (sliceY - 1)) / 2, ((scale.z + space.z) * (sliceZ - 1)) / 2);
        for (int i = 0; i < sliceX; i++)
        {
            for (int j = 0; j < sliceY; j++)
            {
                for (int k = 0; k < sliceZ; k++)
                {
                    InstantiateTile(new Vector3(i * (scale.x + space.x), j * (scale.y + space.y), k * (scale.z + space.z)) - adjust);
                }
            }
        }
    }


    protected GameObject InstantiateTile(Vector3 postion)
    {
        //生成
        GameObject newTile = Instantiate(Tile, postion, Quaternion.identity);
        int m_HashCode = newTile.GetHashCode();
        FracturedWreckage fracturedWreckage = newTile.GetComponent<FracturedWreckage>();
        newTile.transform.SetParent(transform, false);

        List<FracturedWreckage> AdjacencyWreckages = new List<FracturedWreckage>();
        //List<Collider> colliders = new List<Collider>(Physics.OverlapSphere(newTile.transform.position, 2.0f));
        //检测附近方块返回数组
        List<Collider> colliders = new List<Collider>(Physics.OverlapSphere(newTile.transform.position, 0.26f));

        foreach (Collider collider in colliders)
        {
            //排除其他物体与自己
            if (collider.gameObject.tag == "WallChunk" && m_HashCode != collider.gameObject.GetHashCode())
            {
                //找到另一半
                FracturedWreckage other = collider.gameObject.GetComponent<FracturedWreckage>();
                //if (AdjacencyWreckages.Count < 100)
                AdjacencyWreckages.Add(other);
                //if (other.AdjacencyWreckages.Count < 100)
                other.AdjacencyWreckages.Add(fracturedWreckage);
            }
        }
        fracturedWreckage.AdjacencyWreckages = AdjacencyWreckages;
        return newTile;
    }

    //execute time 65 66 ms 15*3*15
    //execute time 71 ms 100*20*20 no instantiate
    void InstantiateTile_Ordered(Vector3 postion)
    {
        //生成
        GameObject newTile = Instantiate(Tile, postion, Quaternion.identity);
        int m_HashCode = newTile.GetHashCode();
        FracturedWreckage fracturedWreckage = newTile.GetComponent<FracturedWreckage>();
        newTile.transform.SetParent(transform, false);

        //List<Collider> colliders = new List<Collider>(Physics.OverlapSphere(newTile.transform.position, 2.0f));
        //检测附近方块返回数组
        List<Collider> colliders = new List<Collider>(Physics.OverlapSphere(newTile.transform.position, 0.26f));

        foreach (Collider collider in colliders)
        {
            //排除其他物体与自己
            if (collider.gameObject.tag == "WallChunk" && m_HashCode != collider.gameObject.GetHashCode())
            {
                //找到另一半
                FracturedWreckage other = collider.gameObject.GetComponent<FracturedWreckage>();
                fracturedWreckage.AdjacencyWreckages.Add(other);
                other.AdjacencyWreckages.Add(fracturedWreckage);
            }
        }
    }
}
