using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillTooltip : UI_Tooltip
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillCost;
    [SerializeField] private float defaultNameFontSize;

    public void Show(string _skillName, string _skillDescription, int _cost)
    {
        AdjustPosition();
        skillName.text = _skillName;
        AdjustFontSize(skillName);

        skillDescription.text = _skillDescription;
        if (PlayerManager.Instance.HaveEnoughMoney(_cost))
        {
            skillCost.text = $"Need <color=#00ff00>{_cost}</color> souls to unlock";
        }
        else
        {
            skillCost.text = $"Need <color=#ff0000>{_cost}</color> souls to unlock";
        }
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        skillName.fontSize = defaultNameFontSize;
        gameObject.SetActive(false);
    }
}
