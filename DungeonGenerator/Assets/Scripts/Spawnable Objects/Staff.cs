using UnityEngine;

public class Staff : Item
{
    private void Awake()
    {
        IsMagic = true;

        MagicDamage = Random.Range(1, 5);
        MakeItemRareder(MagicDamage);
    }

    private void MakeItemRareder(int _MagicDamage)
    {
        if (_MagicDamage >= 4)
        {
            MagicDamage = Random.Range(1, 5);
        }
    }

    public override void ItemBehaviour()
    {
        base.ItemBehaviour();
    }
}
