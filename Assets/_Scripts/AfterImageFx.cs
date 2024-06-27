using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageFx : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float colorLoseRate;

    public void SetupAfterImageFx(float _colorLoseRate, Sprite _spriteImage)
    {
        this.colorLoseRate = _colorLoseRate;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = _spriteImage;

    }

    private void Update()
    {
        float alpha = spriteRenderer.color.a - colorLoseRate * Time.deltaTime;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);

        if (alpha <= 0)
        {
            Destroy(gameObject);
        }
    }
}
