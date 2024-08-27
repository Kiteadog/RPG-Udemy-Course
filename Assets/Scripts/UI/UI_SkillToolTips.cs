using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillToolTips : UI_ToolTips
{
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillCost;
    [SerializeField] private float defaultNameFontSize;//记录默认字体，暂不使用

    public void ShowToolTips(string _skillDescription, string _skillName, int _price)
    {
        if (Input.GetKey(KeyCode.LeftControl))
            return;//左ctrl隐藏提示

        skillName.text = _skillName;
        skillDescription.text = _skillDescription;
        skillCost.text = "Cost: " + _price;

        AdjustPosition();
        //AdjustFontSize(skillName);

        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        //skillName.fontSize = defaultNameFontSize;//恢复默认
        gameObject.SetActive(false);
    }
}
