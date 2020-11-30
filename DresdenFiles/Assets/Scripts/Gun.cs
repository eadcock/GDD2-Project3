using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public Bullet b = new Bullet();
    int bullets = 6;
    float reload = 1.5f;
    float angle;
    private Vector2 currentMousePosition;
    public Rigidbody2D bulletBody;
    private DresdenController d;
    Vector3 screenPos = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        d = GetComponent<DresdenController>();
        screenPos = Camera.main.WorldToScreenPoint(bulletBody.position);
    }

    // Update is called once per frame
    void Update()
    {
        //Find mouse angle
        currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float y = currentMousePosition.y - d.transform.position.y;
        float x = currentMousePosition.x - d.transform.position.x;
        angle = Mathf.Atan2(y, x);

        if (Input.GetKeyDown(KeyCode.Space) && bullets != 0)
        {
            b = new Bullet();
            Rigidbody2D clone;
            clone = Instantiate(bulletBody, new Vector3(d.transform.position.x, d.transform.position.y, -1), Quaternion.identity);
            clone.velocity = (transform.TransformDirection(Mathf.Cos(angle), Mathf.Sin(angle), 0)) * 10;
            // Ignore collisions between bullets and the player
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), clone.GetComponent<Collider2D>());
            bullets--;
        }

        if (bullets == 0)
        {
            reload -= Time.deltaTime;
        }
        if (reload <= 0)
        {
            bullets = 6;
            reload = 1.5f;
        }
        Debug.Log(reload);
    }
}//delegate d

public class Bullet
{
    public static int Damage = 15;
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
