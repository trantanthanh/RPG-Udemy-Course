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

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalMat = spriteRenderer.material;
    }

    private IEnumerator FlashFx()
    {
        spriteRenderer.material = hitMat;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.material = originalMat;
    }

    private void RedColorBlink()
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

    private void CancelBlink()
    {
        CancelInvoke();
        spriteRenderer.color = Color.white;
    }
}
