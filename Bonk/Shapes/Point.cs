using System;
using System.Numerics;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    public struct Point : IShape2D, IEquatable<Point>
    {
        private Position2D Position { get; }
        public AABB AABB { get; }

        public AABB TransformedAABB(Transform2D transform)
        {
            return AABB.Transformed(AABB, transform);
        }

        public Vector2 Support(Vector2 direction, Transform2D transform)
        {
            return Vector2.Transform(Position.ToVector2(), transform.TransformMatrix);
        }

        public override bool Equals(object obj)
        {
            return obj is IShape2D other && Equals(other);
        }

        public bool Equals(IShape2D other)
        {
            return other is Point otherPoint && Equals(otherPoint);
        }

        public bool Equals(Point other)
        {
            return Position == other.Position;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position);
        }

        public static bool operator ==(Point a, Point b)
        {
            return true;
        }

        public static bool operator !=(Point a, Point b)
        {
            return false;
        }
    }
}
