public class Sword : Item
{
    private void Awake()
    {
        IsConsumable = false;
    }

    public override void ItemBehaviour()
    {
        base.ItemBehaviour();
    }
}
