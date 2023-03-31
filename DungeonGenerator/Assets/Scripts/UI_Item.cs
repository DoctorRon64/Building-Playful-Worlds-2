using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UI_Item : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image uiImage { get; private set; }
    public Item itemReference { get; private set; }
    public UI_ItemSlot currentSlot { get; private set; }

    private InventoryManager Inventory;
    [SerializeField] private DungeonData dungeonData;
    [SerializeField] private LayerMask OnlyFloors;
    private GameObject EnemieHitHud;
    
    public GameObject HoverText;
    public Text[] HoverTextUI = new Text[3];

    private void Awake()
    {
        Inventory = GetComponent<InventoryManager>();
        uiImage = GetComponent<Image>();
    }

    public void Setup(Item item)
    {
        itemReference = item;
        itemReference.gameObject.SetActive(false);
        dungeonData.ItemList.Remove(item);
        uiImage.sprite = item.icon;

        HoverText.SetActive(false);
        EnemieHitHud = FindAnyObjectByType<Player>().EnemieHitHud;
        EnemieHitHud.SetActive(false);

        for (int i = 0; i < HoverTextUI.Length; i++)
        {
            HoverTextUI[0].text = itemReference.name;
            HoverTextUI[1].text = itemReference.ConsumeAmount.ToString();
            HoverTextUI[2].text = itemReference.AttackDamage.ToString();
        }
    }

    public void DropItem(Vector3 _dropPosition)
    {
        itemReference.gameObject.SetActive(true);
        itemReference.gameObject.transform.position = _dropPosition;
        EnemieHitHud?.SetActive(false);
        dungeonData.ItemList.Add(itemReference);
        currentSlot.ReleaseItem();
        currentSlot = null;
    }

    public void SetSlot(UI_ItemSlot slot)
    {
        currentSlot = slot;
    }

    public void Use()
    {
        itemReference.Use();
        currentSlot.ReleaseItem();
        currentSlot = null;
        itemReference = null;
        Destroy(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverText?.SetActive(true);
        if (itemReference.IsWeapon)
		{
            EnemieHitHud?.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HoverText?.SetActive(false);
        EnemieHitHud?.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnPointerDown(PointerEventData eventData)
	{
        if (Input.GetMouseButtonDown(1))
		{
            Use();
            EnemieHitHud.SetActive(false);
        }
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
            RaycastHit2D hit = Physics2D.Raycast(r.origin, r.direction, Mathf.Infinity, OnlyFloors);
            if (hit == true && detectItemOnItemDrop(hit.transform.position, dungeonData.ItemList) == false && detectItemOnItemDrop(hit.transform.position, dungeonData.EnemyList) == false) 
            {
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

    private bool detectItemOnItemDrop(Vector3 _dropPosition, List<Item> _list)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            if (_dropPosition == _list[i].transform.position)
            {
                return true;
            }
        }
        return false;
    }

    private bool detectItemOnItemDrop(Vector3 _dropPosition, List<Enemy> _list)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            if (_dropPosition == _list[i].transform.position)
            {
                return true;
            }
        }
        return false;
    }
}
