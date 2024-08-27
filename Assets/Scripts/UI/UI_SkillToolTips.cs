using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillToolTips : UI_ToolTips
{
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillCost;
    [SerializeField] private float defaultNameFontSize;//��¼Ĭ�����壬�ݲ�ʹ��

    public void ShowToolTips(string _skillDescription, string _skillName, int _price)
    {
        if (Input.GetKey(KeyCode.LeftControl))
            return;//��ctrl������ʾ

        skillName.text = _skillName;
        skillDescription.text = _skillDescription;
        skillCost.text = "Cost: " + _price;

        AdjustPosition();
        //AdjustFontSize(skillName);

        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        //skillName.fontSize = defaultNameFontSize;//�ָ�Ĭ��
        gameObject.SetActive(false);
    }
}
