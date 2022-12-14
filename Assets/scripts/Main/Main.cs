using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    // Start is called before the first frame update

    [SerializeField] Generator2D generator2D;
    [SerializeField] GameObject wall;
    [SerializeField] GameObject floor;
    [SerializeField] GameObject main;
    [SerializeField] GameObject furniture1;

    [SerializeField] GameObject player;
    [SerializeField] GameObject meleeEnemy;
    float wallXOffSet = 0;
    float wallYOffSet = 0;

    void Start()
    {
        GetGrid();
        SpawnPlayer();
        //SpawnMob();
        PlaceFurniture();
        //placeWalls();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetGrid()
    {
        generator2D.Generate();
        Grid2D<Generator2D.CellType> grid = generator2D.grid;
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
        //Instantiate(floor, new Vector3(transform.position.x + x * 4, 4, transform.position.z + y * 4), Quaternion.Euler(180, 0, 0), main.transform);
        Instantiate(floor, new Vector3(transform.position.x + x * 4, 0, transform.position.z + y * 4), Quaternion.identity,main.transform);
    }

    void SpawnMob()
    {
        Vector3 enemy = new Vector3(transform.position.x + generator2D.rooms[0].bounds.x * 4 + 10, 2, transform.position.z + generator2D.rooms[0].bounds.y * 4 + 6);
        Instantiate(meleeEnemy, enemy, Quaternion.identity, main.transform);
    }


    void PlaceFurniture()
    {
        foreach (var room in generator2D.rooms)
        {
            Vector3 place = new Vector3(room.bounds.x * 4 + transform.position.x + room.bounds.width * 4 / 2, 0, transform.position.z + room.bounds.y *4 + room.bounds.height * 4 / 2);
            Instantiate(furniture1, place,Quaternion.identity,main.transform);
        }
    }


}
