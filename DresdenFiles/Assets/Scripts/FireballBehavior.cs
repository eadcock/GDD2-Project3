using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9 || collision.gameObject.layer == 8)
        {
            Explode();

            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        RaycastHit2D[] targets = Physics2D.CircleCastAll(transform.position, 1.0f, new Vector2(0, 1), 1.0f, LayerMask.GetMask("Enemies"));
        Debug.Log(targets.Length);
        foreach(RaycastHit2D target in targets)
        {
            if(target.collider.gameObject != gameObject)
            {
                target.collider.gameObject.GetComponent<Health>().TakeDamage(gameObject, FireBall.Damage);
            }
        }
    }
}
