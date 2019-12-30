using System;
using System.Collections.Generic;
using System.Numerics;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    /// <summary>
    /// A rectangle is a shape defined by a minimum and maximum X value and a minimum and maximum Y value.
    /// </summary>
    public struct Rectangle : IShape2D, IEquatable<Rectangle>
    {
        public int MinX { get; }
        public int MinY { get; }
        public int MaxX { get; }
        public int MaxY { get; }

        public AABB AABB { get; }

        public IEnumerable<Position2D> Vertices
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

            AABB = new AABB(minX, minY, maxX, maxY);
        }

        public Vector2 Support(Vector2 direction, Transform2D transform)
        {
            var maxDotProduct = float.NegativeInfinity;
            var maxVertex = new Vector2(MinX, MinY);
            foreach (var vertex in Vertices)
            {
                var transformed = Vector2.Transform(vertex, transform.TransformMatrix);
                var dot = Vector2.Dot(transformed, direction);
                if (dot > maxDotProduct)
                {
                    maxVertex = transformed;
                    maxDotProduct = dot;
                }
            }
            return maxVertex;
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
            return (other is Rectangle rectangle && Equals(rectangle)) || (other is Polygon polygon && Equals(polygon));
        }

        public bool Equals(Rectangle other)
        {
            return MinX == other.MinX &&
                MinY == other.MinY &&
                MaxX == other.MaxX &&
                MaxY == other.MaxY;
        }

        public bool Equals(Polygon other)
        {
            return RectanglePolygonComparison.Equals(other, this);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MinX, MinY, MaxX, MaxY);
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
