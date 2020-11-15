using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Collectible> inv;

    // Start is called before the first frame update
    void Start()
    {
        inv = new List<Collectible>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(Collectible item)
    {
        inv.Add(item);
    }
}