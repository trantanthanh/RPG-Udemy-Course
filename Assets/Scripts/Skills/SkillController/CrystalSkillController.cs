using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    private Animator animator;
    private float crystalExistTimer;
    public bool isDestroying { get; private set; } = false;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Explode()
    {
        isDestroying = true;
        animator.SetBool("Explode", true);
        //Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }

    public void SetupCrystal(float _crystalDuration)
    {
        this.crystalExistTimer = _crystalDuration;
        isDestroying = false;
    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;
        if (crystalExistTimer < 0 && !isDestroying)
        {
            Explode();
        }
    }

    private void SelfDestroy() => Destroy(gameObject);
}
