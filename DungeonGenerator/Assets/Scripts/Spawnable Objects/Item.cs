using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public bool IsConsumable;
    public bool IsWeapon;
    public bool IsMagic;

    public int ConsumeAmount;
    public int WeaponDamage;
    public int MagicDamage;

    public Sprite icon;
    public event System.Action<Item> OnUseEvent;
    public bool isConsumable = false;

    public DungeonData dungeonData;

    public virtual void Use()
    {
        OnUseEvent?.Invoke(this);
    }
}