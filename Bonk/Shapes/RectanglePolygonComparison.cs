using System.Linq;

namespace MoonTools.Core.Bonk
{
    internal static class RectanglePolygonComparison
    {
        public static bool Equals(Polygon polygon, Rectangle rectangle)
        {
            var q = from a in polygon.Vertices
                    join b in rectangle.Vertices on a equals b
                    select a;

            return polygon.VertexCount == 4 && q.Count() == 4;
        }
    }
}
