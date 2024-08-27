using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ItemData data;
    public int stackSize;//¿â´æÊý

    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        //Ìí¼Ó¿â´æ
        AddStack();
    }
    
    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
