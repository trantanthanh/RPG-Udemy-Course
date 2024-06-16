using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillTooltip : UI_Tooltip
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private float defaultNameFontSize;

    public void Show(string _skillName, string _skillDescription)
    {
        AdjustPosition();
        skillName.text = _skillName;
        AdjustFontSize(skillName);

        skillDescription.text = _skillDescription;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        skillName.fontSize = defaultNameFontSize;
        gameObject.SetActive(false);
    }
}
