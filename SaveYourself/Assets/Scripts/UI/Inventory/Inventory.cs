using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

	public Action timerComplete;

    [HideInInspector]
    public Slot[] slotArray;

    [SerializeField]
    private GameObject interactingTimer;
    [SerializeField]
    private Image interactingTimerPic;
    [SerializeField]
    private Text interactingTimerText;

	private Image checkBox;
	private int currentSlotIndex = 0;
	private int smothing = 8;

    private void Start()
    {
        slotArray = GetComponentsInChildren<Slot>();
		checkBox = transform.Find("Img_CheckBox").GetComponent<Image>();
    }

	private void Update()
	{
		checkBox.rectTransform.position = Vector3.Lerp(checkBox.transform.position, slotArray[currentSlotIndex].transform.position, smothing * Time.deltaTime);
	}

	public InteractiveObject EquippedItem()
	{
		ItemUI item = slotArray[currentSlotIndex].GetComponentInChildren<ItemUI>();
		if(item == null)
		{
			return null;
		}
		InteractiveObject equipped = item.interObj;
		return equipped;
	}
    /// <summary>
    /// Show the progression of collecting the item
    /// </summary>
    /// <param name="item">item to collect</param>
    public void ShowTimerWhenInteracting(float time, Action OnComplete, Transform obj)
    {
        StartCoroutine(ShowTimerCoroutine(time, OnComplete, obj));
    }
    private IEnumerator ShowTimerCoroutine(float time, Action OnComplete, Transform obj)
    {
		if(timerComplete != null)
		{
			Delegate[] delArray = timerComplete.GetInvocationList();
			for (int i = 0; i < delArray.Length; i++)
			{
				timerComplete -= delArray[i] as Action;
			}
		}
        float initDur = time;
        interactingTimer.SetActive(true);
        while (initDur > 0)
        {
            yield return null;
            initDur -= Time.deltaTime;
            interactingTimerText.text = initDur.ToString("0.0") + " / " + time.ToString("0.0");
			interactingTimerPic.fillAmount = initDur / time;
			if (Vector3.Distance(obj.position, PlayerController.Instance.transform.position) > 4.0f)
			{
				interactingTimer.SetActive(false);
				yield break;
			}
        }
        interactingTimer.SetActive(false);
		if(OnComplete != null)
		{
			OnComplete();
		}
    }


	//Add Item to slot base on name
    public bool AddItem(string itemName)
    {
        InteractiveObject item = InventoryManager.Instance.GetItemByName(itemName);
        return AddItem(item);
    }
    public bool AddItem(InteractiveObject item)
    {
		if (item == null)
        {
            Debug.Log("你增加的物品ID不存在");
            return false;
        }
        if (item.maxStack == 1) //当物品所允许的叠加数量为1时，给这个物品找一个空的物品槽
        {
            Slot emptySlot = FindEmptySlot(item);
            if (emptySlot == null)
            {
                Debug.Log("物品栏中的槽已经满了，不能再增加物品了");
                return false;
            }
            else
            {
                emptySlot.AddItem(item);
			}
        }
        else
        {
            Slot slot = FindSameTypeSlot(item);
            if (slot != null)
            {
                slot.AddItem(item);
            }
            else
            {
                Slot emptySlot = FindEmptySlot(item);
                if (emptySlot != null)
                {
                    emptySlot.AddItem(item);
                }
                else
                {
                    Debug.Log("物品栏中的槽已经满了，不能再增加物品了");
                    return false;
                }
            }
        }
		return true;
    }
	public void SwitchItem()
	{
		currentSlotIndex++;
		if (currentSlotIndex == slotArray.Length)
		{
			currentSlotIndex = 0;
		}
	}

	public ItemUI CheckIfHasTypeOfItem<T>()
	{
		foreach(var slot in slotArray)
		{
			if(slot.transform.childCount != 0)
			{
				if(slot.transform.GetChild(0).GetComponent<ItemUI>().interObj is T)
				{
					return slot.transform.GetChild(0).GetComponent<ItemUI>();
				}
			}
		}
		return null;
	}
	

	private Slot FindEmptySlot(InteractiveObject item)
    {
        foreach (Slot slot in slotArray)
        {
            if (slot.transform.childCount < 1)
            {
                    return slot;
            }
        }
        return null;
    }
    private Slot FindSameTypeSlot(InteractiveObject item)
    {
        foreach (Slot slot in slotArray)
        {
            if ((slot.transform.childCount >= 1) && slot.GetThisItemName() == item.itemName && slot.IsFull() == false)
            {
                return slot;
            }
        }
        return null;
    }


	public void SaveInventory() //频繁的更改字符用stringbuilder,可以提高内存分配效率
    {
        StringBuilder sb = new StringBuilder();
        foreach (Slot slot in slotArray)
        {
            if (slot.transform.childCount > 0)
            {
                ItemUI itemUI = slot.transform.GetChild(0).GetComponent<ItemUI>();
                sb.Append(itemUI.interObj.itemName + "," + itemUI.Amount + "-");
            }
            else
            {
                sb.Append("0-");
            }
        }
        PlayerPrefs.SetString(this.gameObject.name, sb.ToString());
    }
    public void LoadInventory()
    {
        if (PlayerPrefs.HasKey(this.gameObject.name))
        {
            string str = PlayerPrefs.GetString(this.gameObject.name);
            string[] itemArray = str.Split('-');
            for (int i = 0; i < itemArray.Length - 1; i++)
            {
                string s = itemArray[i];
                if (s != "0")
                {
                    string[] temp = s.Split(',');
                    string name = temp[1];
                    int amount = int.Parse(temp[1]);
                    InteractiveObject itemInfo = InventoryManager.Instance.GetItemByName(name);
                    slotArray[i].AddItem(itemInfo);
                    slotArray[i].GetComponentInChildren<ItemUI>().SetAmount(amount);
                }
            }
        }
    }
}
