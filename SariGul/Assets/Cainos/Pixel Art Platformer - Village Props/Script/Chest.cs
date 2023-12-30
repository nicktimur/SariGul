using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.LucidEditor;

namespace Cainos.PixelArtPlatformer_VillageProps
{
    public class Chest : MonoBehaviour
    {
        public string itemType;

        public Item GetItem()
        {
            if (itemType == "Health")
            {
                return new Item { itemType = Item.ItemType.HealthPotion, amount = 1 };
            }
            else if (itemType == "Fireball")
            {
                return new Item { itemType = Item.ItemType.Fireball, amount = 1 };
            }
            return null;
        }
    }
}
