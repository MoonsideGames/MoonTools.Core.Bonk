using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    /// <summary>
    /// Axis-aligned bounding box.
    /// </summary>
    public struct AABB : IEquatable<AABB>
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

        public override bool Equals(object obj)
        {
            return obj is AABB aabb && Equals(aabb);
        }

        public bool Equals(AABB other)
        {
            return MinX == other.MinX &&
                   MinY == other.MinY &&
                   MaxX == other.MaxX &&
                   MaxY == other.MaxY;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MinX, MinY, MaxX, MaxY);
        }

        public AABB(float minX, float minY, float maxX, float maxY)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
        }

        public static bool operator ==(AABB left, AABB right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(AABB left, AABB right)
        {
            return !(left == right);
        }
    }
}
