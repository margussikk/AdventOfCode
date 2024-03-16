using AdventOfCode.Utilities.Graph;

namespace AdventOfCode.Year2019.Day18
{
    internal class VertexWalker(GraphVertex[] currentVertices)
    {
        public GraphVertex[] CurrentVertices { get; } = currentVertices;

        public int Steps { get; set; }

        public int Keys { get; set; }
    }
}
