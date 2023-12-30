using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType
    {
        HealthPotion,
        Fireball,
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.HealthPotion: return ItemAssets.Instance.healthPotionSprite;
            case ItemType.Fireball:     return ItemAssets.Instance.fireballSprite;
        }
    }

    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.HealthPotion:
            case ItemType.Fireball:
                return true;

        }
    }
}
