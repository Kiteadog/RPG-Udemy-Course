using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemToolTips : UI_ToolTips
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;

    public void ShowToolTips(ItemData_Equipment item)
    {
        itemNameText.text = item.itemName;
        itemTypeText.text = item.equipmentType.ToString();
        itemDescription.text = item.GetDescription();

        /*itemNameText.fontSize = itemNameText.text.Length > 12 ?24 :32;//³¬¹ý12×ÖÄ¸ËõÐ¡×ÖÌå*/
        AdjustPosition();


        gameObject.SetActive(true);
    }

    public void HideToolTips() => gameObject.SetActive(false);
}
