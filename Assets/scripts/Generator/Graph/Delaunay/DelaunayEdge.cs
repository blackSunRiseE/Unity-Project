namespace Scripts.Model
{
    public class DelaunayEdge : Edge
    {
        public bool IsBad { get; set; }

        public DelaunayEdge()
        {
        }

        public DelaunayEdge(Vertex u, Vertex v) : base(u, v)
        {
        }

        public static bool AlmostEqual(Edge left, Edge right)
        {
            return DelaunayAlgo.AlmostEqual(left.U, right.U) && DelaunayAlgo.AlmostEqual(left.V, right.V)
                   || DelaunayAlgo.AlmostEqual(left.U, right.V) && DelaunayAlgo.AlmostEqual(left.V, right.U);
        }
    }
}