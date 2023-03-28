using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public bool IsConsumable;
    public bool IsWeapon;

    public int AttackDamage;
    public int ConsumeAmount;

    public Sprite icon;
    public event System.Action<Item> OnUseEvent;

    public DungeonData dungeonData;

    public virtual void Use()
    {
        OnUseEvent?.Invoke(this);
    }
}