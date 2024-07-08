using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utilities;

public class EntityFx : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    protected Player player;

    [Header("Pop up text")]
    [SerializeField] GameObject popupTextPrefab;

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

    private GameObject myHealthBar;

    protected virtual void Start()
    {
        player = GetComponent<Player>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalMat = spriteRenderer.material;

        myHealthBar = GetComponentInChildren<UI_HealthBar>().gameObject;
    }

    protected virtual void Update()
    {
    }

    public void CreatePopupText(string _text)
    {
        float randomX = Random.Range(-.5f, 0.5f);
        float randomY = Random.Range(0.5f, 0.8f);
        Vector3 offset = new Vector2(randomX, randomY);
        GameObject newPopupText = Instantiate(popupTextPrefab, transform.position + offset, Quaternion.identity);
        newPopupText.GetComponent<TextMeshPro>().text = _text;
    }

    public IEnumerator FlashFx()
    {
        isFlashing = true;
        Color currentColor = spriteRenderer.color;
        spriteRenderer.color = Color.white;
        spriteRenderer.material = hitMat;
        yield return flashDuration.WaitForSeconds();
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

    protected void ChillColorFx()
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

    protected void IgniteColorFx()
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

    protected void ShockColorFx()
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
            myHealthBar.SetActive(false);
        }
        else
        {
            spriteRenderer.color = Color.white;
            myHealthBar.SetActive(true);
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
        Vector3 hitFxRotation = new Vector3(0, 0, zRotation);

        float xPosition = Random.Range(-0.5f, 0.5f);
        float yPosition = Random.Range(-0.5f, 0.5f);
        if (_isCritical)
        {
            float yRotation = GetComponent<Entity>().facingDir == -1 ? 180 : 0;

            zRotation = Random.Range(-45, 45);
            hitFxRotation = new Vector3(0, yRotation, zRotation);
        }

        Vector3 newPosition = _target.position + (new Vector3(xPosition, yPosition));
        GameObject newHitFx = Instantiate(_isCritical ? hitCritFxPrefab : hitFxPrefab, newPosition, Quaternion.identity);

        newHitFx.transform.Rotate(hitFxRotation);

        Destroy(newHitFx, 0.5f);
    }
}
