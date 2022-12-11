using UnityEngine;

namespace Scripts.Model
{
    public class Triangle
    {
        public Vertex A { get; set; }
        public Vertex B { get; set; }
        public Vertex C { get; set; }
        public bool IsBad { get; set; }
        
        public Triangle() {

        }

        public Triangle(Vertex a, Vertex b, Vertex c) {
            A = a;
            B = b;
            C = c;
        }

        public bool ContainsVertex(Vector3 v) {
            return Vector2.Distance(v, A.Position) < 0.01f
                   || Vector2.Distance(v, B.Position) < 0.01f
                   || Vector2.Distance(v, C.Position) < 0.01f;
        }

        public bool CircumCircleContains(Vector2 v) {
            Vector2 a = A.Position;
            Vector2 b = B.Position;
            Vector2 c = C.Position;

            float ab = a.sqrMagnitude;
            float cd = b.sqrMagnitude;
            float ef = c.sqrMagnitude;

            float circumX = (ab * (c.y - b.y) + cd * (a.y - c.y) + ef * (b.y - a.y)) / (a.x * (c.y - b.y) + b.x * (a.y - c.y) + c.x * (b.y - a.y));
            float circumY = (ab * (c.x - b.x) + cd * (a.x - c.x) + ef * (b.x - a.x)) / (a.y * (c.x - b.x) + b.y * (a.x - c.x) + c.y * (b.x - a.x));

            Vector2 circum = new Vector2(circumX / 2, circumY / 2);
            float circumRadius = Vector2.SqrMagnitude(a - circum);
            float dist = Vector2.SqrMagnitude(v - circum);
            return dist <= circumRadius;
        }

    }
}