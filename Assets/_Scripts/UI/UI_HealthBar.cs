using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity => GetComponentInParent<Entity>();
    private Slider slider => GetComponentInChildren<Slider>();
    private CharacterStats stats => GetComponentInParent<CharacterStats>();

    private void Start()
    {
        UpdateHealthBar();//update 1st times
    }

    private void UpdateHealthBar()
    {
        slider.maxValue = stats.GetMaxHealth();
        slider.value = stats.CurrentHealth;
    }

    private void FlipUI() => transform.Rotate(0, 180, 0);

    private void OnEnable()
    {
        entity.onFlipped += FlipUI;
        stats.onHealthChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;
        stats.onHealthChanged -= UpdateHealthBar;
    }
}
