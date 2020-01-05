using System;
using System.Numerics;

namespace MoonTools.Core.Bonk
{
    /// <summary>
    /// A Minkowski difference between two shapes.
    /// </summary>
    public struct MinkowskiDifference<T, U> : IEquatable<MinkowskiDifference<T, U>> where T : struct, IShape2D where U : struct, IShape2D
    {
        private TransformedShape2D<T> ShapeA { get; }
        private TransformedShape2D<U> ShapeB { get; }

        public MinkowskiDifference(TransformedShape2D<T> shapeA, TransformedShape2D<U> shapeB)
        {
            ShapeA = shapeA;
            ShapeB = shapeB;
        }

        public Vector2 Support(Vector2 direction)
        {
            return ShapeA.Support(direction) - ShapeB.Support(-direction);
        }

        public override bool Equals(object other)
        {
            return other is MinkowskiDifference<T, U> minkowskiDifference && Equals(minkowskiDifference);
        }

        public bool Equals(MinkowskiDifference<T, U> other)
        {
            return
                ShapeA == other.ShapeA &&
                ShapeB == other.ShapeB;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ShapeA, ShapeB);
        }

        public static bool operator ==(MinkowskiDifference<T, U> a, MinkowskiDifference<T, U> b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(MinkowskiDifference<T, U> a, MinkowskiDifference<T, U> b)
        {
            return !(a == b);
        }
    }
}
