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

            //����������ֵ���㣬�˴�Ӧ��װ�ڽ�ɫ�������У���¶�������㣬�������Ż�����ͬʱ�޸��˺�������ز���
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

    public void OnPointerExit(PointerEventData eventData)//����ƿ�����
    {
        ui.statToolTips.HideToolTips();
    }

    public void OnPointerEnter(PointerEventData eventData)//���ָ����ʾ
    {
        ui.statToolTips.ShowToolTips(statDescription);
    }
}
