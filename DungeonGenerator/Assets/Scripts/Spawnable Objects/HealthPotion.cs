using UnityEngine;

public class HealthPotion : Item
{
    private void Awake()
    {
        IsConsumable = true;
        ConsumeAmount = Random.Range(1, 5);
    }
}
