using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
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

        private IEnumerable<Position2D> vertices
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
            return Bonk.AABB.FromTransformedVertices(vertices, Transform2D);
        }

        public bool Equals(IShape2D other)
        {
            if (other is Line)
            {
                var otherLine = (Line)other;
                return v0.ToVector2() == otherLine.v0.ToVector2() && v1.ToVector2() == otherLine.v1.ToVector2();
            }

            return false;
        }
    }
}
