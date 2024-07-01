using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private string targetLayerName = "Player";
    [SerializeField] private int damage;
    [SerializeField] private float xVelocity;
    ParticleSystem particle;
    Rigidbody2D rb;
    bool isFlipped;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        particle = GetComponentInChildren<ParticleSystem>();
        Invoke(nameof(DestroyArrow), 15f);
    }

    private void Update()
    {
        rb.velocity = new Vector2(xVelocity, rb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            Debug.Log("arrow damage player");
            collision.GetComponent<CharacterStats>()?.TakeDamage(damage);
            StuckInto(collision);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StuckInto(collision);
        }
    }

    private void StuckInto(Collider2D collision)
    {
        particle.Stop();
        Invoke(nameof(DestroyArrow), 3f);
        transform.parent = collision.transform;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<CapsuleCollider2D>().enabled = false;
    }

    public void FlipArrow()
    {
        if (isFlipped) return;

        xVelocity = xVelocity * -1;
        isFlipped = true;
        transform.Rotate(0, 180, 0);
        targetLayerName = "Enemy";
    }

    private void DestroyArrow()
    {
        transform.parent = null;
        Destroy(gameObject);
    }
}
