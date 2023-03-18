using UnityEngine;

public class Sword : Item
{
    private void Awake()
    {
        IsWeapon = true;
        WeaponDamage = Random.Range(1, 5);
    }
}
