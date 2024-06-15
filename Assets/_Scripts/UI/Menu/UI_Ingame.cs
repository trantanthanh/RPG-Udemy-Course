using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ingame : MonoBehaviour
{
    [Header("HP bar")]
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Slider slider;

    [Header("Skill cooldown")]
    [SerializeField] Image imageDashCooldown;
    [SerializeField] Image imageParryCooldown;
    [SerializeField] Image imageBlackHoleCooldown;

    private SkillManager skills;

    // Start is called before the first frame update
    void Start()
    {
        if (playerStats != null)
        {
            playerStats.onHealthChanged += UpdateHealthUI;
        }

        skills = SkillManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SetCooldownOf(imageDashCooldown);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetCooldownOf(imageParryCooldown);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetCooldownOf(imageBlackHoleCooldown);
        }

        CheckCooldownOf(imageDashCooldown, skills.dash.cooldown);
        CheckCooldownOf(imageParryCooldown, skills.parry.cooldown);
        CheckCooldownOf(imageBlackHoleCooldown, skills.blackHole.cooldown);
    }

    private void UpdateHealthUI()
    {
        if (slider != null)
        {
            slider.maxValue = playerStats.GetMaxHealth();
            slider.value = playerStats.CurrentHealth;
        }
    }

    private void SetCooldownOf(Image _imageCooldown)
    {
        if (_imageCooldown.fillAmount <= 0)
        {
            _imageCooldown.fillAmount = 1;
        }
    }

    private void CheckCooldownOf(Image _imageCoolDown, float _cooldown)
    {
        if (_imageCoolDown.fillAmount > 0)
        {
            _imageCoolDown.fillAmount -= 1 / _cooldown * Time.deltaTime;
        }
    }
}
