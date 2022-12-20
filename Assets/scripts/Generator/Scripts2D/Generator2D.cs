using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using Graphs;
using System;

public class Generator2D : MonoBehaviour {
    public enum CellType {
        None,
        Room,
        Hallway
    }

    public class Room {
        public RectInt bounds;
        public System.Guid ID;
        public bool isFinal = false;
        public bool isBossRoom;
        public int hallwaysCount = 0;
        public Room(Vector2Int location, Vector2Int size) {
            bounds = new RectInt(location, size);
            ID = System.Guid.NewGuid();
        }

        public static bool Intersect(Room a, Room b) {
            return !((a.bounds.position.x >= (b.bounds.position.x + b.bounds.size.x)) || ((a.bounds.position.x + a.bounds.size.x) <= b.bounds.position.x)
                || (a.bounds.position.y >= (b.bounds.position.y + b.bounds.size.y)) || ((a.bounds.position.y + a.bounds.size.y) <= b.bounds.position.y));
        }

        public bool IsInside(Vector3 playerPosition,float offset)
        {
            return bounds.xMin + offset <= playerPosition.x && bounds.xMax - offset >= playerPosition.x && bounds.yMin + offset <= playerPosition.z && bounds.yMax - offset >= playerPosition.z;

        }
    }

    [SerializeField]
    Vector2Int size;
    [SerializeField]
    int roomCount;
    [SerializeField]
    Vector2Int roomMaxSize;
    [SerializeField]
    Vector2Int roomMinSize;
    [SerializeField]
    GameObject cubePrefab;
    [SerializeField]
    Material redMaterial;
    [SerializeField]
    Material blueMaterial;

    Random random;
    public Grid2D<CellType> grid { get; set; }
    public List<Room> rooms { get;  set; }
    Delaunay2D delaunay;
    HashSet<Prim.Edge> selectedEdges;
    Vector2 roomPosition;

    public void Generate(int seed) {
        random = new Random(seed);
        grid = new Grid2D<CellType>(size, Vector2Int.zero);
        rooms = new List<Room>();

        PlaceRooms();
        Triangulate();
        CreateHallways();
        PathfindHallways();
    }

    void PlaceRooms() {
        for (int i = 0; roomCount > rooms.Count; i++) {
            Vector2Int location = new Vector2Int(
                random.Next(0, size.x),
                random.Next(0, size.y)
            );

            Vector2Int roomSize = new Vector2Int(
                random.Next(roomMinSize.x, roomMaxSize.x + 1),
                random.Next(roomMinSize.y, roomMaxSize.y + 1)
            );

            bool add = true;
            Room newRoom = new Room(location, roomSize);
            Room buffer = new Room(location + new Vector2Int(-1, -1), roomSize + new Vector2Int(2, 2));

            foreach (var room in rooms) {
                if (Room.Intersect(room, buffer)) {
                    add = false;
                    break;
                }
            }

            if (newRoom.bounds.xMin < 0 || newRoom.bounds.xMax >= size.x
                || newRoom.bounds.yMin < 0 || newRoom.bounds.yMax >= size.y) {
                add = false;
            }

            if (add) {
                rooms.Add(newRoom);
                //PlaceRoom(newRoom.bounds.position, newRoom.bounds.size);

                foreach (var pos in newRoom.bounds.allPositionsWithin) {
                    grid[pos] = CellType.Room;
                }
            }
        }
    }

    void Triangulate() {
        List<Vertex> vertices = new List<Vertex>();

        foreach (var room in rooms) {
            vertices.Add(new Vertex<Room>((Vector2)room.bounds.position + ((Vector2)room.bounds.size) / 2, room));
        }

        delaunay = Delaunay2D.Triangulate(vertices);
    }

