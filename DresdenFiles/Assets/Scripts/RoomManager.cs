using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using quiet;
using System.Linq;
using UnityEditor.AI;

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

    [SerializeField]
    private Sprite[] groundSprites;
    [SerializeField]
    private Sprite[] obstacleSprites;

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

    public EnemyManager enemyManager;

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

        Dictionary<string, Sprite> lookUpGroundSprite = new Dictionary<string, Sprite>
        {
            ["tr"] = groundSprites[0],
            ["t"] = groundSprites[1],
            ["tl"] = groundSprites[2],
            ["r"] = groundSprites[3],
            ["0"] = groundSprites[4],
            ["l"] = groundSprites[5],
            ["br"] = groundSprites[6],
            ["b"] = groundSprites[7],
            ["bl"] = groundSprites[8],
            ["rl"] = groundSprites[9],
            ["tb"] = groundSprites[10]
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

                string spriteIndex = "0";
                string type = levelData[c];
                Debug.Log(levelData[c]);
                if(levelData[c].Length > 1)
                {
                    string[] levelMetaData = levelData[c].Split('/');
                    spriteIndex = levelMetaData[1];
                    type = levelMetaData[0];
                }

                currentRow.Add(
                    Instantiate(tileTypes[type], 
                    (grid.CellToWorld(
                        new Vector3Int(
                            Math.Map(r, 0, level.Length - 1, level.Length - 1, 0), c, 0)
                        ) 
                        + new Vector3(0.5f, 0.5f, 0)
                    ).Replace(0.1f, Dimension.Z), 
                    Quaternion.identity, 
                    gameObject.transform
                    )
                );

                if (type == "g")
                    currentRow[currentRow.Count - 1].GetComponent<SpriteRenderer>().sprite = lookUpGroundSprite[spriteIndex];
                else
                    currentRow[currentRow.Count - 1].GetComponent<SpriteRenderer>().sprite = obstacleSprites[0];
            }
        }

        for(int i = 0; i < tiles.Count; i++)
        {
            for(int j = 0; j < tiles[i].Count; j++)
            {
                string neighborString = "";
                if(i - 1 >= 0 && tiles[i - 1][j].CompareTag("Obstacle"))
                {
                    neighborString += "t";
                }
                if(i + 1 < tiles.Count && tiles[i + 1][j].CompareTag("Obstacle"))
                {
                    neighborString += "b";
                }
                if(j - 1 >= 0 && tiles[i][j - 1].CompareTag("Obstacle"))
                {
                    neighborString += "r";
                }
                if(j + 1 < tiles[i].Count && tiles[i][j + 1].CompareTag("Obstacle"))
                {
                    neighborString += "l";
                }
                
                //tiles[i][j].GetComponent<SpriteRenderer>().sprite = lookUpGroundSprite[neighborString == "" ? "0" : neighborString];
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

        enemyManager = FindObjectOfType<EnemyManager>();

        BoxCollider collider = GetComponent<BoxCollider>();
        collider.size = new Vector2(grid.cellSize.x * Columns, grid.cellSize.y * Rows);

        NavMeshBuilder.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        if (!Completed)
        {
            if (enemies.Length > 0)
            {
                foreach (string enemy in enemies)
                {
                    string[] enemyData = enemy.Split(',');
                    string enemyType = enemyData[0];
                    string chosenEnemy = enemyData[0];
                    if(enemyType.Length > 0)
                    {
                        string[] enemyVarients = enemyType.Split('/');
                        if(enemyVarients.Length > 1)
                        {
                            List<string> e = new List<string>();
                            List<float> chance = new List<float>();
                            for(int i = 0; i < enemyVarients.Length; i += 1)
                            {
                                string[] chanceData = enemyVarients[i].Split('?');
                                e.Add(chanceData[0]);
                                chance.Add(int.Parse(chanceData[1]));
                            }

                            chosenEnemy = WeightedRandomEnemy(e.ToArray(), chance.ToArray());
                        }
                    }
                    if(chosenEnemy != "0")
                        enemyManager.CreateEnemy(grid.CellToWorld(new Vector3Int(int.Parse(enemyData[2]), int.Parse(enemyData[1]), 0)) + (grid.cellSize.StripZ() / 2), chosenEnemy);
                    
                }

                enemyManager.OnClear = OnClear;
                Locked = true;
            } 
            else
            {
                Locked = false;
                Completed = true;
            }
        }
        Active = true;
    }

    public string WeightedRandomEnemy(string[] enemies, float[] weights)
    {
        if(enemies.Length == 0)
            return "";
        else if (enemies.Length == 1)
            return enemies[0];

        float sum = weights.Aggregate((a, b) => a + b);

        float random = UnityEngine.Random.Range(0, sum);
        for (int i = 0; i < enemies.Length; i++)
        {
            if (random < weights[i])
                return enemies[i];
            random -= weights[i];
        }
        Debug.LogWarning("Defaulted");
        return enemies[0];
    }

    public void OnClear()
    {
        Completed = true;
        Locked = false;
    }

    public void DrawWalls()
    {
        for(int j = -1; j < Columns + 1; j++)
        {
            Instantiate(tileTypes["o"], grid.CellToWorld(new Vector3Int(Rows, j, 1)) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, gameObject.transform);
        }
        for (int j = -1; j < Columns + 1; j++)
        {
            Instantiate(tileTypes["o"], grid.CellToWorld(new Vector3Int(-1, j, 1)) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, gameObject.transform);
        }
        for (int j = 0; j < Rows; j++)
        {
            Instantiate(tileTypes["o"], grid.CellToWorld(new Vector3Int(j, -1, 1)) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, gameObject.transform);
        }
        for (int j = 0; j < Rows; j++)
        {
            Instantiate(tileTypes["o"], grid.CellToWorld(new Vector3Int(j, Columns, 1)) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, gameObject.transform);
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
