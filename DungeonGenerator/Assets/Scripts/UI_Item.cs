using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UI_Item : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Image uiImage { get; private set; }
    public Item itemReference { get; private set; }
    public UI_ItemSlot currentSlot { get; private set; }

    private InventoryManager Inventory;
    [SerializeField] private DungeonData dungeonData;
    [SerializeField] private LayerMask OnlyFloors;
    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        Inventory = GetComponent<InventoryManager>();
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

    public void DropItem(Vector3 _dropPosition)
    {
        itemReference.gameObject.SetActive(true);
        itemReference.gameObject.transform.position = _dropPosition;
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
        if (usedItem.IsConsumable)
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
            RaycastHit2D hit = Physics2D.Raycast(r.origin, r.direction, Mathf.Infinity, OnlyFloors);
            if (hit == true)
            {
                if (detectItemOnItemDrop(hit.transform.position, dungeonData.ItemList) == true)
                {
                    
                }
                else if (detectEnemyOnItemDrop(hit.transform.position, dungeonData.EnemyList) == true && itemReference.IsWeapon == true)
                {
                    AttackEnemy(hit.transform.position);
                }
                else if (detectEnemyOnItemDrop(hit.transform.position, dungeonData.EnemyList) == true && itemReference.IsConsumable == true)
                {
                    LoveEnemy(hit.transform.position);
                }
                else if (detectPlayerOnItemDrop(hit.transform.position) == true && itemReference.IsConsumable == true)
                {
                    LovePlayer(hit.transform.position);
                } 
                else
                {
                    DropItem(hit.transform.position);
                }
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

    private bool detectEnemyOnItemDrop(Vector3 _dropPosition, List<Enemy> _list)
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

    private bool detectPlayerOnItemDrop(Vector3 _dropPosition)
    {
        if (player.transform.position == _dropPosition)
        {
            return true;
        }
        return false;
    }

    private void AttackEnemy(Vector3 _dropPosition)
    {
        for (int i = 0; i < dungeonData.EnemyList.Count; i++)
        {
            if (dungeonData.EnemyList[i].transform.position == _dropPosition && itemReference.IsWeapon == true)
            {
                Debug.Log("attack");
                dungeonData.EnemyList[i].GetComponent<Enemy>().TakeDamage(itemReference.AttackDamage);
                Destroy(itemReference);
                currentSlot.ReleaseItem();
                currentSlot = null;
            }
        }
    }

    private void LoveEnemy(Vector3 _dropPosition)
    {
        for (int i = 0; i < dungeonData.EnemyList.Count; i++)
        {
            if (dungeonData.EnemyList[i].transform.position == _dropPosition && itemReference.IsConsumable == true)
            {
                Debug.Log("love is in the air");
                dungeonData.EnemyList[i].GetComponent<Enemy>().ApplyHealth(itemReference.ConsumeAmount);
                Destroy(itemReference);
                currentSlot.ReleaseItem();
                currentSlot = null;
            }
        }
    }

    private void LovePlayer(Vector3 _dropPosition)
    {
        if (player.transform.position == _dropPosition && itemReference.IsConsumable == true)
        {
            player.ApplyHealth(itemReference.ConsumeAmount);
            Destroy(itemReference);
            currentSlot.ReleaseItem();
            currentSlot = null;
        }
    }
}
