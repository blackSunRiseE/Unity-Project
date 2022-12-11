using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Scripts.Model
{
    public class Room
    {
        public GUID ID;
        public RectInt bounds;
        public Vector2Int center;

        public Room()
        {
        }

        public Room(Vector2Int position, Vector2Int size)
        {
            ID = GUID.Generate();
            bounds.size = size;
            bounds.position = position;
            center = new Vector2Int((position.x + size.x) / 2, (position.y + size.y) / 2);
        }

        public Vector2 Size
        {
            get { return bounds.size; }
        }

        public Vector2 Position
        {
            get { return bounds.position; }
        }

        public bool Overlap(RectInt r)
        {
            return bounds.Overlaps(r);
        }

        public bool Intersect(Room that)
        {
            return !((this.bounds.position.x >= (that.bounds.position.x + that.bounds.size.x))
                     || ((this.bounds.position.x + this.bounds.size.x) <= that.bounds.position.x)
                     || (this.bounds.position.y >= (that.bounds.position.y + that.bounds.size.y))
                     || ((this.bounds.position.y + this.bounds.size.y) <= that.bounds.position.y));
        }
    }
}