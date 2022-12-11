using UnityEngine;

namespace Scripts.Model
{
    public class Vertex
    {
        public Vector2 Position { get; private set; }

        public Vertex()
        {
        }

        public Vertex(Vector2 position)
        {
            Position = position;
        }
        
    }
}