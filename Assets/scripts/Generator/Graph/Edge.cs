using System.Numerics;
using UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;

namespace Scripts.Model
{
    public class Edge
    {
        public float Length { get; }
        public Vertex U { get; set; }
        public Vertex V { get; set; }

        public Edge()
        {
        }

        public Edge(Vertex u, Vertex v)
        {
            Length = Vector2.Distance(u.Position, v.Position);
            U = u;
            V = v;
        }
    }
}