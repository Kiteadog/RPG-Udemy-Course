using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatToolTips : UI_ToolTips
{
    [SerializeField] private TextMeshProUGUI statDescription;

    public void ShowToolTips(string _text)
    {
        statDescription.text = _text;
        AdjustPosition();

        gameObject.SetActive(true);
    }

    public void HideToolTips()
    {
        statDescription.text = "";

        gameObject.SetActive(false);
    } 
    
}
