using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall
{
    bool active;
    int damage;

    public FireBall()
    {
        active = true;
        damage = 10;
    }

    public bool Active
    {
        get { return active; }
        set { active = value; }
    }

    public int Damage
    {
        get { return damage; }
    }
}
