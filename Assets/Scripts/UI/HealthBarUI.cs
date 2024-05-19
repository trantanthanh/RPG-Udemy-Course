using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    private Entity entity;
    private Slider slider;
    private CharacterStats stats;

    private void Start()
    {
        entity = GetComponentInParent<Entity>();
        slider = GetComponentInChildren<Slider>();
        stats = GetComponentInParent<CharacterStats>();
     
        entity.onFlipped += FlipUI;
    }

    private void Update()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        slider.maxValue = stats.GetMaxHealth();
        slider.value = stats.CurrentHealth;
    }

    private void FlipUI() => transform.Rotate(0, 180, 0);

    private void OnDisable() => entity.onFlipped -= FlipUI;
}
