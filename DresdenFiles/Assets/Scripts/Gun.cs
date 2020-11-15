using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    Bullet b = new Bullet();
    public Rigidbody bullet;
    private DresdenController d;
    Vector3 screenPos = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        d = GetComponent<DresdenController>();
        screenPos = Camera.main.WorldToScreenPoint(bullet.position);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            b = new Bullet();
            Rigidbody clone;
            clone = Instantiate(bullet, new Vector3(d.transform.position.x, d.transform.position.y, 0), Quaternion.identity);
            clone.velocity = transform.TransformDirection(Vector3.right * 10);
            /*if (clone.position.x > screenPos.x)
            {
                Destroy(clone);
                Destroy(bullet);
            }*/
        }
        


    }
}

public class Bullet
{
    int damage;
    bool active;
    float x;
    float y;

    public Bullet()
    {
        damage = 15;
        active = true;
        x = 0;
        y = 0;
    }

    public int Damage
    {
        get { return damage; }
    }

    public bool Active
    {
        get { return active; }
        set { active = value; }
    }

    public float X
    {
        get { return x; }
        set { x = value; }
    }

    public float Y
    {
        get { return y; }
        set { y = value; }
    }
}
