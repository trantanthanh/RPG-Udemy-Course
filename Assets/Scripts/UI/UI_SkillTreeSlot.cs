using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Menu;

public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string skillName;
    [SerializeField] private int skillPrice;
    [SerializeField] private Color lockedSkillColor;
    [TextArea]
    [SerializeField] private string skillDescription;

    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBelocked;

    private UI ui;
    private Image skillImage;

    public bool unlocked = false;

    private void OnValidate()
    {
        ui = GetComponentInParent<UI>();
        gameObject.name = "SkillTreeSlot_UI - " + skillName;

        //Temporary
        //GetComponentInChildren<TextMeshProUGUI>().text = skillName;
        //GetComponentInChildren<TextMeshProUGUI>().gameObject.SetActive(false);
        //GetComponent<Image>().color = lockedSkillColor;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    private void Start()
    {
        skillImage = GetComponent<Image>();

        if (unlocked)
        {
            skillImage.color = Color.white;
        }
        else
        {
            skillImage.color = lockedSkillColor;
        }
    }

    public void UnlockSkillSlot()
    {
        if (unlocked) return;
        //Check the previous skills in skill tree are unlocked or not
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (!shouldBeUnlocked[i].unlocked)
            {
                Debug.Log("Can not unlock skill");
                return;
            }
        }

        //Check the skills in another tree need to lock when unlock this skill
        for (int i = 0; i < shouldBelocked.Length; i++)
        {
            if (shouldBelocked[i].unlocked)
            {
                Debug.Log("Can not unlock skill");
                return;
            }
        }
        if (!PlayerManager.Instance.HaveEnoughMoney(skillPrice)) return;

        unlocked = true;
        skillImage.color = Color.white;
        ui.skillToolTip.Hide();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.Show(skillName, skillDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.Hide();
    }
}
