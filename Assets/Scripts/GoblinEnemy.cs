using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinEnemy : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        // set initial values
        attackRange = 0.4f;
        damage = 10;
        speed = 2;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}