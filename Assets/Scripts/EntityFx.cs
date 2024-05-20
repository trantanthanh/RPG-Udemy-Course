using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFx : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [Header("Flash Fx")]
    [SerializeField] Material hitMat;
    [SerializeField] float flashDuration = 0.2f;
    private Material originalMat;

    [Header("Ailment color")]
    [SerializeField] private Color chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalMat = spriteRenderer.material;
    }

    private IEnumerator FlashFx()
    {
        Color currentColor = spriteRenderer.color;
        spriteRenderer.color = Color.white;
        spriteRenderer.material = hitMat;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = currentColor;
        spriteRenderer.material = originalMat;
    }

    public void RedColorBlinkWithInterval(float _second)
    {
        InvokeRepeating(nameof(RedColorBlink), 0f, _second);
    }

    public void RedColorBlink()
    {
        if (spriteRenderer.color != Color.white)
        {
            spriteRenderer.color = Color.white;
        }
        else
        {
            spriteRenderer.color = Color.red;
        }
    }

    public void IgniteFxFor(float _second, float _interval)
    {
        InvokeRepeating(nameof(IgniteColorFx),0f, _interval);
        Invoke(nameof(CancelColorChange), _second);
    }

    private void IgniteColorFx()
    {
        if (spriteRenderer.color != igniteColor[0])
        {
            spriteRenderer.color = igniteColor[0];
        } else
        {
            spriteRenderer.color = igniteColor[1];
        }
    }

    public void CancelColorChange()
    {
        CancelInvoke();
        spriteRenderer.color = Color.white;
    }
}
