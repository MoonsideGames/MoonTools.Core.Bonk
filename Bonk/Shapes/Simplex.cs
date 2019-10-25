using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MoonTools.Core.Structs;
using MoonTools.Core.Bonk.Extensions;

namespace MoonTools.Core.Bonk
{
    public struct Simplex : IShape2D
    {
        MinkowskiDifference minkowskiDifference;
        Vector2 directionA;
        Vector2 directionB;

        public Simplex(MinkowskiDifference minkowskiDifference, Vector2 directionA, Vector2 directionB)
        {
            this.minkowskiDifference = minkowskiDifference;
            this.directionA = directionA;
            this.directionB = directionB;
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

        public bool Equals(IShape2D other)
        {
            if (other is Simplex polytope)
            {
                return minkowskiDifference.Equals(polytope.minkowskiDifference) &&
                    directionA == polytope.directionA &&
                    directionB == polytope.directionB;
            }

            return false;
        }

        public Vector2 Support(Vector2 direction)
        {
            return minkowskiDifference.Support(direction);
        }

        public Vector2 Support(Vector2 direction, Transform2D transform)
        {
            return Vector2.Transform(Support(direction), transform.TransformMatrix);
        }
    }
}