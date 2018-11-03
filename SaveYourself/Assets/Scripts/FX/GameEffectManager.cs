using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEffectManager : Singleton<GameEffectManager> {

    Dictionary<string, List<GameEffect>> effectPool = new Dictionary<string, List<GameEffect>>();

    private GameManager _GM;

    public void Init()
    {
        _GM = GetComponent<GameManager>();
		effectPool.Clear();
    }

    /// <summary>
    /// Add static effect in somewhere of the scene
    /// </summary>
    /// <param name="effectName"></param>
    /// <param name="pos"></param>
    /// <param name="scale"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public GameEffect AddWorldEffect(string effectName, Vector3 pos, float scale, float time)
    {
        GameEffect ge = GetEffect(effectName, pos);
        if (ge == null) return null;
        ge.transform.position = pos;
        ge.Play(scale, time);
        return ge;
    }

    /// <summary>
    /// Adds effects and sets a parent object that it will follow.
    /// </summary>
    /// <param name="effectName"> The name of this effect</param>
    /// <param name="obj"></param>
    /// <param name="pos"></param>
    /// <param name="scale"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public GameEffect AddEffect(string effectName, GameObject obj, Vector3 pos, float scale, float time)
    {
        if (obj == null)
            AddWorldEffect(effectName, pos, scale, time);

        GameEffect ge = GetEffect(effectName, obj.transform.position + pos);
        if (ge == null)
            return null;

        ge.SetParent(obj);
        ge.Play(scale, time);
        return ge;
    }

    GameEffect GetEffect(string effectName, Vector3 worldPos)
    {
        GameEffect ret = null;
        if (effectPool.ContainsKey(effectName))
        {
            List<GameEffect> pool = effectPool[effectName];
            for (int i = 0; i < pool.Count; i++)
            {
                GameEffect eff = pool[i];

                if (eff.gameObject.activeSelf)
                {
                    continue;
                }
                ret = eff;
                break;
            }
        }

        if (ret == null)
        {
            GameEffect eff = CreateEffect(effectName, worldPos);
            if (eff != null) ret = eff;
        }

        return ret;
    }



    public GameEffect CreateEffect(string effectName, Vector3 worldPos)
    {
        GameObject obj = Resources.Load<GameObject>("VFX/" + effectName);
        if (obj == null)
        {
            Debug.Log("can't find file ! :" + effectName);
        }
        GameObject effectObj = Instantiate(obj);
        GameEffect ge = effectObj.AddComponent<GameEffect>();
        ge.Init(effectName);
        if (!effectPool.ContainsKey(effectName))
        {
            effectPool.Add(effectName, new List<GameEffect>());
        }
        effectPool[effectName].Add(ge);
        return ge;
    }
}
