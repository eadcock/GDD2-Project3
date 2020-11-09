using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Bullet b = new Bullet();
        }

        /* if bullet is active
         * {
         * if player face left{bullet goes left}
         * if player faces right{bullet goes right}
         * }
         * 
         * if bullet hits enemy
         * enemy.health - bullet.Damage;
        */
    }
}

public class Bullet
{
    int damage;

    public Bullet()
    {
        damage = 5;
    }

    public int Damage
    {
        get { return damage; }
    }
}
