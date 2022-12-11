using UnityEngine;

namespace Scripts.Model
{
    public class Node
    {
        public Vector2Int Position { get; private set; }
        public Node Previous { get; set; }
        public float Cost { get; set; }

        public Node(Vector2Int position) {
            Position = position;
        }
    }
}