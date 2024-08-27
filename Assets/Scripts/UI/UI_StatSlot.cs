using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;

    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea]
    [SerializeField] private string statDescription;

    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;

        if(statNameText != null)
            statNameText.text = statName;
    }

    // Start is called before the first frame update
    void Start()
    {
        ui = GetComponent<UI>();

        UpdateStatValueUI();
    }

    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if(playerStats != null)
        {
            statValueText.text = playerStats.GetStat(statType).GetValue().ToString();

            //各属性最终值计算，此处应封装于角色属性类中，暴露函数计算，后续可优化，可同时修改伤害计算相关部分
            if(statType == StatType.maxHealth)
                statValueText.text = playerStats.GetMaxHealthValue().ToString();
            if (statType == StatType.damage)
                statValueText.text = playerStats.damage.GetValue() + playerStats.strength.GetValue().ToString();
            if (statType == StatType.critPower)
                statValueText.text = playerStats.critPower.GetValue() + playerStats.strength.GetValue().ToString();
            if (statType == StatType.critChance)
                statValueText.text = playerStats.critChance.GetValue() + playerStats.agility.GetValue().ToString();
            if (statType == StatType.evasion)
                statValueText.text = playerStats.evasion.GetValue() + playerStats.agility.GetValue().ToString();
            if (statType == StatType.magicResistance)
                statValueText.text = (playerStats.magicResistance.GetValue() + playerStats.intelligence.GetValue() * 3).ToString();
        }
    }

    public void OnPointerExit(PointerEventData eventData)//鼠标移开隐藏
    {
        ui.statToolTips.HideToolTips();
    }

    public void OnPointerEnter(PointerEventData eventData)//鼠标指向显示
    {
        ui.statToolTips.ShowToolTips(statDescription);
    }
}
