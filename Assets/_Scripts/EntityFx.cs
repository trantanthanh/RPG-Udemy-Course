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

    [Header("Ailment Fx")]
    [SerializeField] ParticleSystem igniteFx;
    [SerializeField] ParticleSystem chillFx;
    [SerializeField] ParticleSystem shockFx;

    [Header("Hit fx")]
    [SerializeField] GameObject hitFxPrefab;
    [SerializeField] GameObject hitCritFxPrefab;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalMat = spriteRenderer.material;
    }

    public IEnumerator FlashFx()
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
        chillFx.Play();
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
        igniteFx.Play();
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
        shockFx.Play();
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

    public void MakeTransparent(bool _isTransparent)
    {
        if (_isTransparent)
        {
            spriteRenderer.color = Color.clear;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    public void CancelColorChange()
    {
        igniteFx.Stop();
        chillFx.Stop();
        shockFx.Stop();
        CancelInvoke();
        spriteRenderer.color = Color.white;
    }

    public void CreateHitFx(Transform _target, bool _isCritical = false)
    {
        float zRotation = Random.Range(-90, 90);
        float xPosition = Random.Range(-0.5f, 0.5f);
        float yPosition = Random.Range(-0.5f, 0.5f);
        Vector3 newPosition = _target.position + (new Vector3(xPosition, yPosition));
        GameObject newHitFx = Instantiate(_isCritical ? hitCritFxPrefab : hitFxPrefab, newPosition, Quaternion.identity);

        if (_isCritical)
        {
            newHitFx.transform.localScale = new Vector3(GetComponent<Entity>().facingDir, 1, 1);
        }
        else
        {
            newHitFx.transform.Rotate(new Vector3(0, 0, zRotation));
        }


        Destroy(newHitFx, 0.5f);
    }
}
