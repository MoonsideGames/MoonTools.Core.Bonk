using System;
using System.Collections.Generic;
using System.Numerics;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    /// <summary>
    /// A line is a shape defined by exactly two points in space.
    /// </summary>
    public struct Line : IShape2D, IEquatable<IShape2D>
    {
        private Position2D v0;
        private Position2D v1;

        private IEnumerable<Position2D> Vertices
        {
            get
            {
                yield return v0;
                yield return v0;
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
            if (obj is IShape2D other)
            {
                return Equals(other);
            }

            return false;
        }

        public bool Equals(IShape2D other)
        {
            if (other is Line otherLine)
            {
                return (v0 == otherLine.v0 && v1 == otherLine.v1) || (v1 == otherLine.v0 && v0 == otherLine.v1);
            }

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -851829407;
            hashCode = hashCode * -1521134295 + EqualityComparer<Position2D>.Default.GetHashCode(v0);
            hashCode = hashCode * -1521134295 + EqualityComparer<Position2D>.Default.GetHashCode(v1);
            hashCode = hashCode * -1521134295 + EqualityComparer<IEnumerable<Position2D>>.Default.GetHashCode(Vertices);
            return hashCode;
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
