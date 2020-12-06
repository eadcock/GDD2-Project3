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
    float lightningDrainCooldown;
    float cloneCoolDown = 0;
    float floodCoolDown = 0;
    float floodGrowth = .5f;
    float floodCounter = 3;
    float angle;

    public GameObject fireBallPrefab;
    public Transform lightningBolt;
    public Transform cloneBody;
    public Transform floodCircle;
    private DresdenController d;
    private Stamina s;
    private Vector2 currentMousePosition;

    public Anim animator;


    // Start is called before the first frame update
    void Start()
    {
        d = GetComponent<DresdenController>();
        s = d.GetComponent<Stamina>();
        animator = d.GetComponent<Anim>();
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
            if (fire == true && fireCoolDown <= 0 && s.RequestStaminaDrain(10))
            {
                GameObject clone;
                clone = Instantiate(fireBallPrefab, new Vector3(d.transform.position.x, d.transform.position.y, -1), Quaternion.identity);
                clone.GetComponent<Rigidbody2D>().velocity = (transform.TransformDirection(Mathf.Cos(angle),Mathf.Sin(angle),0))*5;
                fireCoolDown = 1;
            }
            if (lightning == true && lightningCoolDown <= 0 && s.RequestStaminaDrain(1))
            {
                if (lightningCreate == false)
                {
                    lightningBolt.gameObject.SetActive(true);
                    lightningCreate = true;
                }
                else
                {
                    lightningBolt.gameObject.SetActive(true);
                }
                buttonHold = true;
                lightningCoolDown = 1;
                Lightning.Active = true;
            }
            if(clone == true && cloneCoolDown <= 0 && s.RequestStaminaDrain(30))
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
            if(flood == true && floodCoolDown <= 0 && s.RequestStaminaDrain(50))
            {
                floodCircle.transform.position = new Vector3(d.transform.position.x, d.transform.position.y, -1);
                floodCreate = true;
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
            Lightning.Active = false;
        }

        if (buttonHold == true && Lightning.Active)
        {
            lightningBolt.transform.position = d.transform.position + transform.TransformDirection(Mathf.Cos(angle), Mathf.Sin(angle), 0) * 0.7f;
            lightningBolt.transform.position = new Vector3(lightningBolt.transform.position.x, lightningBolt.transform.position.y - 0.3f, -1);
            lightningBolt.transform.rotation = Quaternion.Euler(0, 0, Mathf.Sign(Mathf.Cos(angle)) == 1 ? Mathf.Sin(angle) * 90 : Mathf.Sin(angle) * -90);
            lightningDrainCooldown += Time.deltaTime;
            if (lightningDrainCooldown >= 0.1)
            {
                lightningDrainCooldown = 0;
                if(!s.RequestStaminaDrain(1))
                { 
                    lightningBolt.gameObject.SetActive(false);
                    Lightning.Active = false;
                }
            }
        }
        else
        {
            lightningBolt.gameObject.SetActive(false);
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
