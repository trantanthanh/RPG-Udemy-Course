using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI description;

    public void ShowStatToolTip(string text)
    {
        if (text.Length == 0) return;
        description.text = text;
        gameObject.SetActive(true);
    }

    public void HideStatTooltip() => gameObject.SetActive(false);
}