    void CreateHallways() {
        List<Prim.Edge> edges = new List<Prim.Edge>();

        foreach (var edge in delaunay.Edges) {
            edges.Add(new Prim.Edge(edge.U, edge.V));
        }

        List<Prim.Edge> mst = Prim.MinimumSpanningTree(edges, edges[0].U);

        selectedEdges = new HashSet<Prim.Edge>(mst);
        var remainingEdges = new HashSet<Prim.Edge>(edges);
        remainingEdges.ExceptWith(selectedEdges);

        foreach (var edge in remainingEdges) {
            if (random.NextDouble() < 0.125) {
                selectedEdges.Add(edge);
            }
        }
    }

    void PathfindHallways() {
        DungeonPathfinder2D aStar = new DungeonPathfinder2D(size);

        foreach (var edge in selectedEdges) {
            var startRoom = (edge.U as Vertex<Room>).Item;
            var endRoom = (edge.V as Vertex<Room>).Item;

            var startPosf = startRoom.bounds.center;
            var endPosf = endRoom.bounds.center;
            var startPos = new Vector2Int((int)startPosf.x, (int)startPosf.y);
            var endPos = new Vector2Int((int)endPosf.x, (int)endPosf.y);

            var path = aStar.FindPath(startPos, endPos, (DungeonPathfinder2D.Node a, DungeonPathfinder2D.Node b) => {
                var pathCost = new DungeonPathfinder2D.PathCost();
                
                pathCost.cost = Vector2Int.Distance(b.Position, endPos);    //heuristic

                if (grid[b.Position] == CellType.Room) {
                    pathCost.cost += 10;
                } else if (grid[b.Position] == CellType.None) {
                    pathCost.cost += 5;
                } else if (grid[b.Position] == CellType.Hallway) {
                    pathCost.cost += 1;
                }

                pathCost.traversable = true;

                return pathCost;
            });

            if (path != null) {
                for (int i = 0; i < path.Count; i++) {
                    var current = path[i];

                    if (grid[current] == CellType.None) {
                        grid[current] = CellType.Hallway;
                    }

                    if (i > 0) {
                        var prev = path[i - 1];

                        var delta = current - prev;
                    }
                }

                /*foreach (var pos in path) {
                    if (grid[pos] == CellType.Hallway) {
                        PlaceHallway(pos);
                    }
                }*/
            }
        }
    }
    public Room GetRoom(Vector3 playerPosition,float offset)
    {
        for(int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].IsInside(playerPosition,offset))
            {
                return rooms[i];
            }
        }
        return null;
    }

    public void ChooseBossRoom()
    {
        for (int i = 0; i < grid.Size.x; i++)
        {
            for (int j = 0; j < grid.Size.y; j++)
            {
                if (FindDoor(i, j))
                {
                    GetRoom(new Vector3(roomPosition.x, 0, roomPosition.y),0).hallwaysCount++;
                }
            }
        }
        for(int i = 0; i < rooms.Count;i++)
        {
            if (rooms[i].hallwaysCount == 1 && i != 0)
            {
                rooms[i].isBossRoom = true;
                break;
            }
        }
    }
    bool FindDoor(int x,int y)
    {
        if(grid[x, y] == CellType.Hallway)
        {
            if (CountNeighbour(x, y, CellType.Hallway) == 1 && CountNeighbour(x, y, CellType.Room) == 1 )
            {
                return true;
            }
        }
        return false;
        
    }
    int CountNeighbour(int x,int y,CellType cell)
    {
        int counter = 0;
        if(grid.Size.x > x)
        {
            if (grid[x + 1, y] == cell)
            {
                roomPosition = new Vector2(x + 1, y);
                counter++;
            }
        }
        if (x > 0)
        {
            if (grid[x - 1, y] == cell)
            {
                roomPosition = new Vector2(x - 1, y);
                counter++;
            }
        }
        if (y > 0)
        {
            if (grid[x , y - 1] == cell)
            {
                roomPosition = new Vector2(x, y - 1);
                counter++;
            }
        }
        if (grid.Size.y > y)
        {
            if (grid[x, y + 1] == cell)
            {
                roomPosition = new Vector2(x, y + 1);
                counter++;
            }
        }
        return counter;
    }
}
