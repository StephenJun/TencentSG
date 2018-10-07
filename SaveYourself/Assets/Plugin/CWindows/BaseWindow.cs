
namespace CWindow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using DG.Tweening;
    using System;
    using UnityEngine.UI;
    public class BaseWindow : MonoBehaviour
    {
        public bool locked = false;
        public WindowName windowName;
        public WindowName upperName;
        public List<WindowName> downName = new List<WindowName>();
        protected Transform ConfirmButton;
        protected Transform[] CancelButton = new Transform[2];
        protected Transform MainMessage;
        Transform m_transform;
        public Action confirm;
        public Action cancel;
        public bool isRoot { get { return upperName == WindowName.None; } }
        protected void Awake()
        {
            ConfirmButton = transform.Find("ConfirmButton");
            CancelButton[0] = transform.Find("CancelButton");
            CancelButton[1] = transform.Find("Background/Cross");
            MainMessage = transform.Find("MainMessage");
        }
        protected virtual void Start()
        {
            m_transform = transform;
            Close(0);
        }
        protected virtual void OnEnable()
        {
            if (ConfirmButton) ConfirmButton.gameObject.GetComponent<Button>().onClick.AddListener(ConfirmAction);
            if (CancelButton[0]) CancelButton[0].gameObject.GetComponent<Button>().onClick.AddListener(CancelAction);
            if (CancelButton[1]) CancelButton[1].gameObject.GetComponent<Button>().onClick.AddListener(CancelAction);
        }
        protected virtual void OnDisable()
        {
            if (ConfirmButton) ConfirmButton.gameObject.GetComponent<Button>().onClick.RemoveListener(ConfirmAction);
            if (CancelButton[0]) CancelButton[0].gameObject.GetComponent<Button>().onClick.RemoveListener(CancelAction);
            if (CancelButton[1]) CancelButton[1].gameObject.GetComponent<Button>().onClick.RemoveListener(CancelAction);
        }
        public virtual void Pop(WindowName _upperWindow = WindowName.None, float time = 0.8f)
        {
            upperName = _upperWindow;
            m_transform.DOScale(1, time);
        }
        public virtual void Close(float time = 0.8f)
        {
            m_transform.DOScale(0, time);
            if (!isRoot)
            {
                AdditionalUIManager.PopWindow(upperName);
                upperName = WindowName.None;
            }
        }
        public  virtual void SetUpperWindow(WindowName _upperName)
        {
            upperName = _upperName;
        }
        protected virtual void ConfirmAction()
        {
            if (confirm != null) confirm();
            AdditionalUIManager.CloseWindow(windowName);
        }
        protected virtual void CancelAction()
        {
            if (cancel != null) cancel();
            AdditionalUIManager.CloseWindow(windowName);
        }
    }
    public enum WindowName
    {
        None,
        ParentsCenter,

    }
}
