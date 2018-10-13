using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager> {

    public List<InteractiveObject> CollectableItemsList;

    public Inventory inventory;


    //TODO
    void ParseItemJson()
    {

    }
    public void AddItemToList(InteractiveObject io)
    {
        CollectableItemsList.Add(io);
    }

    public InteractiveObject GetItemByName(string itemName)
    {
        foreach (InteractiveObject item in CollectableItemsList)
        {
            if (item.itemName == itemName)
            {
                return item;
            }
        }
        return null;
    }
}
