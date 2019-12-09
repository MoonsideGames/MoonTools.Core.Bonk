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
        private Position2D v0;
        private Position2D v1;

        public IEnumerable<Position2D> Vertices
        {
            get
            {
                yield return v0;
                yield return v1;
            }
        }

        public Line(Position2D start, Position2D end)
        {
            v0 = start;
            v1 = end;
        }

        public Vector2 Support(Vector2 direction, Transform2D transform)
        {
            var TransformedStart = Vector2.Transform(v0, transform.TransformMatrix);
            var TransformedEnd = Vector2.Transform(v1, transform.TransformMatrix);
            return Vector2.Dot(TransformedStart, direction) > Vector2.Dot(TransformedEnd, direction) ?
                TransformedStart :
                TransformedEnd;
        }

        public AABB AABB(Transform2D Transform2D)
        {
            return Bonk.AABB.FromTransformedVertices(Vertices, Transform2D);
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
            return (v0 == other.v0 && v1 == other.v1) || (v1 == other.v0 && v0 == other.v1);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(v0, v1);
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
