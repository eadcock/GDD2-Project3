using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollisionHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 9)
        {
            if (collision.gameObject.TryGetComponent(out Health collisionHealth))
            {
                collisionHealth.TakeDamage(gameObject, Bullet.Damage);
            }

            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            if (collision.gameObject.TryGetComponent(out Health collisionHealth))
            {
                collisionHealth.TakeDamage(gameObject, Bullet.Damage);
            }

            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
    }
}
