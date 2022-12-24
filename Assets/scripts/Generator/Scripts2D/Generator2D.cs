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
        Hallway,
        DoorLeft,
        DoorRight,
        DoorTop,
        DoorBot,
        DoubleDoor
    }

    public class Room {
        public RectInt bounds;
        public System.Guid ID;
        public bool isFinal = false;
        public bool isBossRoom;
        public int hallwaysCount = 0;
        public int parallelCount = 0;
        public bool[] parallelSide = new bool[4] { true, true, true, true }; // left top right bottom
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
        GetRoomParallelHallway();
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
                Vector2Int prevPoint = path[0];
                for (int i = 0; i < path.Count; i++) {
                    var current = path[i];
                    
                    if (grid[current] == CellType.None) {
                        
                        if(grid[prevPoint] == CellType.Room)
                        {
                            grid[current] = GetRotationOfDoor(current, prevPoint);

                        }
                        else if (grid[path[i+1]] == CellType.Room)
                        {
                            grid[current] = GetRotationOfDoor(current, path[i + 1]);
                        }
                        else if (grid[path[i + 1]] == CellType.Room && grid[prevPoint] == CellType.Room)
                        {
                            grid[current] = CellType.DoubleDoor;
                        }
                        else
                        {
                            grid[current] = CellType.Hallway;
                        }
                    }


                    if (i > 0) {
                        var prev = path[i - 1];

                        var delta = current - prev;
                    }
                    prevPoint = current;
                }
            }
        }
    }

    CellType GetRotationOfDoor(Vector2Int position,Vector2Int prevPosition)
    {
        if (position.x + 1 == prevPosition.x)
            return CellType.DoorRight;
        if (position.x - 1 == prevPosition.x)
            return CellType.DoorLeft;
        if (position.y + 1 == prevPosition.y)
            return CellType.DoorTop;
        if (position.y - 1 == prevPosition.y)
            return CellType.DoorBot;
        return CellType.DoubleDoor;
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
        if(grid[x, y] == CellType.DoorTop)
        {
            roomPosition = new Vector2(x, y+1);
            return true;
        }
        if (grid[x, y] == CellType.DoorBot)
        {
            roomPosition = new Vector2(x, y - 1);
            return true;
        }
        if (grid[x, y] == CellType.DoorLeft)
        {
            roomPosition = new Vector2(x - 1, y);
            return true;
        }
        if (grid[x, y] == CellType.DoorRight)
        {
            roomPosition = new Vector2(x+1, y);
            return true;
        }
        if (grid[x, y] == CellType.DoubleDoor)
        {
            GetNeighbours(x, y, CellType.Room);
            return true;
        }
        return false;
        
    }
    void GetNeighbours(int x,int y,CellType cell)
    {
        if(grid.Size.x > x)
        {
            if (grid[x + 1, y] == cell)
            {
                roomPosition = new Vector2(x + 1, y);
                GetRoom(new Vector3(x-1, 0, y), 0).hallwaysCount++;
            }
        }
        if (x > 0)
        {
            if (grid[x - 1, y] == cell)
            {
                roomPosition = new Vector2(x - 1, y);
                GetRoom(new Vector3(x + 1, 0, y), 0).hallwaysCount++;
            }
        }
        if (y > 0)
        {
            if (grid[x , y - 1] == cell)
            {
                roomPosition = new Vector2(x, y - 1);
                GetRoom(new Vector3(x , 0, y+1), 0).hallwaysCount++;
            }
        }
        if (grid.Size.y > y)
        {
            if (grid[x, y + 1] == cell)
            {
                roomPosition = new Vector2(x, y + 1);
                GetRoom(new Vector3(x, 0, y - 1), 0).hallwaysCount++;
            }
        }
    }
    public Room GetRoomById(Guid id)
    {
        foreach(var room in rooms)
        {
            if(room.ID == id)
            {
                return room;
            }
        }
        return null;
    }

    void GetRoomParallelHallway()
    {

        foreach (var room in rooms)
        {
            for (int i = room.bounds.x; i < room.bounds.xMax; i++)
            {
                if (room.bounds.y > 0 && grid[i, room.bounds.y - 1] != CellType.Hallway)
                {
                    room.parallelSide[3] = false;
                }
                if (room.bounds.yMax < grid.Size.y - 1 && grid[i, room.bounds.yMax + 1] != CellType.Hallway)
                {
                    room.parallelSide[1] = false;
                }

            }
            for (int i = room.bounds.y; i < room.bounds.yMax; i++)
            {
                if (room.bounds.x > 0 && grid[room.bounds.x - 1, i] != CellType.Hallway)
                {
                    room.parallelSide[0] = false;
                }
                if (room.bounds.xMax < grid.Size.x - 1 && grid[room.bounds.xMax + 1, i] != CellType.Hallway)
                {
                    room.parallelSide[2] = false;
                }
            }
        }
    }
}
