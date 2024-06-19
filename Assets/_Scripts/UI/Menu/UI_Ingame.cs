using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] Image imageCrystalCooldown;
    [SerializeField] Image imageSwordCooldown;
    [SerializeField] Image imageFlaskCooldown;

    [SerializeField] TextMeshProUGUI currentSouls;

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
        int currency = PlayerManager.Instance.GetCurrency();
        currentSouls.text = currency == 0 ? "0" : currency.ToString("#,#");
        UpdateCheckCooldown();
    }

    private void UpdateCheckCooldown()
    {
        CheckCooldownOf(imageDashCooldown, skills.dash.cooldown);
        CheckCooldownOf(imageParryCooldown, skills.parry.cooldown);
        CheckCooldownOf(imageBlackHoleCooldown, skills.blackHole.cooldown);
        CheckCooldownOf(imageCrystalCooldown, skills.crystal.cooldown);
        CheckCooldownOf(imageSwordCooldown, skills.swordThrow.cooldown);
        CheckCooldownOf(imageFlaskCooldown, InventoryManager.Instance.FlaskCooldown);
    }

    public void SetDashCooldown() => SetCooldownOf(imageDashCooldown);
    public void SetParryCooldown() => SetCooldownOf(imageParryCooldown);
    public void SetBlackHoleCooldown() => SetCooldownOf(imageBlackHoleCooldown);
    public void SetCrystalCooldown() => SetCooldownOf(imageCrystalCooldown);
    public void SetSwordThrowCooldown() => SetCooldownOf(imageSwordCooldown);
    public void SetFlaskCooldown() => SetCooldownOf(imageFlaskCooldown);

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
