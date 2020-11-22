using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using quiet;

public enum Direction
{
    North = 0,
    East,
    South,
    West
}

public enum OppositeDirection
{
    South = 0,
    West,
    North,
    East
}

public class RoomManager : MonoBehaviour
{
    private int rows, cols;

    public Dictionary<Direction, RoomManager> neighbors;
    public Dictionary<Direction, GameObject> door;

    [Header("Tile Data")]
    // [cols],[rows],[tile 1],...,[tile n-(rows*cols)]
    [SerializeField]
    [Tooltip("g = walkable ground, o = obstacle, 0 = empty space")]
    private string[] level;

    [SerializeField]
    private string[] enemies;

    public List<GameObject> activeEnemies;

    public Grid grid;
    public List<List<GameObject>> tiles;

    public Vector2 Center => grid.CellToWorld(new Vector3Int((cols / 2) - 1, (rows / 2) + 1, 0)) + new Vector3(0.5f, 0.5f, 0);

    public RoomManager this[Direction dir] { get => neighbors[dir]; set => SetNeighbor(dir, value); }

    public int Rows => rows;
    public int Columns => cols;

    private GameObject doorTile;

    public bool Active { get; set; }
    public bool Completed { get; set; }
    public bool Locked { get; set; }

    public Dictionary<string, GameObject> tileTypes;
    public Dictionary<string, GameObject> enemyTypes;

    // Start is called before the first frame update
    void Awake()
    {
        neighbors = new Dictionary<Direction, RoomManager>();
        door = new Dictionary<Direction, GameObject>();

        string[] metaData = level[0].Split(',');
        cols = int.Parse(metaData[0]);
        rows = int.Parse(metaData[1]);

        transform.position = new Vector3(-(cols / 2.0f), -(rows / 2.0f), transform.position.z);

        grid = GetComponent<Grid>();

        tileTypes = new Dictionary<string, GameObject>
        {
            ["g"] = (GameObject)Instantiate(Resources.Load("Ground")),
            ["o"] = (GameObject)Instantiate(Resources.Load("Obstacle")),
            //["f"] = (GameObject)Instantiate(Resources.Load("ShortObstacle")),
        };

        doorTile = (GameObject)Instantiate(Resources.Load("Door"));
        doorTile.SetActive(false);

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

                currentRow.Add(Instantiate(tileTypes[levelData[c]], grid.CellToWorld(new Vector3Int(Math.Map(r, 0, level.Length - 1, level.Length - 1, 0), c, 0)) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, gameObject.transform));
            }
        }

        DrawWalls();

        foreach (KeyValuePair<string, GameObject> keyValue in tileTypes)
        {
            Destroy(keyValue.Value);
        }

        Active = false;
        Completed = false;
        Locked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Active && !Completed)
        {
            if(activeEnemies.Count == 0)
            {
                Completed = true;
                Locked = false;
            }
        }
    }

    public void Activate()
    {
        if(!Completed)
        {
            if(enemies.Length > 0)
            {
                enemyTypes = new Dictionary<string, GameObject>
                {
                    ["b"] = (GameObject)Instantiate(Resources.Load("Bat")),
                    ["g"] = (GameObject)Instantiate(Resources.Load("Goblin")),
                    ["h"] = (GameObject)Instantiate(Resources.Load("Human")),
                    ["k"] = (GameObject)Instantiate(Resources.Load("Kenku")),
                    ["p"] = (GameObject)Instantiate(Resources.Load("Practitioner")),
                    ["w"] = (GameObject)Instantiate(Resources.Load("Wolf"))
                };
                foreach (KeyValuePair<string, GameObject> keyValue in enemyTypes)
                {
                    keyValue.Value.SetActive(false);
                }

                foreach (string enemy in enemies)
                {
                    string[] enemyData = enemy.Split(',');
                    activeEnemies.Add(Instantiate(enemyTypes[enemyData[0]], grid.CellToWorld(new Vector3Int(int.Parse(enemyData[2]), int.Parse(enemyData[1]), 0)) + (grid.cellSize.StripZ() / 2), Quaternion.identity));
                    activeEnemies[activeEnemies.Count - 1].SetActive(true);
                }

                Locked = true;

                /*foreach (KeyValuePair<string, GameObject> keyValue in enemyTypes)
                {
                    Destroy(keyValue.Value);
                }*/
            } 
            else
            {
                Locked = false;
            }
        }
        Active = true;
    }

    public void DrawWalls()
    {
        for(int j = -1; j < Columns + 1; j++)
        {
            Instantiate(tileTypes["o"], grid.CellToWorld(new Vector3Int(Rows, j, -1)) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, gameObject.transform);
        }
        for (int j = -1; j < Columns + 1; j++)
        {
            Instantiate(tileTypes["o"], grid.CellToWorld(new Vector3Int(-1, j, -1)) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, gameObject.transform);
        }
        for (int j = 0; j < Rows; j++)
        {
            Instantiate(tileTypes["o"], grid.CellToWorld(new Vector3Int(j, -1, -1)) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, gameObject.transform);
        }
        for (int j = 0; j < Rows; j++)
        {
            Instantiate(tileTypes["o"], grid.CellToWorld(new Vector3Int(j, Columns, -1)) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, gameObject.transform);
        }
    }

    public void DrawDoor(Direction dir)
    {
        switch (dir)
        {
            case Direction.North:
                door.Add(Direction.North, Instantiate(doorTile, grid.CellToWorld(new Vector3Int(rows, cols / 2, 0)) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, gameObject.transform));
                door[Direction.North].SetActive(true);
                break;
            case Direction.East:
                door.Add(Direction.East, Instantiate(doorTile, grid.CellToWorld(new Vector3Int(rows / 2, cols, 0)) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, gameObject.transform));
                door[Direction.East].SetActive(true);
                break;
            case Direction.South:
                door.Add(Direction.South, Instantiate(doorTile, grid.CellToWorld(new Vector3Int(-1, cols / 2, 0)) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, gameObject.transform));
                door[Direction.South].SetActive(true);
                break;
            case Direction.West:
                door.Add(Direction.West, Instantiate(doorTile, grid.CellToWorld(new Vector3Int(rows / 2, -1, 0)) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, gameObject.transform));
                door[Direction.West].SetActive(true);
                break;
        }
    }

    public void SetNeighbor(Direction dir, RoomManager manager, bool drawDoor = true)
    {
        if(neighbors.ContainsKey(dir))
            neighbors[dir] = manager;
        else
            neighbors.Add(dir, manager);

        if(drawDoor)
        {
            DrawDoor(dir);

            RoomTransition trans = door[dir].GetComponent<RoomTransition>();
            trans.destination = manager;
        }
    }
}
