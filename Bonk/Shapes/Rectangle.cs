using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MoonTools.Core.Structs;
using MoreLinq;

namespace MoonTools.Core.Bonk
{
    public struct Rectangle : IShape2D, IEquatable<IShape2D>
    {
        public int MinX { get; }
        public int MinY { get; }
        public int MaxX { get; }
        public int MaxY { get; }

        private IEnumerable<Position2D> vertices
        {
            get
            {
                yield return new Position2D(MinX, MinY);
                yield return new Position2D(MinX, MaxY);
                yield return new Position2D(MaxX, MinY);
                yield return new Position2D(MaxX, MaxY);
            }
        }

        public Rectangle(int minX, int minY, int maxX, int maxY)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
        }

        public Vector2 Support(Vector2 direction, Transform2D transform)
        {
            return vertices.Select(vertex => Vector2.Transform(vertex, transform.TransformMatrix)).MaxBy(transformed => Vector2.Dot(transformed, direction)).First();
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
