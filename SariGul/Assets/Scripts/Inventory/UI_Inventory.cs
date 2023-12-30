using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    public Player player;

    private void Awake()
    {
        itemSlotContainer = transform.Find("itemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
    }

    private void Update()
    {
        List<Item> itemList = inventory.GetItemList();
        if (Input.GetKeyDown(KeyCode.Alpha1) && itemList != null && itemList.Count > 0)
        {
            Item firstItem = new Item();
            firstItem = itemList[0];
            if (firstItem != null)
                player.UseItem(firstItem);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && itemList != null && itemList.Count > 1)
        {
            Item secItem = new Item();
            secItem = itemList[1];
            if (secItem != null)
                player.UseItem(secItem);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && itemList != null && itemList.Count > 2)
        {
            Item thrdItem = new Item(); 
            thrdItem = itemList[2];
            if (thrdItem != null)
                player.UseItem(thrdItem);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && itemList != null && itemList.Count > 3)
        {
            Item frtItem = new Item(); 
            frtItem = itemList[3];
            if(frtItem != null)
                player.UseItem(frtItem);
        }
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;

        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        foreach (Transform child in itemSlotContainer){
            if (child == itemSlotTemplate) continue;
            else
            {
                Destroy(child.gameObject);
            }
        }
        int count = 0;
        int x = 0;
        int y = 0;
        float itemSlotCellSize = 47f;
        foreach (Item item in inventory.GetItemList())
        {
            count += 1;
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            Image image = itemSlotRectTransform.Find("image").GetComponent <Image>();
            image.sprite = item.GetSprite();
            TextMeshProUGUI uitext = itemSlotRectTransform.Find("amountText").GetComponent<TextMeshProUGUI>();
            if (item.amount > 1)
            {
                uitext.SetText(item.amount.ToString());
            }
            else
            {
                uitext.SetText("");
            }
            TextMeshProUGUI indexText = itemSlotRectTransform.Find("useText").GetComponent<TextMeshProUGUI>();
            indexText.SetText(count.ToString());
            x++;
            if (x > 4)
            {
                x = 0;
                y++;
            }
        }
    }

}
