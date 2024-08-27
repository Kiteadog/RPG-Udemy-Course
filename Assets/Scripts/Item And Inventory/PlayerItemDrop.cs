using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("player's Drop")]
    [SerializeField] private float chanceToLoseItems;
    [SerializeField] private float chanceToLoseMaterials;

    //掉落
    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;

        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();
        List<InventoryItem> materialsToUnequip = new List<InventoryItem>();

        //检测是否掉落
        foreach (InventoryItem item in Inventory.instance.GetEquipmentList())//遍历装备清单
        {
            if (Random.Range(0, 100) <= chanceToLoseItems)
            {
                DropItem(item.data);
                itemsToUnequip.Add(item);
            }
        }

        for (int i = 0; i < itemsToUnequip.Count; i++)
        {
            inventory.UnequipItem(itemsToUnequip[i].data as ItemData_Equipment);
        }
                
        foreach (InventoryItem item in Inventory.instance.GetStashList())
        {
            if (Random.Range(0, 100) <= chanceToLoseMaterials)
            {
                DropItem(item.data);
                materialsToUnequip.Add(item);
            }
        }

        for (int i = 0; i < materialsToUnequip.Count; i++)
        {
            inventory.RemoveItem(materialsToUnequip[i].data);
        }
    }
}
