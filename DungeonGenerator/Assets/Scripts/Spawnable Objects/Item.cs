using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public bool IsConsumable;
    public bool IsWeapon;
    public bool IsStaff;

    public int AttackDamage = 0;
    public int ConsumeAmount = 0;

    public Sprite icon;
    public DungeonData dungeonData;
    public Player player;
    public SoundEffectsPlayer sfxPlayer;

    public void Awake()
	{
		player = FindObjectOfType<Player>();
        sfxPlayer = player.GetComponent<SoundEffectsPlayer>();
        OtherAwake();
    }

    public virtual void OtherAwake()
	{

	}

	public virtual void Use()
    {
        
    }

    public void PlayItemSound(Item _item)
	{
        if (_item.GetComponent<HealthPotion>())
        {
            sfxPlayer.PlayAudio(5);
        }
        else if (_item.GetComponent<Staff>())
        {
            sfxPlayer.PlayAudio(6);
        }
        else if (_item.GetComponent<Sword>())
        {
            sfxPlayer.PlayAudio(7);
        }
    }
}