using UnityEngine;

public class Staff : Item
{
    public override void OtherAwake()
    {
        IsWeapon = true;
        IsConsumable = true;
        AttackDamage = Random.Range(5, 10);
        ConsumeAmount = Random.Range(0, 5);
        MakeItemRareder(AttackDamage);
    }

    private void MakeItemRareder(int _MagicDamage)
    {
        if (_MagicDamage >= 8)
        {
            AttackDamage = Random.Range(5, 10);
        }
    }

    public override void Use()
    {
        player.DoDamage(AttackDamage);
        player.ApplyHealth(ConsumeAmount);
    }
}
