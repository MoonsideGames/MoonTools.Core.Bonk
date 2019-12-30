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
        public Vector2 Min { get; private set; }
        public Vector2 Max { get; private set; }

        public float Width { get { return Max.X - Min.X; } }
        public float Height { get { return Max.Y - Min.Y; } }

        public AABB(float minX, float minY, float maxX, float maxY)
        {
            Min = new Vector2(minX, minY);
            Max = new Vector2(maxX, maxY);
        }

        public AABB(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;
        }

        private static Matrix4x4 AbsoluteMatrix(Matrix4x4 matrix)
        {
            return new Matrix4x4
            (
                Math.Abs(matrix.M11), Math.Abs(matrix.M12), Math.Abs(matrix.M13), Math.Abs(matrix.M14),
                Math.Abs(matrix.M21), Math.Abs(matrix.M22), Math.Abs(matrix.M23), Math.Abs(matrix.M24),
                Math.Abs(matrix.M31), Math.Abs(matrix.M32), Math.Abs(matrix.M33), Math.Abs(matrix.M34),
                Math.Abs(matrix.M41), Math.Abs(matrix.M42), Math.Abs(matrix.M43), Math.Abs(matrix.M44)
            );
        }

        public static AABB Transformed(AABB aabb, Transform2D transform)
        {
            var center = (aabb.Min + aabb.Max) / 2f;
            var extent = (aabb.Max - aabb.Min) / 2f;

            var newCenter = Vector2.Transform(center, transform.TransformMatrix);
            var newExtent = Vector2.TransformNormal(extent, AbsoluteMatrix(transform.TransformMatrix));

            return new AABB(newCenter - newExtent, newCenter + newExtent);
        }

        public static AABB FromVertices(IEnumerable<Position2D> vertices)
        {
            var minX = float.MaxValue;
            var minY = float.MaxValue;
            var maxX = float.MinValue;
            var maxY = float.MinValue;

            foreach (var vertex in vertices)
            {
                if (vertex.X < minX)
                {
                    minX = vertex.X;
                }
                if (vertex.Y < minY)
                {
                    minY = vertex.Y;
                }
                if (vertex.X > maxX)
                {
                    maxX = vertex.X;
                }
                if (vertex.Y > maxY)
                {
                    maxY = vertex.Y;
                }
            }

            return new AABB(minX, minY, maxX, maxY);
        }

        public override bool Equals(object obj)
        {
            return obj is AABB aabb && Equals(aabb);
        }

        public bool Equals(AABB other)
        {
            return Min == other.Min &&
                   Max == other.Max;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Min, Max);
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
