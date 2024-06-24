using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Menu;

public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
{
    [SerializeField] private string skillName;
    [SerializeField] private int skillCost;
    [SerializeField] private Color lockedSkillColor;
    [TextArea]
    [SerializeField] private string skillDescription;

    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBelocked;

    private UI ui;
    private Image skillImage;

    public bool unlocked = false;
    public System.Action onUpgradeSkill;

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
        skillImage = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    private void OnEnable()
    {
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
        if (!PlayerManager.Instance.HaveEnoughMoney(skillCost)) return;
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

        SoundManager.Instance.PlaySFX(SFXDefine.sfx_click);

        unlocked = true;
        skillImage.color = Color.white;
        ui.skillToolTip.Hide();
        onUpgradeSkill?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.Show(skillName, skillDescription, skillCost);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.Hide();
    }

    public void LoadData(GameData _data)
    {
        //Debug.Log("Skill tree loaded");
        if (_data.skillTree.TryGetValue(skillName, out bool value))
        {
            unlocked = value;
            //if (unlocked)
            //{
            //    onUpgradeSkill?.Invoke();
            //}
        }
    }

    public void SaveData(ref GameData _data)
    {
        Debug.Log("Skill tree saved");
        if (_data.skillTree.TryGetValue(skillName, out bool value))
        {
            _data.skillTree.Remove(skillName);
            _data.skillTree.Add(skillName, unlocked);
        }
        else
            _data.skillTree.Add(skillName, unlocked);
    }
}
