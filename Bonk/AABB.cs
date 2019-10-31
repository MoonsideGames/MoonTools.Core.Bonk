using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    /// <summary>
    /// Axis-aligned bounding box.
    /// </summary>
    public struct AABB
    {
        public float MinX { get; private set; }
        public float MinY { get; private set; }
        public float MaxX { get; private set; }
        public float MaxY { get; private set; }

        public float Width { get { return MaxX - MinX; } }
        public float Height { get { return MaxY - MinY; } }

        public static AABB FromTransformedVertices(IEnumerable<Position2D> vertices, Transform2D transform)
        {
            var TransformedVertices = vertices.Select(vertex => Vector2.Transform(vertex, transform.TransformMatrix));

            return new AABB
            {
                MinX = TransformedVertices.Min(vertex => vertex.X),
                MinY = TransformedVertices.Min(vertex => vertex.Y),
                MaxX = TransformedVertices.Max(vertex => vertex.X),
                MaxY = TransformedVertices.Max(vertex => vertex.Y)
            };
        }

        public AABB(float minX, float minY, float maxX, float maxY)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
        }
    }
}