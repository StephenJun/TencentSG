using UnityEngine;
public class WallManager : FracturedWreckageManager
{
    public float minScale = 0.8f;
    public float maxScale = 1f;
    //public AnimationCurve animationCurveX;
    //public AnimationCurve animationCurveY;
    //protected override void Start()
    //{

    //    Keyframe[] XKeys = new Keyframe[5];// { new Keyframe(0.2f, Random.Range(minScale, maxScale)), new Keyframe(0.2f, Random.Range(minScale, maxScale)), new Keyframe(0.2f, Random.Range(minScale, maxScale)), new Keyframe(0.2f, Random.Range(minScale, maxScale)), new Keyframe(0.2f, Random.Range(minScale, maxScale)) };
    //    Keyframe[] YKeys = new Keyframe[5];
    //    for (int i = 0; i < XKeys.Length; i++)
    //    {
    //        XKeys[i] = new Keyframe(0.2f * i, Random.Range(minScale, maxScale));
    //    }
    //    YKeys[0] = XKeys[0];
    //    for (int i = 1; i < YKeys.Length; i++)
    //    {
    //        YKeys[i] = new Keyframe(0.2f * i, Random.Range(minScale, maxScale));
    //    }
    //    animationCurveX = new AnimationCurve(XKeys);
    //    animationCurveY = new AnimationCurve(YKeys);

    //    base.Start();
    //}
    public override void Generate()
    {
        FracturedWreckage.restoringCurve = restoringCurve;
        scale = Tile.transform.localScale;
        Vector3 adjust = new Vector3(((scale.x + space.x) * (sliceX - 1)) / 2, ((scale.y + space.y) * (sliceY - 1)) / 2, ((scale.z + space.z) * (sliceZ - 1)) / 2);
        float[,] heights = new float[sliceX, sliceZ];
        for (int i = 0; i < sliceX; i++)
        {
            for (int j = 0; j < sliceY; j++)
            {
                for (int k = 0; k < sliceZ; k++)
                {
                    float value;
                    //if (k == 0)
                    //{
                    //    value = animationCurveX.Evaluate(i / sliceX);
                    //    Debug.Log(value + " " + 0);
                    //}
                    //else if (i == 0)
                    //{
                    //    value = animationCurveY.Evaluate(k / sliceZ);
                    //    Debug.Log(value + " " + 1);
                    //}
                    //else
                    //{
                    //    value = (animationCurveX.Evaluate((i - 1) / sliceX) + animationCurveX.Evaluate(i / sliceX) + animationCurveY.Evaluate((k - 1) / sliceZ))/3;
                    //    Debug.Log(animationCurveX.Evaluate((i - 1) / sliceX) + " " + animationCurveX.Evaluate(i / sliceX) + " " + animationCurveY.Evaluate((k - 1) / sliceZ) + " " + 2);
                    //}
                    //heights[k,i] = ;
                    //Vector3 tempScale = new Vector3(scale.x, scale.y * Mathf.Clamp01(Random.Range(value - 0.1f, value + 0.1f)), scale.z);
                    //Vector3 tempScale = new Vector3(scale.x, scale.y * value, scale.z);

                    InstantiateTile(new Vector3(i * (scale.x + space.x), j * (scale.y + space.y), k * (scale.z + space.z)) - adjust);
                }
            }
        }
    }
    public void Init()
    {
        Invoke("Crumble", 5f);
    }
    public float AdjustZ()
    {
        return (space.z + scale.z) * (sliceZ - 1) / 2 + 0.5f;
    }
    public float AdjustY()
    {
        return (space.y + scale.y) * (sliceY - 1) / 2 + 0.5f;
    }

    [ContextMenu("Crumble")]
    public void Crumble()
    {
        WallChunk[] wallChunks = GetComponentsInChildren<WallChunk>();
        foreach (WallChunk wallChunk in wallChunks)
        {
            wallChunk.Collapse(1);
        }
        //Destroy(gameObject);
    }

}
