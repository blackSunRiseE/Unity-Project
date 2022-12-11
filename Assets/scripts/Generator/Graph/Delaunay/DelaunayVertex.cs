using UnityEngine;

namespace Scripts.Model
{
    public class DelaunayVertex<T> : Vertex
    {
        public T Item { get; private set; }

        public DelaunayVertex(T item)
        {
            Item = item;
        }

        public DelaunayVertex(Vector2 position, T item) : base(position)
        {
            Item = item;
        }
    }
}