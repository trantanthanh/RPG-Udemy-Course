using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFx : EntityFx
{
    [Header("Screen shake fx")]
    CinemachineImpulseSource screenShake;
    [SerializeField] private float shakePower;
    [SerializeField] private Vector3 shakeDirection;

    [Header("Trail image fx")]
    [SerializeField] GameObject afterImagePrefab;
    [SerializeField] float colorLoseRate;
    [Tooltip("Time interval between 2 images")]
    [SerializeField] float afterImageCooldow;

    [Header("Dust fx")]
    [SerializeField] ParticleSystem dustFx;
    private float afterImageCooldownTimer;

    protected override void Start()
    {
        base.Start();
        screenShake = GetComponent<CinemachineImpulseSource>();
    }

    protected override void Update()
    {
        base.Update();
        afterImageCooldownTimer -= Time.deltaTime;
    }

    public void ShakeScreen()
    {
        screenShake.m_DefaultVelocity = new Vector3(shakeDirection.x * player.facingDir, shakeDirection.y) * shakePower;
        screenShake.GenerateImpulse();
    }

    public void CreateAfterImage()
    {
        if (afterImageCooldownTimer <= 0)
        {
            afterImageCooldownTimer = afterImageCooldow;
            GameObject newAfterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);
            newAfterImage.GetComponent<AfterImageFx>().SetupAfterImageFx(colorLoseRate, spriteRenderer.sprite);
        }
    }
    public void PlayDustFx()
    {
        if (dustFx != null)
        {
            dustFx.Play();
        }
    }
}
