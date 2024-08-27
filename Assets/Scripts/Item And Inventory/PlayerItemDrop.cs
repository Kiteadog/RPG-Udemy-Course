using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("player's Drop")]
    [SerializeField] private float chanceToLoseItems;
    [SerializeField] private float chanceToLoseMaterials;

    //����
    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;

        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();
        List<InventoryItem> materialsToUnequip = new List<InventoryItem>();

        //����Ƿ����
        foreach (InventoryItem item in Inventory.instance.GetEquipmentList())//����װ���嵥
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
