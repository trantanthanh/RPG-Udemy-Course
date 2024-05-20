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
    private bool isFlashing = false;

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
        isFlashing = true;
        Color currentColor = spriteRenderer.color;
        spriteRenderer.color = Color.white;
        spriteRenderer.material = hitMat;
        yield return new WaitForSeconds(flashDuration);
        isFlashing = false;
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

    public void ChillFxFor(float _second)
    {
        InvokeRepeating(nameof(ChillColorFx), 0f, 0.3f);
        Invoke(nameof(CancelColorChange), _second);
    }

    private void ChillColorFx()
    {
        if (isFlashing) return;
        spriteRenderer.color = chillColor;
    }

    public void IgniteFxFor(float _second, float _interval)
    {
        InvokeRepeating(nameof(IgniteColorFx), 0f, _interval);
        Invoke(nameof(CancelColorChange), _second);
    }

    private void IgniteColorFx()
    {
        if (isFlashing) return;
        if (spriteRenderer.color != igniteColor[0])
        {
            spriteRenderer.color = igniteColor[0];
        }
        else
        {
            spriteRenderer.color = igniteColor[1];
        }
    }

    public void ShockFxFor(float _second)
    {
        InvokeRepeating(nameof(ShockColorFx), 0f, 0.3f);
        Invoke(nameof(CancelColorChange), _second);
    }

    private void ShockColorFx()
    {
        if (isFlashing) return;
        if (spriteRenderer.color != shockColor[0])
        {
            spriteRenderer.color = shockColor[0];
        }
        else
        {
            spriteRenderer.color = shockColor[1];
        }
    }

    public void CancelColorChange()
    {
        CancelInvoke();
        spriteRenderer.color = Color.white;
    }
}
