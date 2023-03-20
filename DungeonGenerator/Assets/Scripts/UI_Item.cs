using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Item : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Image uiImage { get; private set; }
    public Item itemReference { get; private set; }
    public UI_ItemSlot currentSlot { get; private set; }

    [SerializeField] private DungeonData dungeonData;
    private void Awake()
    {
        uiImage = GetComponent<Image>();
    }

    public void Setup(Item item)
    {
        itemReference = item;
        itemReference.gameObject.SetActive(false);
        dungeonData.ItemList.Remove(item);
        itemReference.OnUseEvent += OnUseItem;
        uiImage.sprite = item.icon;
    }

    public void DropItem(Vector3 dropPosition)
    {
        itemReference.gameObject.SetActive(true);
        itemReference.gameObject.transform.position = dropPosition;
        dungeonData.ItemList.Add(itemReference);
        currentSlot.ReleaseItem();
        currentSlot = null;
    }

    public void SetSlot(UI_ItemSlot slot)
    {
        currentSlot = slot;
    }

    private void OnUseItem(Item usedItem)
    {
        if (usedItem.isConsumable)
        {
            currentSlot.ReleaseItem();
            Destroy(gameObject);
        }
    }

    public void Use()
    {
        itemReference.Use();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        uiImage.raycastTarget = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var list = eventData.hovered;
        UI_ItemSlot dropSlot = null;
        foreach (var v in list)
        {
            var possibleSlot = v.GetComponent<UI_ItemSlot>();
            if (possibleSlot != null)
            {
                dropSlot = possibleSlot;
                break;
            }
        }
        if (dropSlot != null)
        {
            var otherItem = dropSlot.item;
            currentSlot.ObtainItem(otherItem);
            dropSlot.ObtainItem(this);
            uiImage.raycastTarget = true;
        }
        else
        {
            //Drop the item
            Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -1f);
            Ray r = Camera.main.ScreenPointToRay(pos);
            RaycastHit2D hit = Physics2D.Raycast(r.origin, r.direction);
            if (hit == true)
            {
                Debug.Log(" raken ");
                DropItem(hit.transform.position);
                Destroy(gameObject);
            }
            else
            {
                uiImage.raycastTarget = true;
                transform.localPosition = Vector3.zero;
            }
        }
    }
}
