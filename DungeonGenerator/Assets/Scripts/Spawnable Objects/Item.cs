using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public bool IsConsumable;
    public bool IsWeapon;
    public bool IsMagic;

    public int ConsumeAmount;
    public int WeaponDamage;
    public int MagicDamage;

    public DungeonData dungeonData;

    public void ItemPickUped()
    {
        gameObject.SetActive(false);
    }
}