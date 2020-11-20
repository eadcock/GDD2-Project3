using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour
{
    bool fire = true;
    bool lightning = false;
    bool clone = false;

    bool buttonHold = false;
    bool lightningCreate = false;
    bool cloneCreate = false;

    float fireCoolDown = 0;
    float lightningCoolDown = 0;
    float cloneCoolDown = 0;

    public Rigidbody fireBall;
    public Transform lightningBolt;
    public Transform cloneBody;
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
            lightning = false;
            clone = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            fire = false;
            lightning = true;
            clone = false;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            fire = false;
            lightning = false;
            clone = true;
        }

        //Use spell
        if (!InputManager.isPaused && Input.GetMouseButtonDown(0))
        {
            if (fire == true && fireCoolDown <= 0)
            {
                FireBall ball = new FireBall();
                Rigidbody clone;
                clone = Instantiate(fireBall, new Vector3(d.transform.position.x, d.transform.position.y, -1), Quaternion.identity);
                clone.velocity = transform.TransformDirection(Vector3.right * 10);
                fireCoolDown = 1;
            }
            if (lightning == true && lightningCoolDown <= 0)
            {
                Lightning bolt = new Lightning();
                if (lightningCreate == false)
                {
                    lightningBolt = Instantiate(lightningBolt, new Vector3(d.transform.position.x, d.transform.position.y, 0), Quaternion.identity);
                    lightningCreate = true;
                }
                buttonHold = true;
                lightningCoolDown = 10;
            }
            if(clone == true && cloneCoolDown <= 0)
            {
                cloneCreate = false;
                if(cloneCreate == false)
                {
                    Transform cloneCopy;
                    cloneCopy = Instantiate(cloneBody, new Vector3(d.transform.position.x, d.transform.position.y, -1), Quaternion.identity);
                    cloneCreate = true;
                    Destroy(cloneCopy.gameObject, 5);
                }
                cloneCoolDown = 10;
            }
        }

        //Cooldowns
        fireCoolDown -= Time.deltaTime;
        lightningCoolDown -= Time.deltaTime;
        cloneCoolDown -= Time.deltaTime;

        if (Input.GetMouseButtonUp(0))
        {
            buttonHold = false;
        }

        if (buttonHold == true)
        {
            lightningBolt.transform.position = d.transform.position;
        }
        else
        {
            lightningBolt.transform.position = new Vector3(0, 10, 0);
        }
    }
}
