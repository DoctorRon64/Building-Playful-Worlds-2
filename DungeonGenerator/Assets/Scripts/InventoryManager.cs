using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public UI_Item UI_ItemPrefab;
    public List<UI_ItemSlot> slots = new List<UI_ItemSlot>();

    public DungeonData dungeonData;

    private void Start()
    {
        GetComponentsInChildren<UI_ItemSlot>(slots);
    }

    public void PickupItem(Item item)
    {
        var emptySlot = slots.Find(x => x.item == null);
        if(emptySlot != null)
        {
            UI_Item obj = Instantiate(UI_ItemPrefab);
            obj.Setup(item);
            emptySlot.ObtainItem(obj);
        }
        else
        {
            Debug.Log("Inventory is Full!");
        }
    }
}