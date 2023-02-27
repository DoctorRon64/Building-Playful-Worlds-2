public class HealthPotion : Item
{
    private void Awake()
    {
        IsConsumable = true;
        ConsumeAmount = 10;
    }

    public override void ItemBehaviour()
    {
        base.ItemBehaviour();
    }
}
