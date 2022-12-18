using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    // Start is called before the first frame update

    [SerializeField] Generator2D generator2D;
    [SerializeField] GameObject wall;
    [SerializeField] GameObject door;
    [SerializeField] GameObject bossDoor;
    [SerializeField] GameObject floor;
    [SerializeField] GameObject top;
    [SerializeField] GameObject main;
    [SerializeField] GameObject furniture1;
    [SerializeField] GameObject bossFurniture;
    [SerializeField] GameObject key;
    [SerializeField] GameObject flask;


    [SerializeField] GameObject player;
    [SerializeField] GameObject meleeEnemy;
    float wallXOffSet = 0;
    float wallYOffSet = 0;
    static bool enemyDeath = false;
    Generator2D.Room prevRoom;
    Generator2D.Room currentRoom;
    Grid2D<Generator2D.CellType> grid;
    static Vector3 lastMobPosition;

    void Start()
    {
        GetGrid();
        SpawnPlayer();
        currentRoom =  generator2D.GetRoom(GetPlayerPosition());
        prevRoom = currentRoom;
        //SpawnMob();
        PlaceFurniture();
        //placeWalls();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentRoom != null)
        {
            if (CheckRoomChanges())
            {
                if (ActivateMobs()) 
                {
                    CloseDoors();
                }
                
            }
        }

        if (enemyDeath)
        {
            if (IsAllEnemyDead())
            {
                OpenDoors();
                SpawnReward();
            }
            enemyDeath = false;
        }

        
    }

    void GetGrid()
    {
        generator2D.Generate();
        generator2D.ChooseBossRoom();
        grid = generator2D.grid;
        for(int i = 0; i < grid.Size.x; i++)
        {
            for (int j = 0; j < grid.Size.y; j++)
            {
                switch(grid[i, j])
                {
                    case Generator2D.CellType.None:
                        {
                            break;
                        }
                    case Generator2D.CellType.Room:
                        {
                            if(IsNeighbourNone(grid, i, j))
                            {
                                PlaceWalls(grid,i,j);
                            }
                            SpawnFloor(i, j);
                            break;
                        }
                    case Generator2D.CellType.Hallway:
                        {
                            if (IsNeighbourNone(grid, i, j))
                            {
                                PlaceWalls(grid, i, j);
                                
                            }
                            PlaceDoors(grid, i, j);
                            SpawnFloor(i, j);
                            break;
                        }
                }
            }
        }
    }

    /*void placeWalls()
    {
        foreach(var room in generator2D.rooms)
        {
            for(int i= room.bounds.x; i < room.bounds.xMax; i++)
            {
                for (int j = room.bounds.y; j < room.bounds.yMax; j++)
                {
                    if (i == room.bounds.x)
                    {
                        Instantiate(wall, new Vector3(transform.position.x + i *4+2, 0, transform.position.z + j*4+2), Quaternion.Euler(0f, 90f, 0f));
                    }
                    else if(i == room.bounds.xMax-1)
                    {
                        Instantiate(wall, new Vector3(transform.position.x + i * 4 + 2, 0, transform.position.z + j * 4 + 2), Quaternion.Euler(0f,90f,0f));
                    }
                    else if (j == room.bounds.y)
                    {
                        Instantiate(wall, new Vector3(transform.position.x + i * 4 + 2, 0, transform.position.z + j * 4 + 2), Quaternion.identity);
                    }
                    else if (j == room.bounds.yMax-1)
                    {
                        Instantiate(wall, new Vector3(transform.position.x + i * 4 + 2, 0, transform.position.z + j * 4 + 2), Quaternion.identity);
                    }

                }
            }
            
        }
        
    }*/


    bool IsNeighbourNone(Grid2D<Generator2D.CellType> grid, int x, int y)
    {
        if (grid[x + 1, y] == Generator2D.CellType.None)
        {
            return true;
        }
        else if (grid[x - 1, y] == Generator2D.CellType.None)
        {
            return true;
        }
        else if (grid[x, y + 1] == Generator2D.CellType.None)
        {
            return true;
        }
        else if (grid[x, y - 1] == Generator2D.CellType.None)
        {
            return true;
        }
        return false;
    }
    void PlaceWalls(Grid2D<Generator2D.CellType> grid,int x,int y)
    {
        if(grid[x+1,y] == Generator2D.CellType.None)
        {
            wallXOffSet = 2;
            wallYOffSet = 0;
            InstantiateWalls(x, y, Quaternion.Euler(0, -90, 0));

        }
        if(grid[x - 1, y] == Generator2D.CellType.None)
        {
            wallXOffSet = -2;
            wallYOffSet = 0;
            InstantiateWalls(x, y, Quaternion.Euler(0, 90, 0));
        }
        if (grid[x, y+1] == Generator2D.CellType.None)
        {
            wallXOffSet = 0;
            wallYOffSet = 2;
            InstantiateWalls(x, y, Quaternion.Euler(0, 180, 0));
        }
        if (grid[x, y - 1] == Generator2D.CellType.None)
        {
            wallXOffSet = 0;
            wallYOffSet = -2;
            InstantiateWalls(x, y, Quaternion.Euler(0, 0, 0));
        }
    }

    void PlaceDoors(Grid2D<Generator2D.CellType> grid, int x, int y)
    {
        List<Quaternion> rotation = new List<Quaternion>();
        List<Vector2> offset = new List<Vector2>();
        if (grid[x + 1, y] == Generator2D.CellType.Room)
        {
            offset.Add(new Vector2(2, 0));
            rotation.Add(Quaternion.Euler(0, -90, 0));

        }
        if (grid[x - 1, y] == Generator2D.CellType.Room)
        {
            offset.Add(new Vector2(-2, 0));
            rotation.Add(Quaternion.Euler(0, 90, 0));
        }
        if (grid[x, y + 1] == Generator2D.CellType.Room)
        {
            offset.Add(new Vector2(0, 2));
            rotation.Add(Quaternion.Euler(0, 180, 0));
        }
        if (grid[x, y - 1] == Generator2D.CellType.Room)
        {
            offset.Add(new Vector2(0, -2));
            rotation.Add(Quaternion.Euler(0, 0, 0));
        }

        if (IsDoor(grid, x, y))
        {
            if(!generator2D.GetRoom(new Vector3(x + offset[0].x, 0, y + offset[0].y)).isBossRoom)
            {
                Instantiate(door, new Vector3(transform.position.x + x * 4 + offset[0].x, 0.14f, transform.position.z + y * 4 + offset[0].y),
                                rotation[0], main.transform);
            }
            else
            {
                Instantiate(bossDoor, new Vector3(transform.position.x + x * 4 + offset[0].x, 0.14f, transform.position.z + y * 4 + offset[0].y),
                                rotation[0], main.transform);
            }
            Debug.Log(generator2D.GetRoom(new Vector3(x + offset[0].x, 0, y + offset[0].y)).isBossRoom);
            

        }
        else
        {
            for(int i = 0; i < rotation.Count; i++)
            {
                Instantiate(wall, new Vector3(transform.position.x + x * 4 + offset[i].x, 0, transform.position.z + y * 4 + offset[i].y),
                rotation[i], main.transform);
            }
        }
    }

    bool IsDoor(Grid2D<Generator2D.CellType> grid, int x, int y)
    {
        int counter = 0;
        if (grid[x + 1, y] == Generator2D.CellType.Hallway)
        {
            counter++;
        }
        if (grid[x - 1, y] == Generator2D.CellType.Hallway)
        {
            counter++;
        }
        if (grid[x, y + 1] == Generator2D.CellType.Hallway)
        {
            counter++;
        }
        if (grid[x, y - 1] == Generator2D.CellType.Hallway)
        {
            counter++;
        }
        return counter == 1;
    }

    void InstantiateWalls(float x,float y,Quaternion rotate)
    {
        Instantiate(wall, new Vector3(transform.position.x + x * 4 + wallXOffSet, 0, transform.position.z + y * 4 + wallYOffSet),
                rotate, main.transform);
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

    void SpawnMob()
    {
        Vector3 enemy = new Vector3(transform.position.x + generator2D.rooms[0].bounds.x * 4 + 10, 2, transform.position.z + generator2D.rooms[0].bounds.y * 4 + 6);
        Instantiate(meleeEnemy, enemy, Quaternion.identity, main.transform);
    }


    void PlaceFurniture()
    {
        for(int i = 0; i < generator2D.rooms.Count; i++)
        {
            Debug.Log(generator2D.rooms[i].isBossRoom);
            Vector3 place = new Vector3(generator2D.rooms[i].bounds.x * 4 + transform.position.x + generator2D.rooms[i].bounds.width * 4 / 2, 0, transform.position.z + generator2D.rooms[i].bounds.y * 4 + generator2D.rooms[i].bounds.height * 4 / 2);
            GameObject EmptyObj = new GameObject(string.Format("Room{0}", generator2D.rooms[i].ID));
            EmptyObj.transform.parent = main.transform;
            if (generator2D.rooms[i].isBossRoom)
            {
                Instantiate(bossFurniture, place, Quaternion.identity, EmptyObj.transform);
            }
            else
            {
                Instantiate(furniture1, place, Quaternion.identity, EmptyObj.transform);
            }
            
        }
    }

    bool CheckRoomChanges()
    {
        var room = generator2D.GetRoom(GetPlayerPosition());
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
        return new Vector3(playerPosition.position.x / 4 + 2, 2, playerPosition.position.z / 4 + 2);
    }

    bool ActivateMobs()
    {
        bool hasEnemy = false;
        if (currentRoom != null)
        {
            Transform current = main.transform.Find(string.Format("Room{0}", currentRoom.ID)).GetChild(0);
            foreach (Transform child in current)
            {
                if (child.CompareTag("Enemy"))
                {
                    child.gameObject.SetActive(true);
                    hasEnemy = true;
                }
            }
        }
        return hasEnemy;
    }

    void CloseDoors()
    {
        DoorControler.isAvailible = false;
    }

    public static void EnemyDeath(Vector3 position)
    {
        lastMobPosition = position;
        enemyDeath = true;
    }


    bool IsAllEnemyDead()
    {
        int enemyCounter = 0;
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
    }

    void SpawnReward()
    {
        if(PlayerTarget.keys == PlayerTarget.maxKeys)
        {
            Instantiate(flask, lastMobPosition, Quaternion.identity, main.transform);
        }
        else
        {
            Instantiate(key, lastMobPosition, Quaternion.identity, main.transform);
        }
        
    }

}
