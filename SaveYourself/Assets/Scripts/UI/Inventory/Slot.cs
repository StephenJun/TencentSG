using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour {

    [SerializeField] private GameObject itemUIObject;

    public void AddItem(InteractiveObject item)
    {
        //if (transform.childCount == 0)
        //{
        //	GameObject go = Instantiate(itemObject) as GameObject;
        //	go.transform.SetParent(this.transform, false); //false表示是作为UI的子物体，不是在场景中worldPositionStay
        //	go.GetComponent<Item>().SetIcon(item);
        //}
        //else
        //{
        //	transform.GetChild(0).GetComponent<Item>().AddAmount();
        //}
        GameObject go = Instantiate(itemUIObject) as GameObject;
        go.transform.SetParent(this.transform, false); //false表示是作为UI的子物体，不是在场景中worldPositionStay
        go.GetComponent<ItemUI>().SetIcon(item);

    }

    public string GetThisItemName()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().interObj.itemName;
    }

    public bool IsFull()
    {
        ItemUI itemUI = transform.GetChild(0).GetComponent<ItemUI>();
        if (itemUI.Amount >= itemUI.interObj.maxStack)
        {
            return true;
        }
        return false;
    }

    public void DiscardItem(InteractiveObject item)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (item.itemName == transform.GetChild(i).GetComponent<ItemUI>().interObj.itemName)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

    }
}
