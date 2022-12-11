﻿using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Model
{
    public class DelaunayAlgo
    {
        public List<Vertex> Vertices { get; private set; }
        public List<DelaunayEdge> Edges { get; private set; }
        public List<Triangle> Triangles { get; private set; }

        DelaunayAlgo()
        {
            Edges = new List<DelaunayEdge>();
            Triangles = new List<Triangle>();
        }

        public static DelaunayAlgo Triangulate(List<Vertex> vertices)
        {
            DelaunayAlgo delaunay = new DelaunayAlgo();
            delaunay.Vertices = new List<Vertex>(vertices);
            delaunay.Triangulate();
            return delaunay;
        }

        private void Triangulate()
        {
            float minX = Vertices[0].Position.x;
            float minY = Vertices[0].Position.y;
            float maxX = minX;
            float maxY = minY;

            foreach (var vertex in Vertices)
            {
                if (vertex.Position.x < minX) minX = vertex.Position.x;
                if (vertex.Position.x > maxX) maxX = vertex.Position.x;
                if (vertex.Position.y < minY) minY = vertex.Position.y;
                if (vertex.Position.y > maxY) maxY = vertex.Position.y;
            }

            float dx = maxX - minX;
            float dy = maxY - minY;
            float deltaMax = Mathf.Max(dx, dy) * 2;

            Vertex p1 = new Vertex(new Vector2(minX - 1, minY - 1));
            Vertex p2 = new Vertex(new Vector2(minX - 1, maxY + deltaMax));
            Vertex p3 = new Vertex(new Vector2(maxX + deltaMax, minY - 1));

            Triangles.Add(new Triangle(p1, p2, p3));

            foreach (var vertex in Vertices)
            {
                List<DelaunayEdge> polygon = new List<DelaunayEdge>();

                foreach (var t in Triangles)
                {
                    if (t.CircumCircleContains(vertex.Position))
                    {
                        t.IsBad = true;
                        polygon.Add(new DelaunayEdge(t.A, t.B));
                        polygon.Add(new DelaunayEdge(t.B, t.C));
                        polygon.Add(new DelaunayEdge(t.C, t.A));
                    }
                }

                Triangles.RemoveAll((Triangle t) => t.IsBad);

                for (int i = 0; i < polygon.Count; i++)
                {
                    for (int j = i + 1; j < polygon.Count; j++)
                    {
                        if (DelaunayEdge.AlmostEqual(polygon[i], polygon[j]))
                        {
                            polygon[i].IsBad = true;
                            polygon[j].IsBad = true;
                        }
                    }
                }

                polygon.RemoveAll((DelaunayEdge e) => e.IsBad);

                foreach (var edge in polygon)
                {
                    Triangles.Add(new Triangle(edge.U, edge.V, vertex));
                }
            }

            Triangles.RemoveAll((Triangle t) =>
                t.ContainsVertex(p1.Position) || t.ContainsVertex(p2.Position) || t.ContainsVertex(p3.Position));

            HashSet<DelaunayEdge> edgeSet = new HashSet<DelaunayEdge>();

            foreach (var t in Triangles)
            {
                var ab = new DelaunayEdge(t.A, t.B);
                var bc = new DelaunayEdge(t.B, t.C);
                var ca = new DelaunayEdge(t.C, t.A);

                if (edgeSet.Add(ab))
                {
                    Edges.Add(ab);
                }

                if (edgeSet.Add(bc))
                {
                    Edges.Add(bc);
                }

                if (edgeSet.Add(ca))
                {
                    Edges.Add(ca);
                }
            }
        }

        public static bool AlmostEqual(float x, float y)
        {
            return Mathf.Abs(x - y) <= float.Epsilon * Mathf.Abs(x + y) * 2
                   || Mathf.Abs(x - y) < float.MinValue;
        }

        public static bool AlmostEqual(Vertex left, Vertex right)
        {
            return AlmostEqual(left.Position.x, right.Position.x) && AlmostEqual(left.Position.y, right.Position.y);
        }
    }
}