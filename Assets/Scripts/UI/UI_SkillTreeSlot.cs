using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour
{
    [SerializeField] private string skillName;
    [SerializeField] private Color lockedSkillColor;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Image skillImage;

    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBelocked;

    public bool unlocked = false;

    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - " + skillName;

        //Temporary
        //GetComponentInChildren<TextMeshProUGUI>().text = skillName;
        //GetComponentInChildren<TextMeshProUGUI>().gameObject.SetActive(false);
        //GetComponent<Image>().color = lockedSkillColor;
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

        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    public void UnlockSkillSlot()
    {
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

        unlocked = true;
        skillImage.color = Color.white;
    }
}
