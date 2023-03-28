using UnityEngine;

public class Sword : Item
{
    private void Awake()
    {
        IsWeapon = true;
        AttackDamage = Random.Range(2, 7);
    }
}
