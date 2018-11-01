
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
        protected Transform MainMessageText;
        Transform m_transform;
        public Action confirm;
        public Action cancel;
        public bool isRoot { get { return upperName == WindowName.None; } }

        private string mainMessage;
        public string MainMessage
        {
            get { return mainMessage; }
            set
            {
                mainMessage = value;
                MainMessageText.GetComponent<Text>().text = mainMessage;
            }
        }
        protected void Awake()
        {
			ConfirmButton = FindObject(transform, "Btn_Confirm");
			CancelButton[0] = FindObject(transform, "Btn_Cancel");
            CancelButton[1] = FindObject(transform, "Btn_Close");
            MainMessageText = FindObject(transform, "Text_MainMessage");
        }

		//Recursion to find child obj using index name.
		private Transform FindObject(Transform trans, string objName)
		{
			if (trans.gameObject.name == objName)
			{
				return trans;
			}
			if (trans.childCount < 1)
			{
				return null;
			}
			Transform obj = null;
			for (int i = 0; i < trans.childCount; i++)
			{
				Transform go = trans.GetChild(i);
				obj = FindObject(go, objName);
				if (obj != null)
				{
					break;
				}
			}
			return obj;
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

		private void ClearDelegate()
		{
			if(confirm != null)
			{
				Delegate[] delArray = confirm.GetInvocationList();
				for (int i = 0; i < delArray.Length; i++)
				{
					confirm -= delArray[i] as Action;
				}
			}
		}

        public virtual void Pop(WindowName _upperWindow = WindowName.None, float time = 0.5f)
        {
			ClearDelegate();
            upperName = _upperWindow;
            m_transform.DOScale(1, time);
        }
        public virtual void Close(float time = 0.8f)
        {
            m_transform.DOScale(0, time);
            if (!isRoot)
            {
                UIManager.PopWindow(upperName);
                upperName = WindowName.None;
            }
        }
        public virtual void SetUpperWindow(WindowName _upperName)
        {
            upperName = _upperName;
        }
        public virtual void ConfirmAction()
        {
            if (confirm != null) confirm();
            UIManager.CloseWindow(windowName);
        }
        public virtual void CancelAction()
        {
            if (cancel != null) cancel();
            UIManager.CloseWindow(windowName);
        }
    }
    public enum WindowName
    {
        None,
        ParentsCenter,
        MainMenu,
        SettingMenu,
        HUD,
		GenericPopup
    }
}
