using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{
    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            playerStats.DoMagicDamage(enemy.stats);
            Destroy(gameObject, 0.5f);
        }
    }
}
