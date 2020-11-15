using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using quiet;

public class Collectible : MonoBehaviour
{
    public GameObject player;
    public string type;

    private Health pHealth;
    private Inventory inv;

    // Start is called before the first frame update
    void Start()
    {
        pHealth = player.GetComponent<Health>();
        inv = player.GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        // if colliding with the player, pick up
        if (quiet.Collision.BoundingCircle(gameObject.transform.position, 0.1f, player.transform.position, 0.1f))
        {
            if (type == "clue")
            {
                // add to inventory
                inv.AddItem(this);
                this.gameObject.SetActive(false);
            }
            else if (type == "health")
            {
                // heal player
                // 30?
                pHealth.Heal(30);
                Destroy(this.gameObject);
            }
        }
    }
}