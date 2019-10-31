using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using MoonTools.Core.Structs;
using MoreLinq;

namespace MoonTools.Core.Bonk
{
    /// <summary>
    /// A rectangle is a shape defined by a minimum and maximum X value and a minimum and maximum Y value.
    /// </summary>
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
            if (other is Rectangle rectangle)
            {
                return MinX == rectangle.MinX &&
                    MinY == rectangle.MinY &&
                    MaxX == rectangle.MaxX &&
                    MaxY == rectangle.MaxY;
            }

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -1260800952;
            hashCode = hashCode * -1521134295 + MinX.GetHashCode();
            hashCode = hashCode * -1521134295 + MinY.GetHashCode();
            hashCode = hashCode * -1521134295 + MaxX.GetHashCode();
            hashCode = hashCode * -1521134295 + MaxY.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<IEnumerable<Position2D>>.Default.GetHashCode(vertices);
            return hashCode;
        }

        public static bool operator ==(Rectangle a, Rectangle b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Rectangle a, Rectangle b)
        {
            return !(a == b);
        }
    }
}
