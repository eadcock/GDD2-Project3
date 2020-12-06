using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour
{
    bool fire = true;
    bool lightning = false;
    bool clone = false;
    bool flood = false;

    bool buttonHold = false;
    bool lightningCreate = false;
    bool cloneCreate = false;
    bool floodCreate = false;

    public string CurrentSpell => fire ? "Fire" :
                lightning ? "Lightning" :
                clone ? "Clone" :
                flood ? "Flood" :
                "None";

    float fireCoolDown = 0;
    float lightningCoolDown = 0;
    float cloneCoolDown = 0;
    float floodCoolDown = 0;
    float floodGrowth = .5f;
    float floodCounter = 3;
    float angle;

    public Rigidbody fireBall;
    public Transform lightningBolt;
    public Transform cloneBody;
    public Transform floodCircle;
    private DresdenController d;
    private Stamina s;
    private Vector2 currentMousePosition;


    // Start is called before the first frame update
    void Start()
    {
        d = GetComponent<DresdenController>();
        s = d.GetComponent<Stamina>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get mouse angle
        currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float y = currentMousePosition.y - d.transform.position.y;
        float x = currentMousePosition.x - d.transform.position.x;
        angle = Mathf.Atan2(y,x);
        //Select spell
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            fire = true;
            lightning = false;
            clone = false;
            flood = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            fire = false;
            lightning = true;
            clone = false;
            flood = false;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            fire = false;
            lightning = false;
            clone = true;
            flood = false;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            fire = false;
            lightning = false;
            clone = false;
            flood = true;
        }

        //Use spell
        if (!InputManager.isPaused && Input.GetMouseButtonDown(0))
        {
            if (fire == true && fireCoolDown <= 0)
            {
                FireBall ball = new FireBall();
                Rigidbody clone;
                clone = Instantiate(fireBall, new Vector3(d.transform.position.x, d.transform.position.y, -1), Quaternion.identity);
                clone.velocity = (transform.TransformDirection(Mathf.Cos(angle),Mathf.Sin(angle),0))*5;
                fireCoolDown = 1;
                s.RequestStaminaDrain(10);
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
                lightningCoolDown = 15;
                s.RequestStaminaDrain(25);
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
                    s.RequestStaminaDrain(30);
                }
                cloneCoolDown = 10;
            }
            if(flood == true && floodCoolDown <= 0)
            {
                floodCircle.transform.position = new Vector3(d.transform.position.x, d.transform.position.y, -1);
                floodCreate = true;
                s.RequestStaminaDrain(50);
                floodCoolDown = 3;
            }
        }

        //Cooldowns
        fireCoolDown -= Time.deltaTime;
        lightningCoolDown -= Time.deltaTime;
        cloneCoolDown -= Time.deltaTime;
        floodCoolDown -= Time.deltaTime;

        //Create and remove lightning
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

        //Expand flood
        if(floodCreate == true)
        {
            floodGrowth -= Time.deltaTime;
            floodCounter -= Time.deltaTime;
            if(floodGrowth <= 0)
            {
                floodCircle.localScale *= 2;
                floodGrowth = 1;
            }
        }
        if(floodCounter <= 0)
        {
            floodCircle.transform.position = new Vector3(0, -10, -1);
            floodCircle.localScale = new Vector3(1,1,0);
            floodCreate = false;
            floodCounter = 3;
        }
    }
}
