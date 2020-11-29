using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using quiet;

public class Enemy : MonoBehaviour
{
    public GameObject player;

    protected float viewRange;
    protected float attackRange;
    protected int damage;
    protected const float MAX_COOLDOWN = 3.0f;
    protected float attackCooldown;
    protected int speed;

    protected Health playerHealth;
    protected Vector3 pos;
    protected Vector3 playerPos;
    protected Gun gun;

    protected Anim animator;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // set initial values
        viewRange = 5.0f;
        attackRange = 1.0f;
        damage = 10;
        attackCooldown = 0.0f;
        speed = 5;

        // get other values
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<Health>();
        pos = gameObject.transform.position;
        playerPos = player.transform.position;
        gun = player.GetComponent<Gun>();

        animator = GetComponent<Anim>();
        animator.SetBool("Walking", true);

        // Destroy on death
        GetComponent<Health>().OnDeath += hed => FindObjectOfType<EnemyManager>().DestroyEnemy(gameObject);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // check if player is within view range
        if (quiet.Collision.BoundingCircle(gameObject.transform.position, viewRange, player.transform.position, 1.0f))
        {
            // move towards player
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, player.transform.position, speed * Time.deltaTime).FillDim(Dimension.Z, -2);
            // check if cooldown is over and player is within attack range
            if (attackCooldown <= 0.0f && 
                quiet.Collision.BoundingCircle(gameObject.transform.position, attackRange, player.transform.position, 1.0f))
            {
                // attack player
                playerHealth.TakeDamage(gameObject, damage);

                // reset cooldown
                attackCooldown = MAX_COOLDOWN;
            }
        }

        // adjust attack cooldown
        if (attackCooldown > 0.0f)
        {
            attackCooldown -= Time.deltaTime;
        }
    }
}