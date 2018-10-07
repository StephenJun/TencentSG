using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using CWindow;

public class AdditionalUIManager : MonoBehaviour
{
    static public AdditionalUIManager Instance;
    static public Dictionary<WindowName, BaseWindow> WindowIndex = new Dictionary<WindowName, BaseWindow>();
    public Action<WWW> process;
    [SerializeField]
    List<RectTransform> WindowRectBank = new List<RectTransform>();
    [SerializeField]
    Text[] debugInfo;
    [SerializeField]
    Image blackCurtain;

    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
        int i = -1;
        foreach (var windowName in Enum.GetValues(typeof(WindowName)))
        {
            //Init window dictionary
            if (i == -1) { i++; continue; }
            WindowIndex[(WindowName)windowName] = WindowRectBank[i].GetComponent<BaseWindow>();
            i++;
        }
    }
    void Start()
    {
        //huTui.color -= new Color(0, 0, 0, 1);
        //huTui.DOFade(1, 1).SetDelay(6);
        //debugInfo[2].text = DownloadVideo.GetPathByPlatform();
    }
    static public void PopMessage(string info = "2333", int i = 0)
    {
        //DOTween.Kill(Instance.debugInfo[i].gameObject);
        Instance.debugInfo[i].transform.localScale = Vector3.one;
        Instance.debugInfo[i].transform.DOPunchScale(Vector3.one * 1.05f, 0.2f, 1).OnComplete(() => Instance.debugInfo[i].transform.localScale = Vector3.one);
        Instance.debugInfo[i].text = info;

    }

    /// <summary>
    /// Pop the window with name windowName and do sth with background raycast target
    /// </summary>
    /// <param name="windowName"></param>
    /// <param name="upperName"></param>
    static public void PopWindow(WindowName windowName, WindowName upperName = WindowName.None)
    {
        if (WindowIndex[windowName].locked)
            return;
        WindowIndex[windowName].Pop(upperName);
        if (upperName == WindowName.None)
        {
            Instance.blackCurtain.DOFade(0.5f, 0.8f);
            Instance.blackCurtain.raycastTarget = true;
        }
    }
    static public void CloseWindow(WindowName windowName)
    {
        if (WindowIndex[windowName].isRoot)
        {
            Instance.blackCurtain.DOFade(0, 0.8f);
            Instance.blackCurtain.raycastTarget = false;
        }
        WindowIndex[windowName].Close();
        WindowIndex[windowName].locked = true;
        DOVirtual.DelayedCall(0.8f, () => WindowIndex[windowName].locked = false);
    }


    #region Debug
    [ContextMenu("Show Dictionary")]
    void ShowDictionary()
    {
        foreach (var window in WindowIndex)
        {
            Debug.Log(window.Value);
        }

    }

    #endregion



}
