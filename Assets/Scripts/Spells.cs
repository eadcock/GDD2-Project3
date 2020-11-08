using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour
{
    bool fire = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //Select spell
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
             fire = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            fire = false;
        }

        //Use spell
        if (Input.GetMouseButtonDown(0))
        {
            if(fire == true)
            {
                FireBall ball = new FireBall();
            }
        }



        //Check if it collides with enemy
        if(fire == true)
        {
            /*if((ball.x - enemy.x <= 10) && ball.y - enemy.y <= 5)
             *{
             * enemy.dead;/enemy.health - 10;
             * ball.Active = false;
             *}
            */
        }
    }
}
