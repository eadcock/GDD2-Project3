using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> prefabs;

    private System.Random rng;

    // Start is called before the first frame update
    void Start()
    {
        rng = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateEnemy(Vector2 pos)
    {
        Instantiate(prefabs[rng.Next(0, prefabs.Count)], new Vector3(pos.x, pos.y, -2), Quaternion.identity);
    }
}