using System;
using System.Collections.Generic;
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
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            foreach (var vertex in vertices)
            {
                var transformedVertex = Vector2.Transform(vertex, transform.TransformMatrix);

                if (transformedVertex.X < minX)
                {
                    minX = transformedVertex.X;
                }
                if (transformedVertex.Y < minY)
                {
                    minY = transformedVertex.Y;
                }
                if (transformedVertex.X > maxX)
                {
                    maxX = transformedVertex.X;
                }
                if (transformedVertex.Y > maxY)
                {
                    maxY = transformedVertex.Y;
                }
            }

            return new AABB
            {
                MinX = minX,
                MinY = minY,
                MaxX = maxX,
                MaxY = maxY
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
