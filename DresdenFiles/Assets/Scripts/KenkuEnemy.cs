using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KenkuEnemy : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        // set initial values
        attackRange = 0.2f;
        damage = 15;
        speed = 3;
        GetComponent<Pathfinding.AILerp>().speed = speed;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}