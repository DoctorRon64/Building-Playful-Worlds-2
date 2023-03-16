using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> ItemInventory = new List<Item>();
    public DungeonData dungeonData;
    private Player playerScript;

    private void Awake()
    {
        playerScript = GetComponent<Player>();
    }

    public void IfPlayerOnItem()
    {
        for (int i = 0; i< dungeonData.ItemList.Count; i++) 
        {
            if (playerScript.MovePoint == (Vector2)dungeonData.ItemList[i].gameObject.transform.position)
            {
                ItemInventory.Add(dungeonData.ItemList[i]);
            }
        }
    }

}