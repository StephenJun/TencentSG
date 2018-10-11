using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    [HideInInspector]
    public Slot[] slotArray;

    [SerializeField]
    private GameObject interactingTimer;
    [SerializeField]
    private Image interactingTimerPic;
    [SerializeField]
    private Text interactingTimerText;
    private void Start()
    {
        slotArray = GetComponentsInChildren<Slot>();
       
    }


    /// <summary>
    /// Show the progression of collecting the item
    /// </summary>
    /// <param name="item">item to collect</param>
    public void ShowTimerWhenInteracting(InteractiveObject item)
    {
        StartCoroutine(ShowTimerCoroutine(item));
    }

    private IEnumerator ShowTimerCoroutine(InteractiveObject item)
    {
        float initDur = item.timeNeedToCollect;
        print(1);
        interactingTimer.SetActive(true);
        while (initDur > 0)
        {
            yield return null;
            initDur -= Time.deltaTime;
            interactingTimerText.text = initDur.ToString("0.0") + " / " + item.timeNeedToCollect.ToString("0.0");
            interactingTimerPic.fillAmount = initDur / item.timeNeedToCollect;
        }
        interactingTimer.SetActive(false);
        AddItem(item.itemName);
        Destroy(item.gameObject);
    }

    public bool AddItem(string itemName)
    {
        InteractiveObject item = InventoryManager.Instance.GetItemByName(itemName);
        return AddItem(item);
    }
    public bool AddItem(InteractiveObject item)
    {
        //if()
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

    //public void SetDescription(InteractiveObject item)
    //{
    //    txtItemName.text = item.icon;
    //    txtTooltip.text = item.tooltip;
    //    txtDescription.text = item.description;
    //    itemPicture.sprite = Resources.Load<Sprite>("Items/" + item.icon);
    //}


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
