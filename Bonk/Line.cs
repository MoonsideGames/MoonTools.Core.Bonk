using System;
using Microsoft.Xna.Framework;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    public struct Line : IShape2D, IEquatable<IShape2D>
    {
        private Position2D[] vertices;

        public Line(Position2D start, Position2D end)
        {
            vertices = new Position2D[2] { start, end };
        }

        public Vector2 Support(Vector2 direction, Transform2D transform)
        {
            var Transform2DedStart = Vector2.Transform(vertices[0], transform.TransformMatrix);
            var Transform2DedEnd = Vector2.Transform(vertices[1], transform.TransformMatrix);
            return Vector2.Dot(Transform2DedStart, direction) > Vector2.Dot(Transform2DedEnd, direction) ?
                Transform2DedStart :
                Transform2DedEnd;
        }

        public AABB AABB(Transform2D Transform2D)
        {
            return Bonk.AABB.FromTransform2DedVertices(vertices, Transform2D);
        }

        public bool Equals(IShape2D other)
        {
            if (other is Line)
            {
                var otherLine = (Line)other;
                return vertices[0].ToVector2() == otherLine.vertices[0].ToVector2() && vertices[1].ToVector2() == otherLine.vertices[1].ToVector2();
            }

            return false;
        }
    }
}
