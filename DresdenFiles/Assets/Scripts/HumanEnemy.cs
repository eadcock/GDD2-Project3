using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanEnemy : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        // set initial values
        attackRange = 0.2f;
        damage = 15;
        attackCooldown = 0.0f;
        speed = 2;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}