using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollisionHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.gameObject.CompareTag("Player"))
        {
            Health collisionHealth;
            if (collision.gameObject.TryGetComponent(out collisionHealth))
            {
                collisionHealth.TakeDamage(gameObject, Bullet.Damage);
            }

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
