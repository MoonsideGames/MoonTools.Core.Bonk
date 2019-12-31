using System;
using System.Numerics;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    public struct Point : IShape2D, IEquatable<Point>
    {
        private Position2D _position;
        public AABB AABB { get; }

        public Point(Position2D position)
        {
            _position = position;
            AABB = new AABB(position, position);
        }

        public Point(int x, int y)
        {
            _position = new Position2D(x, y);
            AABB = new AABB(x, y, x, y);
        }

        public AABB TransformedAABB(Transform2D transform)
        {
            return AABB.Transformed(AABB, transform);
        }

        public Vector2 Support(Vector2 direction, Transform2D transform)
        {
            return Vector2.Transform(_position.ToVector2(), transform.TransformMatrix);
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
            return _position == other._position;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_position);
        }

        public static bool operator ==(Point a, Point b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Point a, Point b)
        {
            return !(a == b);
        }
    }
}
