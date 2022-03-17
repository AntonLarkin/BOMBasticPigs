using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exposion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<Player>() != null)
            {
                TakeLife(collision.GetComponent<Player>());
            }
        }
        if (collision.CompareTag("Pumpkin"))
        {
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Enemy"))
        {
            if (collision.GetComponent<Enemy>()!=null)
            {
                MakeDirty(collision.GetComponent<Enemy>());
            }
        }
    }

    private void MakeDirty(Enemy enemy)
    {
        enemy.BecomeDirty();
    }

    private void TakeLife(Player player)
    {
        player.LoseLife();
    }
}
