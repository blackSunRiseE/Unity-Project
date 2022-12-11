using System.Collections.Generic;

namespace Scripts.Model
{
    public class PrimAlgo
    {
        public static List<Edge> MinimumSpanningTree(List<Edge> edges, Vertex start)
        {
            HashSet<Vertex> openSet = new HashSet<Vertex>();
            HashSet<Vertex> closedSet = new HashSet<Vertex>();
            foreach (var edge in edges)
            {
                openSet.Add(edge.U);
                openSet.Add(edge.V);
            }

            closedSet.Add(start);

            List<Edge> tree = new List<Edge>();

            while (openSet.Count > 0)
            {
                bool chosen = false;
                Edge chosenEdge = null;
                float minWeight = float.PositiveInfinity;

                foreach (var edge in edges)
                {
                    int closedVertices = 0;
                    if (!closedSet.Contains(edge.U)) closedVertices++;
                    if (!closedSet.Contains(edge.V)) closedVertices++;
                    if (closedVertices != 1) continue;

                    if (edge.Length < minWeight)
                    {
                        chosenEdge = edge;
                        chosen = true;
                        minWeight = edge.Length;
                    }
                }

                if (!chosen) break;
                tree.Add(chosenEdge);
                openSet.Remove(chosenEdge.U);
                openSet.Remove(chosenEdge.V);
                closedSet.Add(chosenEdge.U);
                closedSet.Add(chosenEdge.V);
            }

            return tree;
        }
    }
}