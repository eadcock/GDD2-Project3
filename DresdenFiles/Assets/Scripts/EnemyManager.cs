using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> prefabs;

    private System.Random rng;

    public Action OnClear;

    public List<GameObject> enemies;

    public Dictionary<string, GameObject> enemyTypes;

    // Start is called before the first frame update
    void Awake()
    {
        rng = new System.Random();

        enemyTypes = new Dictionary<string, GameObject>
        {
            ["b"] = (GameObject)Instantiate(Resources.Load("Enemies/Bat")),
            ["g"] = (GameObject)Instantiate(Resources.Load("Enemies/Goblin")),
            ["h"] = (GameObject)Instantiate(Resources.Load("Enemies/Human")),
            ["k"] = (GameObject)Instantiate(Resources.Load("Enemies/Kenku")),
            ["p"] = (GameObject)Instantiate(Resources.Load("Enemies/Practitioner")),
            ["w"] = (GameObject)Instantiate(Resources.Load("Enemies/Wolf"))
        };
        foreach (KeyValuePair<string, GameObject> keyValue in enemyTypes)
        {
            keyValue.Value.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateEnemy(Vector2 pos, string enemyCode = "b")
    {
        GameObject newEnemy = Instantiate(enemyTypes[enemyCode], new Vector3(pos.x, pos.y, -1.9f), Quaternion.identity);
        enemies.Add(newEnemy);
        newEnemy.SetActive(true);
        Debug.Log(newEnemy.transform.position);
    }

    public void DestroyEnemy(GameObject enemy)
    {
        if(enemies.Count > 0)
        {
            enemies.Remove(enemy);
            Destroy(enemy);

            if (enemies.Count == 0)
            {
                OnClear.Invoke();
            }
        }
    }
}