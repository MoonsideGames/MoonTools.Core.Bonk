using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    internal static class RectanglePolygonComparison
    {
        public static bool Equals(Polygon polygon, Rectangle rectangle)
        {
            if (polygon.VertexCount != 4) { return false; }

            int? minIndex = null;
            for (var i = 0; i < 4; i++)
            {
                if (polygon.Vertices[i] == rectangle.Min) { minIndex = i; break; }
            }

            if (!minIndex.HasValue) { return false; }

            return
                polygon.Vertices[(minIndex.Value + 1) % 4] == rectangle.TopRight &&
                polygon.Vertices[(minIndex.Value + 2) % 4] == rectangle.Max &&
                polygon.Vertices[(minIndex.Value + 3) % 4] == rectangle.BottomLeft;
        }
    }
}
