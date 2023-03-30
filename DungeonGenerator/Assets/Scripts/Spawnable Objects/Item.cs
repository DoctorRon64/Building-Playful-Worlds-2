using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public bool IsConsumable;
    public bool IsWeapon;

    public int AttackDamage = 0;
    public int ConsumeAmount = 0;

    public Sprite icon;
    public DungeonData dungeonData;
    public Player player;

    public void Awake()
	{
		player = FindObjectOfType<Player>();
        OtherAwake();
    }

    public virtual void OtherAwake()
	{

	}

	public virtual void Use()
    {
        
    }
}