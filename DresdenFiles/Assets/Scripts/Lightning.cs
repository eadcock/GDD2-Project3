using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning
{
    bool active;
    int damage;

    public Lightning()
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
