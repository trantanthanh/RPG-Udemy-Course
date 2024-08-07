using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private string targetLayerName = "Player";
    [SerializeField] private float xVelocity;
    public int direction;
    CharacterStats myStats;
    //ParticleSystem particle;
    Rigidbody2D rb;
    bool isFlipped;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //particle = GetComponentInChildren<ParticleSystem>();
        Invoke(nameof(DestroyArrow), 15f);
    }

    private void Update()
    {
        rb.velocity = new Vector2(xVelocity * direction, rb.velocity.y);
    }

    public void SetupArrow(float _speed, int _direction, CharacterStats _myStats)
    {
        this.xVelocity = _speed;
        this.direction = _direction;
        this.myStats = _myStats;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            CharacterStats targetStats = collision.GetComponent<CharacterStats>();
            if (targetStats != null)
            {
                myStats.DoDamage(targetStats);
            }
            StuckInto(collision);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StuckInto(collision);
        }
    }

    private void StuckInto(Collider2D collision)
    {
        //particle.Stop();
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
