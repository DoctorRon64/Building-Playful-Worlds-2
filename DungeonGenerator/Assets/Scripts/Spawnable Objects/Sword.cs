using UnityEngine;

public class Sword : Item
{
    public override void OtherAwake()
	{
        IsWeapon = true;
        AttackDamage = Random.Range(2, 7);
    }

    public override void Use()
    {
        PlayItemSound(this);
        player.DoDamage(AttackDamage);
    }
}
