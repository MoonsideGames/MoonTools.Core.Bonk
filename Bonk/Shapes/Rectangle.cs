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
        /// <summary>
        /// The minimum position of the rectangle. Note that we assume y-down coordinates.
        /// </summary>
        /// <value></value>
        public Position2D Min { get; }
        /// <summary>
        /// The maximum position of the rectangle. Note that we assume y-down coordinates.
        /// </summary>
        /// <value></value>
        public Position2D Max { get; }
        public AABB AABB { get; }

        public int Left { get { return Min.X; } }
        public int Right { get { return Max.X; } }
        public int Top { get { return Min.Y; } }
        public int Bottom { get { return Max.Y; } }

        public int Width { get; }
        public int Height { get; }

        public Position2D TopRight { get { return new Position2D(Right, Top); } }
        public Position2D BottomLeft { get { return new Position2D(Left, Bottom); } }

        public IEnumerable<Position2D> Vertices
        {
            get
            {
                yield return new Position2D(Min.X, Min.Y);
                yield return new Position2D(Min.X, Max.Y);
                yield return new Position2D(Max.X, Min.Y);
                yield return new Position2D(Max.X, Max.Y);
            }
        }

        public Rectangle(int minX, int minY, int maxX, int maxY)
        {
            Min = new Position2D(minX, minY);
            Max = new Position2D(maxX, maxY);
            AABB = new AABB(minX, minY, maxX, maxY);
            Width = Max.X - Min.X;
            Height = Max.Y - Min.Y;
        }

        private Vector2 Support(Vector2 direction)
        {
            if (direction.X >= 0 && direction.Y >= 0)
            {
                return Max;
            }
            else if (direction.X >= 0 && direction.Y < 0)
            {
                return new Vector2(Max.X, Min.Y);
            }
            else if (direction.X < 0 && direction.Y >= 0)
            {
                return new Vector2(Min.X, Max.Y);
            }
            else if (direction.X < 0 && direction.Y < 0)
            {
                return new Vector2(Min.X, Min.Y);
            }
            else
            {
                throw new ArgumentException("Support vector direction cannot be zero.");
            }
        }

        public Vector2 Support(Vector2 direction, Transform2D transform)
        {
            Matrix3x2 inverseTransform;
            Matrix3x2.Invert(transform.TransformMatrix, out inverseTransform);
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
            return Min == other.Min && Max == other.Max;
        }

        public bool Equals(Polygon other)
        {
            return RectanglePolygonComparison.Equals(other, this);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Min, Max);
        }

        public static bool operator ==(Rectangle a, Rectangle b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Rectangle a, Rectangle b)
        {
            return !(a == b);
        }

        public static bool operator ==(Rectangle a, Polygon b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Rectangle a, Polygon b)
        {
            return !(a == b);
        }
    }
}
