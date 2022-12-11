using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Scripts.Model
{
    public class Generator : MonoBehaviour
    {
        [SerializeField] Vector2Int size;
        [SerializeField] int roomCount;
        [SerializeField] Vector2Int roomMaxSize;
        [SerializeField] private GameObject cube;
        [SerializeField] private int seed;
        [SerializeField] Material redMaterial;
        [SerializeField] Material blueMaterial;
        private Random rnd;
        private List<Room> rooms;
        private HashSet<Edge> _edges;
        private Grid<CellType> _grid;
        private DelaunayAlgo _delaunayAlgo;

        void Start()
        {
            Generate();
        }

        void Generate()
        {
            rnd = new Random(seed);
            _grid = new Grid<CellType>(size, Vector2Int.zero);
            rooms = new List<Room>();

            PlaceRooms();
            Triangulate();
            CreateHallways();
            PathfindHallways();
            
        }

        void PlaceRooms()
        {
            for (int i = 0; rooms.Count < roomCount; i++)
            {
                Vector2Int location = new Vector2Int(
                    rnd.Next(0, size.x),
                    rnd.Next(0, size.y)
                );

                Vector2Int roomSize = new Vector2Int(
                    rnd.Next(1, roomMaxSize.x + 1),
                    rnd.Next(1, roomMaxSize.y + 1)
                );

                Room newRoom = new Room(location, roomSize);
                Room buf = new Room(location + new Vector2Int(-1, -1), roomSize + new Vector2Int(2, 2));
                bool addRoomFlag = true;

                foreach (var room in rooms)
                {
                    if (buf.Intersect(room))
                    {
                        addRoomFlag = false;
                        break;
                    }
                }

                if (newRoom.bounds.xMin < 0 || newRoom.bounds.xMax >= size.x
                                            || newRoom.bounds.yMin < 0 || newRoom.bounds.yMax >= size.y)
                {
                    addRoomFlag = false;
                }

                if (addRoomFlag)
                {
                    rooms.Add(newRoom);
                    PlaceRoom(newRoom.bounds.position, newRoom.bounds.size);

                    foreach (var pos in newRoom.bounds.allPositionsWithin)
                    {
                        _grid[pos] = CellType.R;
                    }
                }
            }
        }

        void Triangulate()
        {
            List<Vertex> vertices = new List<Vertex>();

            foreach (var room in rooms)
            {
                vertices.Add(new DelaunayVertex<Room>((room.bounds.position + room.bounds.size) / 2, room));
            }

            _delaunayAlgo = DelaunayAlgo.Triangulate(vertices);
        }

        void CreateHallways()
        {
            List<Edge> edges = new List<Edge>();

            foreach (var edge in _delaunayAlgo.Edges)
            {
                edges.Add(new Edge(edge.U, edge.V));
            }

            List<Edge> mst = PrimAlgo.MinimumSpanningTree(edges, edges[0].U);

            _edges = new HashSet<Edge>(mst);
            var remainingEdges = new HashSet<Edge>(edges);
            remainingEdges.ExceptWith(_edges);

            foreach (var edge in remainingEdges)
            {
                if (rnd.NextDouble() < 0.125)
                {
                    _edges.Add(edge);
                }
            }
        }

        void PathfindHallways()
        {
            Pathfinder aStar = new Pathfinder(size);

            foreach (var edge in _edges)
            {
                var startRoom = (edge.U as DelaunayVertex<Room>).Item;
                var endRoom = (edge.V as DelaunayVertex<Room>).Item;

                var startPosf = startRoom.bounds.center;
                var endPosf = endRoom.bounds.center;
                var startPos = new Vector2Int((int)startPosf.x, (int)startPosf.y);
                var endPos = new Vector2Int((int)endPosf.x, (int)endPosf.y);

                var path = aStar.FindPath(startPos, endPos, (Node a,
                    Node b) =>
                {
                    var pathCost = new Pathfinder.PathCost();

                    pathCost.cost = Vector2Int.Distance(b.Position, endPos); //heuristic

                    if (_grid[b.Position] == CellType.R)
                    {
                        pathCost.cost += 10;
                    }
                    else if (_grid[b.Position] == CellType.N)
                    {
                        pathCost.cost += 5;
                    }
                    else if (_grid[b.Position] == CellType.H)
                    {
                        pathCost.cost += 1;
                    }

                    pathCost.traversable = true;

                    return pathCost;
                });

                if (path != null)
                {
                    for (int i = 0; i < path.Count; i++)
                    {
                        var current = path[i];

                        if (_grid[current] == CellType.N)
                        {
                            _grid[current] = CellType.H;
                        }

                        if (i > 0)
                        {
                            var prev = path[i - 1];

                            var delta = current - prev;
                        }
                    }

                    foreach (var pos in path)
                    {
                        if (_grid[pos] == CellType.H)
                        {
                            PlaceHallway(pos);
                        }
                    }
                }
            }
        }

        void PlaceCube(Vector2Int location, Vector2Int size, Material material)
        {
            GameObject go = Instantiate(cube, new Vector3(location.x, 0, location.y), Quaternion.identity);
                go.GetComponent<Transform>().localScale = new Vector3(size.x, 1, size.y);
            go.GetComponent<MeshRenderer>().material = material;
        }

        void PlaceRoom(Vector2Int location, Vector2Int size)
        {
            PlaceCube(location, size, redMaterial);
        }

        void PlaceHallway(Vector2Int location)
        {
            PlaceCube(location, new Vector2Int(1, 1), blueMaterial);
        }
    }
}