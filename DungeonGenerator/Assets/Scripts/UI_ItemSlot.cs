using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour
{
    public UI_Item item;
    public void ObtainItem(UI_Item item)
    {
        if(item == null)
        {
            this.item = null;
            return;
        }
        this.item = item;
        item.transform.SetParent(transform);
        item.transform.SetAsLastSibling();
        item.transform.localPosition = Vector3.zero;
        item.SetSlot(this);
    }

    public void ReleaseItem()
    {
        item = null;
    }

    public void UseItem()
    {
        item.Use();
    }
} 
