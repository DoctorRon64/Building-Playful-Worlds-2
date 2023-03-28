using UnityEngine;

public class Staff : Item
{
    private void Awake()
    {
        IsWeapon = true;
        AttackDamage = Random.Range(5, 10);
        MakeItemRareder(AttackDamage);
    }

    private void MakeItemRareder(int _MagicDamage)
    {
        if (_MagicDamage >= 8)
        {
            AttackDamage = Random.Range(5, 10);
        }
    }
}
