using UnityEngine;

public class HealthPotion : Item
{
    public override void OtherAwake()
    {
        IsConsumable = true;
        ConsumeAmount = Random.Range(3, 7);
    }

    public override void Use()
    {
        PlayItemSound(this);
        player.ApplyHealth(ConsumeAmount);
    }
}
