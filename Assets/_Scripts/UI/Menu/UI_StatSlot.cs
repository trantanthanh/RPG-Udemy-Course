using Menu;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private StatType statType;
    [SerializeField] private string statName;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea]
    [SerializeField] private string statDescription;

    private UI ui;

    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;
        if (statNameText != null)
        {
            statNameText.text = statName;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateStatValueUI();
        ui = GetComponentInParent<UI>();
    }

    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            statValueText.text = playerStats.GetFinalValueStat(statType).ToString();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statTooltip.Show(statDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statTooltip.Hide();
    }
}
