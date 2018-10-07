using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using CWindow;

public class UIManager : Manager<UIManager>
{
    [SerializeField]
    List<RectTransform> WindowRectBank = new List<RectTransform>();
    static public Dictionary<WindowName, BaseWindow> WindowIndex = new Dictionary<WindowName, BaseWindow>();
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
