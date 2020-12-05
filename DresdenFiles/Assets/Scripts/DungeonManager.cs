using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DungeonManager : MonoBehaviour
{
    public List<RoomManager> rooms;

    public GameObject[] roomPrefabs;

    public RoomManager CenterRoom => rooms[0];

    public RoomManager currentRoom;

    public int numRooms;

    public EnemyManager enemies;

    // Start is called before the first frame update
    void Start()
    {
        rooms = new List<RoomManager>();

        CreateDungeon();
    }

    private void CreateDungeon()
    {
        rooms.Add(Instantiate(roomPrefabs[0]).GetComponent<RoomManager>());
        RoomManager currentRoom = rooms[0];
        this.currentRoom = currentRoom;
        currentRoom.name = "Room 0";
        Vector2 currentPosition = new Vector2(0, 0);
        int currentNumRooms = 1;
        Dictionary<Vector2, RoomManager> map = new Dictionary<Vector2, RoomManager>
        {
            { new Vector2(0, 0), currentRoom }
        };
        /*Dictionary<RoomManager, Vector2> inverseMap = new Dictionary<RoomManager, Vector2>
        {
            { currentRoom, new Vector2(0, 0) }
        };
        while (currentNumRooms < numRooms)
        {
            RoomManager randomRoom = rooms[UnityEngine.Random.Range(0, rooms.Count - 1)];
            currentPosition = inverseMap[randomRoom];
            Direction? dir = GetRandomEmptyDirection(randomRoom);
            Debug.Log(randomRoom + " | " + dir);
            if (dir != null)
            {
                Direction randomDir = (Direction)dir;
                if (map.ContainsKey(currentPosition + DirectionToVec2(randomDir)))
                {
                    ConnectRooms(currentRoom, randomDir, map[currentPosition + DirectionToVec2(randomDir)]);
                    currentRoom = currentRoom[randomDir];
                }
                else
                {
                    RoomManager newRoom = Instantiate(roomPrefabs[0]).GetComponent<RoomManager>();
                    rooms.Add(newRoom);
                    map.Add(currentPosition + DirectionToVec2(randomDir), newRoom);
                    inverseMap.Add(newRoom, currentPosition + DirectionToVec2(randomDir));
                    newRoom.name = "Room " + currentNumRooms;

                    ConnectRooms(currentRoom, randomDir, newRoom);
                    MoveRoomRelativeTo(newRoom, randomDir, currentRoom);

                    currentRoom = newRoom;
                    currentNumRooms++;
                }
                currentPosition += DirectionToVec2(randomDir);
            }
        }*/
        // weighted random algorithm
        while (currentNumRooms < numRooms)
        {

            Direction randomDir = GetRandomDirection(currentPosition);
            //Debug.Log(currentPosition + DirectionToVec2(randomDir));
            if (currentRoom.neighbors.ContainsKey(randomDir))
            {
                currentRoom = currentRoom.neighbors[randomDir];
            }
            else if (map.ContainsKey(currentPosition + DirectionToVec2(randomDir)))
            {
                ConnectRooms(currentRoom, randomDir, map[currentPosition + DirectionToVec2(randomDir)]);
                currentRoom = currentRoom.neighbors[randomDir];
            }
            else
            {
                RoomManager newRoom = Instantiate(roomPrefabs[1]).GetComponent<RoomManager>();
                rooms.Add(newRoom);
                map.Add(currentPosition + DirectionToVec2(randomDir), newRoom);
                newRoom.name = "Room " + currentNumRooms;

                ConnectRooms(currentRoom, randomDir, newRoom);
                MoveRoomRelativeTo(newRoom, randomDir, currentRoom);

                currentRoom = newRoom;
                currentNumRooms++;

                //enemies.CreateEnemy(currentPosition);
            }
            currentPosition += DirectionToVec2(randomDir);
            //Debug.Log(currentPosition);
        }

        // Build boss room
        Direction? direction = GetRandomEmptyDirection(currentRoom);
        while(direction == null)
        {
            currentPosition += DirectionToVec2(GetRandomDirection(currentPosition));
            currentRoom = map[currentPosition];
            direction = GetRandomEmptyDirection(currentRoom);
        }
        RoomManager bossRoom = Instantiate(roomPrefabs[0]).GetComponent<RoomManager>();
        rooms.Add(bossRoom);
        map.Add(currentPosition + DirectionToVec2((Direction)direction), bossRoom);
        bossRoom.name = "Boss Room";

        ConnectRooms(currentRoom, (Direction)direction, bossRoom);
        MoveRoomRelativeTo(bossRoom, (Direction)direction, currentRoom);
    }

    public Direction? GetRandomEmptyDirection(RoomManager room)
    {
        List<Direction> directions = new List<Direction> { Direction.North, Direction.East, Direction.South, Direction.West };
        Queue<Direction> randomizedDirections = new Queue<Direction>();
        while(directions.Count > 0)
        {
            Direction randomDir = directions[UnityEngine.Random.Range(0, directions.Count - 1)];
            directions.Remove(randomDir);
            randomizedDirections.Enqueue(randomDir);
            
        }

        while(randomizedDirections.Count > 0)
        {
            Direction dir = randomizedDirections.Dequeue();
            if (!room.neighbors.ContainsKey(dir) || (room.neighbors.ContainsKey(dir) && room.neighbors[dir] == null))
                return dir;
        }

        return null;
    }

    public Direction GetRandomDirection(Vector2 pos)
    {
        Dictionary<Direction, float> weights = new Dictionary<Direction, float>
        {
            { Direction.North, 1 },
            { Direction.South, 1 },
            { Direction.East, 1 },
            { Direction.West, 1 }
        };
        if(pos.y < 0)
        {
            weights[Direction.South] -= 0.1f * pos.y;
            if (weights[Direction.South] < 0)
                weights[Direction.South] = 0;
        }
        if(pos.y > 0)
        {
            weights[Direction.North] -= 0.1f * Math.Abs(pos.y);
            if (weights[Direction.North] < 0)
                weights[Direction.North] = 0;
        }
        if(pos.x < 0)
        {
            weights[Direction.West] -= 0.1f * pos.x;
            if (weights[Direction.West] < 0)
                weights[Direction.West] = 0;
        }
        if(pos.x > 0)
        {
            weights[Direction.East] -= 0.1f * Math.Abs(pos.x);
            if (weights[Direction.East] < 0)
                weights[Direction.East] = 0;

        }

        float sum = weights[Direction.North] + weights[Direction.South] + weights[Direction.East] + weights[Direction.West];

        float random = UnityEngine.Random.Range(0, sum - 1);
        for(int i = 0; i < 4; i++)
        {
            if (random < weights[(Direction)i])
                return (Direction)i;
            random -= weights[(Direction)i];
        }
        Debug.LogWarning("Defaulted");
        return Direction.North;
    }

    public Vector2 DirectionToVec2(Direction dir)
    {
        switch (dir)
        {
            case Direction.North:
                return new Vector2(0, 1);
            case Direction.South:
                return new Vector2(0, -1);
            case Direction.East:
                return new Vector2(1, 0);
            default:
                return new Vector2(-1, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConnectRooms(RoomManager r1, Direction dir, RoomManager r2)
    {
        r1.SetNeighbor(dir, r2);
        GameObject newDoor = r1.DrawDoor(dir);
        Direction opDir;
        switch (dir)
        {
            case Direction.North:
                opDir = Direction.South;
                break;
            case Direction.South:
                opDir = Direction.North;
                break;
            case Direction.East:
                opDir = Direction.West;
                break;
            default:
                opDir = Direction.East;
                break;
        }
        r2.SetNeighbor(opDir, r1);
        
        r2.doors.Add(opDir, newDoor);

        newDoor.GetComponent<Door>().rooms = new RoomManager[2] { r1, r2 };
    }

    public void MoveRoomRelativeTo(RoomManager r, Direction dir, RoomManager rRef)
    {
        switch (dir)
        {
            case Direction.North:
                Vector3Int NorthCell = new Vector3Int(rRef.Rows, rRef.Columns / 2, 0);
                Vector3Int bottomLeft = NorthCell + new Vector3Int(1, -(r.Columns / 2), 0);
                r.transform.position = rRef.grid.CellToWorld(bottomLeft);
                break;
            case Direction.South:
                Vector3Int southCell = new Vector3Int(-1, rRef.Columns / 2, 0);
                bottomLeft = southCell + new Vector3Int(-r.Rows, -(r.Columns / 2), 0);
                r.transform.position = rRef.grid.CellToWorld(bottomLeft);
                break;
            case Direction.East:
                Vector3Int eastCell = new Vector3Int(rRef.Rows / 2, rRef.Columns, 0);
                bottomLeft = eastCell + new Vector3Int(-r.Rows / 2, 1, 0);
                r.transform.position = rRef.grid.CellToWorld(bottomLeft);
                break;
            case Direction.West:
                Vector3Int westCell = new Vector3Int(rRef.Rows / 2, -1, 0);
                bottomLeft = westCell + new Vector3Int(-r.Rows / 2, -r.Columns, 0);
                r.transform.position = rRef.grid.CellToWorld(bottomLeft);
                break;
        }
    }

    public (RoomManager, Direction) Move(GameObject door)
    {
        Vector3Int doorCell = currentRoom.grid.WorldToCell(door.transform.position);
        Direction? doorDir = null;
        if (doorCell.y >= currentRoom.Columns)
            doorDir = Direction.East;
        else if (doorCell.y <= 0)
            doorDir = Direction.West;
        else if (doorCell.x >= currentRoom.Rows)
            doorDir = Direction.North;
        else if (doorCell.x <= 0)
            doorDir = Direction.South;
        
        if (doorDir == null)
        {
            throw new Exception("That's not a door in this room.... where are you??");
        }

        currentRoom.Active = false;
        currentRoom = currentRoom.neighbors[(Direction)doorDir];
        currentRoom.Activate();
        return (currentRoom, (Direction)doorDir);
    }
}
