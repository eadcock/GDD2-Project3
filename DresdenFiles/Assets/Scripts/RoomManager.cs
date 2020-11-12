using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using quiet;
public class RoomManager : MonoBehaviour
{
    private int rows, cols;

    // [cols],[rows],[tile 1],...,[tile n-(rows*cols)]
    [SerializeField]
    private string[] level;

    public Grid grid;
    public List<List<GameObject>> tiles;

    public Dictionary<string, GameObject> tileTypes;

    // Start is called before the first frame update
    void Start()
    {
        string[] metaData = level[0].Split(',');
        rows = int.Parse(metaData[0]);
        cols = int.Parse(metaData[1]);

        transform.position = new Vector3(-(cols / 2), -(rows / 2), transform.position.z);

        tileTypes = new Dictionary<string, GameObject>
        {
            ["g"] = (GameObject)Instantiate(Resources.Load("Ground")),
            ["o"] = (GameObject)Instantiate(Resources.Load("Obstacle")),
            //["f"] = (GameObject)Instantiate(Resources.Load("ShortObstacle")),
        };

        grid = GetComponent<Grid>();

        tiles = new List<List<GameObject>>();
        for(int r = level.Length - 1; r >= 1; r--)
        {
            List<GameObject> currentRow = new List<GameObject>();
            tiles.Add(currentRow);
            string[] levelData = level[r].Split(',');
            for(int c = 0; c < levelData.Length; c++)
            {
                if (levelData[c] == "0")
                    continue;

                currentRow.Add(Instantiate(tileTypes[levelData[c]], grid.CellToWorld(new Vector3Int(Math.Map(r, 0, level.Length - 1, level.Length - 1, 0), c, 0)) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity));
            }
        }

        foreach(KeyValuePair<string, GameObject> keyValue in tileTypes)
        {
            Destroy(tileTypes[keyValue.Key]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
