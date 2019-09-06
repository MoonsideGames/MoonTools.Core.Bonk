using System;
using Microsoft.Xna.Framework;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    public struct Rectangle : IShape2D, IEquatable<IShape2D>
    {
        public int MinX { get; }
        public int MinY { get; }
        public int MaxX { get; }
        public int MaxY { get; }

        private Position2D[] vertices;

        public Rectangle(int minX, int minY, int maxX, int maxY)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;

            vertices = new Position2D[4]
            {
                new Position2D(minX, minY),
                new Position2D(minX, maxY),
                new Position2D(maxX, minY),
                new Position2D(maxX, maxY)
            };
        }

        public Vector2 Support(Vector2 direction, Transform2D transform)
        {
            var furthestDistance = float.NegativeInfinity;
            var furthestVertex = Vector2.Transform(vertices[0], transform.TransformMatrix);

            foreach (var v in vertices)
            {
                var TransformedVertex = Vector2.Transform(v, transform.TransformMatrix);
                var distance = Vector2.Dot(TransformedVertex, direction);
                if (distance > furthestDistance)
                {
                    furthestDistance = distance;
                    furthestVertex = TransformedVertex;
                }
            }

            return furthestVertex;
        }

        public AABB AABB(Transform2D Transform2D)
        {
            return Bonk.AABB.FromTransformedVertices(vertices, Transform2D);
        }

        public bool Equals(IShape2D other)
        {
            if (other is Rectangle rectangle)
            {
                return MinX == rectangle.MinX &&
                    MinY == rectangle.MinY &&
                    MaxX == rectangle.MaxX &&
                    MaxY == rectangle.MaxY;
            }

            return false;
        }
    }
}
