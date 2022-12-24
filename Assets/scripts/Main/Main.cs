using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    enum DoorRotation
    {
        Left,
        Right,
        Top,
        Bot
    }
    [SerializeField] Generator2D generator2D;
    [SerializeField] GameObject wall;
    [SerializeField] GameObject door;
    [SerializeField] GameObject bossDoor;
    [SerializeField] GameObject floor;
    [SerializeField] GameObject top;
    [SerializeField] GameObject main;
    [SerializeField] GameObject furniture1;
    [SerializeField] GameObject furniture2;
    [SerializeField] GameObject furniture3;
    [SerializeField] GameObject furniture4;
    [SerializeField] GameObject bossFurniture;
    [SerializeField] GameObject key;
    [SerializeField] GameObject flask;

    [SerializeField] GameObject meleeEnemy;
    [SerializeField] GameObject rangeEnemy;
    [SerializeField] GameObject spiderEnemy;

    [SerializeField] GameObject player;

    [HideInInspector] public bool gameOver;
    [HideInInspector] public bool gameWin = false;
    [SerializeField] GameOver gameOverScreen;
    [SerializeField] TextController textController;
    float wallXOffSet = 0;
    float wallYOffSet = 0;
    int minMobInRoom = 3;
    int maxMobInRoom = 7;
    bool hallwayRoom;
    static bool enemyDeath = false;
    Generator2D.Room prevRoom;
    Generator2D.Room currentRoom;
    Grid2D<Generator2D.CellType> grid;
    static Vector3 lastMobPosition;

    void Start()
    {
        GetGrid();
        SpawnPlayer();
        currentRoom = generator2D.GetRoom(GetPlayerPosition(), 0);
        prevRoom = currentRoom;
        PlaceFurniture();
    }

    void Update()
    {
        if (gameOver)
        {
            if (gameWin)
            {
                textController.SetText("You Win");
            }
            else
            {
                textController.SetText("You Lose");
            }
            GameObject.FindGameObjectWithTag("Player").GetComponent<HideCursor>().SwitchState(true);
            gameOverScreen.OpenMenu();

        }
        if (currentRoom != null)
        {
            if (CheckRoomChanges())
            {
                Debug.Log("Changes");
                if (SpawnMobs())
                {
                    CloseDoors();
                }
            }
        }

        if (enemyDeath)
        {
            if (IsAllEnemyDead())
            {
                main.transform.Find(string.Format("Room{0}", currentRoom.ID)).GetChild(0).GetComponent<RoomState>().isRoomClear = true;
                OpenDoors();
                SpawnReward();
            }
            enemyDeath = false;
        }
    }

    void GetGrid()
    {
        generator2D.Generate(UnityEngine.Random.Range(0, 50000));
        generator2D.ChooseBossRoom();
        grid = generator2D.grid;
        for (int i = 0; i < grid.Size.x; i++)
        {
            for (int j = 0; j < grid.Size.y; j++)
            {
                switch (grid[i, j])
                {
                    case Generator2D.CellType.None:
                        {
                            break;
                        }
                    case Generator2D.CellType.Room:
                        {
                            if (IsNeighbourNone(grid, i, j))
                            {
                                PlaceWalls(grid, i, j);
                            }
                            SpawnFloor(i, j);
                            break;
                        }
                    case Generator2D.CellType.Hallway:
                        {
                            PlaceHallwayWalls(grid, i, j);
                            SpawnFloor(i, j);
                            break;
                        }
                    case Generator2D.CellType.DoorLeft:
                        {
                            if (IsNeighbourNone(grid, i, j))
                            {
                                PlaceWalls(grid, i, j);

                            }
                            if (IsNeighbourRoom(grid, i, j))
                            {
                                PlaceDoor(grid, i, j, false,DoorRotation.Left);
                            }
                            SpawnFloor(i, j);
                            break;
                        }
                    case Generator2D.CellType.DoorRight:
                        {
                            if (IsNeighbourNone(grid, i, j))
                            {
                                PlaceWalls(grid, i, j);

                            }
                            if (IsNeighbourRoom(grid, i, j))
                            {
                                PlaceDoor(grid, i, j, false, DoorRotation.Right);
                            }
                            SpawnFloor(i, j);
                            break;
                        }
                    case Generator2D.CellType.DoorBot:
                        {
                            if (IsNeighbourNone(grid, i, j))
                            {
                                PlaceWalls(grid, i, j);

                            }
                            if (IsNeighbourRoom(grid, i, j))
                            {
                                PlaceDoor(grid, i, j, false, DoorRotation.Bot);
                            }
                            SpawnFloor(i, j);
                            break;
                        }
                    case Generator2D.CellType.DoorTop:
                        {
                            if (IsNeighbourNone(grid, i, j))
                            {
                                PlaceWalls(grid, i, j);

                            }
                            if (IsNeighbourRoom(grid, i, j))
                            {
                                PlaceDoor(grid, i, j, false, DoorRotation.Top);
                            }
                            SpawnFloor(i, j);
                            break;
                        }
                    case Generator2D.CellType.DoubleDoor:
                        {
                            if (IsNeighbourNone(grid, i, j))
                            {
                                PlaceWalls(grid, i, j);

                            }
                            if (IsNeighbourRoom(grid, i, j))
                            {
                                PlaceDoor(grid, i, j, true, DoorRotation.Left);
                            }
                            SpawnFloor(i, j);
                            break;
                        }
                }
            }
        }
    }

    bool IsNeighbourNone(Grid2D<Generator2D.CellType> grid, int x, int y)
    {
        if (x == grid.Size.x - 1)
        {
            return true;
        }
        else if (grid[x + 1, y] == Generator2D.CellType.None)
        {
            return true;
        }
        if (x == 0)
        {
            return true;
        }
        else if (grid[x - 1, y] == Generator2D.CellType.None)
        {
            return true;
        }
        if (y == grid.Size.y-1)
        {
            return true;
        }
        else if (grid[x, y + 1] == Generator2D.CellType.None)
        {
            return true;
        }
        if (y == 0)
        {
            return true;
        }
        else if (grid[x, y - 1] == Generator2D.CellType.None)
        {
            return true;
        }
        return false;
    }

    bool IsNeighbourRoom(Grid2D<Generator2D.CellType> grid, int x, int y)
    {
        if (x < grid.Size.x && grid[x + 1, y] == Generator2D.CellType.Room)
        {
            return true;
        }
        else if (x > 0 && grid[x - 1, y] == Generator2D.CellType.Room)
        {
            return true;
        }
        else if (y < grid.Size.y && grid[x, y + 1] == Generator2D.CellType.Room)
        {
            return true;
        }
        else if (y > 0 && grid[x, y - 1] == Generator2D.CellType.Room)
        {
            return true;
        }
        return false;
    }
    void PlaceWalls(Grid2D<Generator2D.CellType> grid, int x, int y)
    {
        if(x == grid.Size.x-1)
        {
            wallXOffSet = 2;
            wallYOffSet = 0;
            InstantiateWalls(x, y, Quaternion.Euler(0, -90, 0));
        }
        else if (grid[x + 1, y] == Generator2D.CellType.None)
        {
            wallXOffSet = 2;
            wallYOffSet = 0;
            InstantiateWalls(x, y, Quaternion.Euler(0, -90, 0));
        }
        if (x == 0)
        {
            wallXOffSet = -2;
            wallYOffSet = 0;
            InstantiateWalls(x, y, Quaternion.Euler(0, 90, 0));
        }
        else if(grid[x - 1, y] == Generator2D.CellType.None)
        {
            wallXOffSet = -2;
            wallYOffSet = 0;
            InstantiateWalls(x, y, Quaternion.Euler(0, 90, 0));
        }
        if (y == grid.Size.y-1)
        {
            wallXOffSet = 0;
            wallYOffSet = 2;
            InstantiateWalls(x, y, Quaternion.Euler(0, 180, 0));
        }
        else if(grid[x, y + 1] == Generator2D.CellType.None)
        {
            wallXOffSet = 0;
            wallYOffSet = 2;
            InstantiateWalls(x, y, Quaternion.Euler(0, 180, 0));
        }
        if (y == 0)
        {
            wallXOffSet = 0;
            wallYOffSet = -2;
            InstantiateWalls(x, y, Quaternion.Euler(0, 0, 0));
        }
        else if(grid[x, y - 1] == Generator2D.CellType.None)
        {
            wallXOffSet = 0;
            wallYOffSet = -2;
            InstantiateWalls(x, y, Quaternion.Euler(0, 0, 0));
        }
    }

    void PlaceHallwayWalls(Grid2D<Generator2D.CellType> grid, int x, int y)
    {
        if (x < grid.Size.x && grid[x + 1, y] == Generator2D.CellType.None || x < grid.Size.x && grid[x + 1, y] == Generator2D.CellType.Room)
        {
            wallXOffSet = 2;
            wallYOffSet = 0;
            InstantiateWalls(x, y, Quaternion.Euler(0, -90, 0));

        }
        if (x > 0 && grid[x - 1, y] == Generator2D.CellType.None || x > 0 && grid[x - 1, y] == Generator2D.CellType.Room)
        {
            wallXOffSet = -2;
            wallYOffSet = 0;
            InstantiateWalls(x, y, Quaternion.Euler(0, 90, 0));
        }
        if (y < grid.Size.y && grid[x, y + 1] == Generator2D.CellType.None || y < grid.Size.y && grid[x, y + 1] == Generator2D.CellType.Room)
        {
            wallXOffSet = 0;
            wallYOffSet = 2;
            InstantiateWalls(x, y, Quaternion.Euler(0, 180, 0));
        }
        if (y > 0 && grid[x, y - 1] == Generator2D.CellType.None || y > 0 && grid[x, y - 1] == Generator2D.CellType.Room)
        {
            wallXOffSet = 0;
            wallYOffSet = -2;
            InstantiateWalls(x, y, Quaternion.Euler(0, 0, 0));
        }
    }

    void PlaceDoor(Grid2D<Generator2D.CellType> grid, int x, int y, bool isDoubleDoor,DoorRotation rotation)
    {
        int doorCount = 0;
        if (x < grid.Size.x && grid[x + 1, y] == Generator2D.CellType.Room)
        {
            wallXOffSet = 2;
            wallYOffSet = 0;
            if (isDoubleDoor | rotation == DoorRotation.Right)
            {
                InstantiateDoors(x, y, Quaternion.Euler(0, -90, 0));
            }
            else
            {
                InstantiateWalls(x, y, Quaternion.Euler(0, -90, 0));
            }
            doorCount++;

        }
        if (x > 0 && grid[x - 1, y] == Generator2D.CellType.Room)
        {
            wallXOffSet = -2;
            wallYOffSet = 0;
            if (isDoubleDoor | rotation == DoorRotation.Left)
            {
                InstantiateDoors(x, y, Quaternion.Euler(0, 90, 0));
            }
            else
            {
                InstantiateWalls(x, y, Quaternion.Euler(0, 90, 0));
            }
            doorCount++;
        }
        if (y < grid.Size.y && grid[x, y + 1] == Generator2D.CellType.Room)
        {
            wallXOffSet = 0;
            wallYOffSet = 2;
            if ( isDoubleDoor | rotation == DoorRotation.Top)
            {
                InstantiateDoors(x, y, Quaternion.Euler(0, 180, 0));
            }
            else
            {
                InstantiateWalls(x, y, Quaternion.Euler(0, 180, 0));
            }
            doorCount++;
        }
        if (y > 0 && grid[x, y - 1] == Generator2D.CellType.Room)
        {
            wallXOffSet = 0;
            wallYOffSet = -2;
            if ( isDoubleDoor || rotation == DoorRotation.Bot)
            {
                InstantiateDoors(x, y, Quaternion.Euler(0, 0, 0));
            }
            else
            {
                InstantiateWalls(x, y, Quaternion.Euler(0, 0, 0));
            }
        }
    }
   
    void InstantiateWalls(float x,float y,Quaternion rotate)
    {
        Instantiate(wall, new Vector3(transform.position.x + x * 4 + wallXOffSet, 0, transform.position.z + y * 4 + wallYOffSet),
                rotate, main.transform);
    }

    void InstantiateDoors(float x, float y, Quaternion rotate)
    {
        if (!generator2D.GetRoom(new Vector3(x + wallXOffSet, 0, y + wallYOffSet), 0).isBossRoom)
        {
            Instantiate(door, new Vector3(transform.position.x + x * 4 + wallXOffSet, 0, transform.position.z + y * 4 + wallYOffSet),
                rotate, main.transform);
        }
        else
        {
            Instantiate(bossDoor, new Vector3(transform.position.x + x * 4 + wallXOffSet, 0, transform.position.z + y * 4 + wallYOffSet),
                rotate, main.transform);
        }
        
    }

    void SpawnPlayer()
    {
        Vector3 playerPosition = new Vector3( transform.position.x + generator2D.rooms[0].bounds.x * 4+5,2, transform.position.z + generator2D.rooms[0].bounds.y * 4 + 6);
        Instantiate(player, playerPosition, Quaternion.identity, main.transform);
    }

    void SpawnFloor(float x,float y)
    {
        Instantiate(top, new Vector3(transform.position.x + x * 4, 4, transform.position.z + y * 4), Quaternion.Euler(180, 0, 0), main.transform);
        Instantiate(floor, new Vector3(transform.position.x + x * 4, 0, transform.position.z + y * 4), Quaternion.identity,main.transform);
    }

    void PlaceFurniture()
    {
        List<GameObject> allFurtiture = new List<GameObject>() { furniture1, furniture2, furniture3, furniture4 };
        for(int i = 0; i < generator2D.rooms.Count; i++)
        {
            Vector3 place = new Vector3(generator2D.rooms[i].bounds.x * 4 + transform.position.x + generator2D.rooms[i].bounds.width * 4 / 2, 0, transform.position.z + generator2D.rooms[i].bounds.y * 4 + generator2D.rooms[i].bounds.height * 4 / 2);
            GameObject EmptyObj = new GameObject(string.Format("Room{0}", generator2D.rooms[i].ID));
            EmptyObj.transform.parent = main.transform;
            if (generator2D.rooms[i].isBossRoom)
            {
                Instantiate(bossFurniture, place, Quaternion.identity, EmptyObj.transform);
            }
            else
            {
                Instantiate(allFurtiture[UnityEngine.Random.Range(0, 3)], place, Quaternion.identity, EmptyObj.transform);
            }
            
        }
    }
    bool CheckRoomChanges()
    {
        var room = generator2D.GetRoom(GetPlayerPosition(),2);
        if(room != null)
        {
            if (room.ID != currentRoom.ID)
            {
                prevRoom = currentRoom;
                currentRoom = room;
                return true;
            }
        }
        return false;
    }

    Vector3 GetPlayerPosition()
    {
        Transform playerPosition = main.transform.Find("Player(Clone)").transform;
        return new Vector3(playerPosition.position.x / 4 , 2, playerPosition.position.z / 4);
    }

    bool SpawnMobs()
    {
        if (currentRoom != null)
        {
            Transform current = main.transform.Find(string.Format("Room{0}", currentRoom.ID)).GetChild(0);
            if (!current.GetComponent<RoomState>().isRoomClear)
            {
                if (currentRoom.isBossRoom)
                {
                    Vector3 bossPos = new Vector3(transform.position.x + currentRoom.bounds.x * 4 + currentRoom.bounds.width * 2, 2, transform.position.z + currentRoom.bounds.y * 4 + currentRoom.bounds.height * 2);
                    Instantiate(spiderEnemy, bossPos, Quaternion.identity, main.transform.Find(string.Format("Room{0}", currentRoom.ID)).GetChild(0));
                }
                else
                {
                    int mobCount = UnityEngine.Random.Range(minMobInRoom,maxMobInRoom);
                    List<GameObject> mobs = new List<GameObject> { meleeEnemy, rangeEnemy};
                    for(int i = 0; i < mobCount; i++)
                    {
                        Vector3 randomPosition;
                        Vector3 roomCenter = new Vector3(transform.position.x + currentRoom.bounds.x * 4 + currentRoom.bounds.width * 2, 0, transform.position.z + currentRoom.bounds.y * 4 + currentRoom.bounds.height * 2);
                        float range = currentRoom.bounds.width < currentRoom.bounds.height  ? currentRoom.bounds.width * 2 : currentRoom.bounds.height * 2;
                        RandomPointOnNavMesh(roomCenter,range,out randomPosition);
                        Instantiate(mobs[UnityEngine.Random.Range(0, 2)], randomPosition, Quaternion.identity, main.transform.Find(string.Format("Room{0}", currentRoom.ID)).GetChild(0));  
                    }
                }
                return true;
            }
        }
        return false;
    }

    void RandomPointOnNavMesh(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
        randomPoint.y = 0;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas);
        result = hit.position; 
    }


    void CloseDoors()
    {
        DoorControler.isAvailible = false;
        BossDoorControler.isAvailible = false;
    }

    public static void EnemyDeath(Vector3 position)
    {
        lastMobPosition = position;
        enemyDeath = true;
    }


    bool IsAllEnemyDead()
    {
        Transform current = main.transform.Find(string.Format("Room{0}", currentRoom.ID)).GetChild(0);

        foreach (Transform child in current)
        {
            if (child.CompareTag("Enemy"))
            {
                if (child.gameObject.activeSelf)
                {
                    return false;
                }
            }
        }
        return true;
    }

    void OpenDoors()
    {
        DoorControler.isAvailible = true;
        BossDoorControler.isAvailible = true;
    }

    void SpawnReward()
    {
        if(PlayerTarget.keys >= PlayerTarget.maxKeys)
        {
            Instantiate(flask, lastMobPosition, Quaternion.identity, main.transform);
        }
        else
        {
            Instantiate(key, lastMobPosition, Quaternion.identity, main.transform);
        }
    }
    
}
