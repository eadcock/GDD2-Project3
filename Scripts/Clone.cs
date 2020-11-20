using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone
{
    bool active;
    int health;

    public Clone()
    {
        active = true;
        health = 1;
    }

    public bool Active
    {
        get { return active; }
        set { active = value; }
    }

    public int Health
    {
        get { return health; }
    }
}
