using System;
using System.Collections.Generic;
using System.Numerics;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    /// <summary>
    /// A line is a shape defined by exactly two points in space.
    /// </summary>
    public struct Line : IShape2D, IEquatable<Line>
    {
        private Position2D _v0;
        private Position2D _v1;

        public AABB AABB { get; }

        public IEnumerable<Position2D> Vertices
        {
            get
            {
                yield return _v0;
                yield return _v1;
            }
        }

        public Line(Position2D start, Position2D end)
        {
            _v0 = start;
            _v1 = end;

            AABB = new AABB(Math.Min(_v0.X, _v1.X), Math.Min(_v0.Y, _v1.Y), Math.Max(_v0.X, _v1.X), Math.Max(_v0.Y, _v1.Y));
        }

        public Vector2 Support(Vector2 direction, Transform2D transform)
        {
            var transformedStart = Vector2.Transform(_v0, transform.TransformMatrix);
            var transformedEnd = Vector2.Transform(_v1, transform.TransformMatrix);
            return Vector2.Dot(transformedStart, direction) > Vector2.Dot(transformedEnd, direction) ?
                transformedStart :
                transformedEnd;
        }

        public AABB TransformedAABB(Transform2D transform)
        {
            return AABB.Transformed(AABB, transform);
        }

        public override bool Equals(object obj)
        {
            return obj is IShape2D other && Equals(other);
        }

        public bool Equals(IShape2D other)
        {
            return other is Line otherLine && Equals(otherLine);
        }

        public bool Equals(Line other)
        {
            return (_v0 == other._v0 && _v1 == other._v1) || (_v1 == other._v0 && _v0 == other._v1);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_v0, _v1);
        }

        public static bool operator ==(Line a, Line b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Line a, Line b)
        {
            return !(a == b);
        }
    }
}
