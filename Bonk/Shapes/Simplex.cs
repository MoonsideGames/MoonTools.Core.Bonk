using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MoonTools.Core.Structs;
using MoonTools.Core.Bonk.Extensions;

namespace MoonTools.Core.Bonk
{
    /// <summary>
    /// A simplex is a shape used to calculate overlap. It is defined by a Minkowski difference and two direction vectors.
    /// </summary>
    public struct Simplex : IShape2D
    {
        MinkowskiDifference minkowskiDifference;
        Vector2 directionA;
        Vector2 directionB;

        public Vector2 DirectionA { get { return directionA; } }
        public Vector2 DirectionB { get { return directionB; } }

        public Simplex(MinkowskiDifference minkowskiDifference, Vector2 directionA, Vector2 directionB)
        {
            this.minkowskiDifference = minkowskiDifference;
            this.directionA = directionA;
            this.directionB = directionB;
        }

        public Simplex WithDirections(Vector2 a, Vector2 b)
        {
            return new Simplex(minkowskiDifference, a, b);
        }

        public IEnumerable<Position2D> Vertices
        {
            get
            {
                yield return (Position2D)Support(directionA);
                yield return (Position2D)Support(directionB);
                yield return (Position2D)Support(-(directionB - directionA).Perpendicular());
            }
        }

        public AABB AABB(Transform2D transform)
        {
            return Bonk.AABB.FromTransformedVertices(Vertices, transform);
        }

        public Vector2 Support(Vector2 direction)
        {
            return minkowskiDifference.Support(direction);
        }

        public Vector2 Support(Vector2 direction, Transform2D transform)
        {
            return Vector2.Transform(Support(direction), transform.TransformMatrix);
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
            if (other is Simplex otherSimplex)
            {
                return minkowskiDifference == otherSimplex.minkowskiDifference &&
                    ((directionA == otherSimplex.directionA && directionB == otherSimplex.directionB) ||
                    (directionA == otherSimplex.directionB && directionB == otherSimplex.directionA));
            }

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 74270316;
            hashCode = hashCode * -1521134295 + EqualityComparer<MinkowskiDifference>.Default.GetHashCode(minkowskiDifference);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector2>.Default.GetHashCode(directionA);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector2>.Default.GetHashCode(directionB);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector2>.Default.GetHashCode(DirectionA);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector2>.Default.GetHashCode(DirectionB);
            hashCode = hashCode * -1521134295 + EqualityComparer<IEnumerable<Position2D>>.Default.GetHashCode(Vertices);
            return hashCode;
        }

        public static bool operator ==(Simplex a, Simplex b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Simplex a, Simplex b)
        {
            return !(a == b);
        }
    }
}