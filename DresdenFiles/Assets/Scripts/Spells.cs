using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour
{
    bool fire = true;
    public Rigidbody fireBall;
    private DresdenController d;


    // Start is called before the first frame update
    void Start()
    {
        d = GetComponent<DresdenController>();
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
                Rigidbody clone;
                clone = Instantiate(fireBall, new Vector3(d.transform.position.x, d.transform.position.y, 0), Quaternion.identity);
                clone.velocity = transform.TransformDirection(Vector3.right * 10);
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
