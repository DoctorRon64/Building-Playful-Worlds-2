using UnityEngine;

public class Staff : Item
{
    private void Awake()
    {
        IsMagic = true;
        MagicDamage = Random.Range(1, 5);
    }

    public override void ItemBehaviour()
    {
        base.ItemBehaviour();
    }
}
