using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    protected UI ui;
    public InventoryItem item;

    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;
        itemImage.color = Color.white;
        itemText.faceColor = Color.white;

        itemImage.sprite = item.data.icon;


        if (item.stackSize > 1)
        {
            itemText.text = item.stackSize.ToString();
        }
        else
        {
            itemText.text = "";
        }
    }

    public void CleanUpSlot()
    {
        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
            return;

        if(Input.GetKey(KeyCode.LeftControl))//左ctrl点击丢弃
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }

        if (item.data.itemType == ItemType.Equipment)//点击装备
            Inventory.instance.EquipItem(item.data);

        ui.itemToolTips.HideToolTips();//隐藏说明
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null) 
            return;
 
        ui.itemToolTips.ShowToolTips(item.data as ItemData_Equipment);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null)
            return;

        ui.itemToolTips.HideToolTips();
    }
}
