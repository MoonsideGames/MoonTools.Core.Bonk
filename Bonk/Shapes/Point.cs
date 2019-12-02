using System;
using System.Linq;
using System.Numerics;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    public struct Point : IShape2D, IEquatable<IShape2D>
    {
        private Position2D position;

        public Point(Position2D position)
        {
            this.position = position;
        }

        public Point(int x, int y)
        {
            this.position = new Position2D(x, y);
        }

        public AABB AABB(Transform2D transform)
        {
            return Bonk.AABB.FromTransformedVertices(Enumerable.Repeat<Position2D>(position, 1), transform);
        }

        public Vector2 Support(Vector2 direction, Transform2D transform)
        {
            return Vector2.Transform(position.ToVector2(), transform.TransformMatrix);
        }

        public override bool Equals(object obj)
        {
            if (obj is IShape2D other)
            {
                return Equals(other);
            }

            return false;
        }

        public bool Equals(IShape2D other)
        {
            if (other is Point otherPoint)
            {
                return position == otherPoint.position;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(position);
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
