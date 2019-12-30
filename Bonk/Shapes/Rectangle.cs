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

        private Vector2 Support(Vector2 direction)
        {
            if (direction.X >= 0 && direction.Y >= 0)
            {
                return new Vector2(MaxX, MaxY);
            }
            else if (direction.X >= 0 && direction.Y < 0)
            {
                return new Vector2(MaxX, MinY);
            }
            else if (direction.X < 0 && direction.Y >= 0)
            {
                return new Vector2(MinX, MaxY);
            }
            else if (direction.X < 0 && direction.Y < 0)
            {
                return new Vector2(MinX, MinY);
            }
            else
            {
                throw new ArgumentException("Support vector direction cannot be zero.");
            }
        }

        public Vector2 Support(Vector2 direction, Transform2D transform)
        {
            Matrix4x4 inverseTransform;
            Matrix4x4.Invert(transform.TransformMatrix, out inverseTransform);
            var inverseDirection = Vector2.TransformNormal(direction, inverseTransform);
            return Vector2.Transform(Support(inverseDirection), transform.TransformMatrix);
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
