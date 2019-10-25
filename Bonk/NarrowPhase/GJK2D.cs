using Microsoft.Xna.Framework;
using MoonTools.Core.Structs;
using MoonTools.Core.Bonk.Extensions;

namespace MoonTools.Core.Bonk
{
    public static class GJK2D
    {
        /// <summary>
        /// Tests if the two shape-transform pairs are overlapping.
        /// </summary>
        public static bool TestCollision(IShape2D shapeA, Transform2D transformA, IShape2D shapeB, Transform2D transformB)
        {
            var minkowskiDifference = new MinkowskiDifference(shapeA, transformA, shapeB, transformB);
            var a = minkowskiDifference.Support(Vector2.UnitX);
            var b = minkowskiDifference.Support(-a);

            return Vector2.Dot(a, b) > 0 ? false : CheckSimplex(new Simplex(minkowskiDifference, a, b));
        }

        private static bool CheckSimplex(Simplex simplex)
        {
            var a = simplex.DirectionA;
            var b = simplex.DirectionB;

            var axb = a.Cross(b);
            var c = simplex.Support((b - a).Perpendicular());
            var axc = a.Cross(c);
            var bxc = b.Cross(c);
            var cxb = -bxc;

            return (b - a) == Vector2.Zero || (axb.Y > 0 != bxc.Y > 0 ? CheckSimplex(simplex.WithDirections(b, c)) : (axc.Y > 0 != cxb.Y > 0 ? CheckSimplex(simplex.WithDirections(a, c)) : true));
        }

        /// <summary>
        /// Tests if the two shape-transform pairs are overlapping, and returns a simplex that can be used by the EPA algorithm to determine a miminum separating vector.
        /// </summary>
        public static (bool, Simplex) FindCollisionSimplex(IShape2D shapeA, Transform2D transformA, IShape2D shapeB, Transform2D transformB)
        {
            var minkowskiDifference = new MinkowskiDifference(shapeA, transformA, shapeB, transformB);
            var a = minkowskiDifference.Support(Vector2.UnitX);
            var b = minkowskiDifference.Support(-a);

            return Vector2.Dot(a, b) > 0 ? (false, default(Simplex)) : Simplex(new Simplex(minkowskiDifference, a, b));
        }

        private static (bool, Simplex) Simplex(Simplex simplex)
        {
            var a = simplex.DirectionA;
            var b = simplex.DirectionB;

            if ((b - a) == Vector2.Zero)
            {
                return (false, simplex.WithDirections(a, b));
            }
            else
            {
                var c = simplex.Support((b - a).Perpendicular());
                var axb = a.Cross(b);
                var bxc = b.Cross(c);

                if (axb.Y > 0 != bxc.Y > 0)
                {
                    return Simplex(simplex.WithDirections(b, c));
                }
                else
                {
                    var axc = a.Cross(c);
                    var cxb = -bxc;

                    if (axc.Y > 0 != cxb.Y > 0)
                    {
                        return Simplex(simplex.WithDirections(a, b));
                    }
                    else
                    {
                        return (true, simplex.WithDirections(a, b));
                    }
                }
            }
        }
    }
}
