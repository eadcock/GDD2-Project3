using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBehavior : MonoBehaviour
{
    [SerializeField]
    private int tickRate;

    private float cooldown;

    public void Start()
    {
        cooldown = 0;
    }

    public void Update()
    {
        if(Lightning.Active)
        {
            cooldown += Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == null)
            return;

        if (collision.gameObject.layer == 9)
        {
            if(cooldown >= tickRate / 100)
            {
                Debug.Log("Damage!");
                collision.gameObject.GetComponent<Health>().TakeDamage(gameObject, Lightning.Damage);
                cooldown = 0;
            }
        }
    }
}
