using Microsoft.Xna.Framework;
using MoonTools.Core.Structs;
using System;
using MoonTools.Core.Bonk.Extensions;

namespace MoonTools.Core.Bonk
{


    public static class GJK2D
    {
        public static bool TestCollision(IShape2D shapeA, Transform2D transformA, IShape2D shapeB, Transform2D transformB)
        {
            var minkowskiDifference = new MinkowskiDifference(shapeA, transformA, shapeB, transformB);
            return OriginInside(minkowskiDifference);
        }

        public static (bool, Simplex) CollisionAndSimplex(IShape2D shapeA, Transform2D transformA, IShape2D shapeB, Transform2D transformB)
        {
            var minkowskiDifference = new MinkowskiDifference(shapeA, transformA, shapeB, transformB);
            var (collision, a, b) = OriginInsideWithSimplex(minkowskiDifference);
            var polytope = new Simplex(minkowskiDifference, a, b);
            return (collision, polytope);
        }

        private static Vector2 MinkowskiDifference(Vector2 direction, IShape2D shapeA, Transform2D transformA, IShape2D shapeB, Transform2D transformB)
        {
            return shapeA.Support(direction, transformA) - shapeB.Support(-direction, transformB);
        }

        private static bool OriginInside(MinkowskiDifference minkowskiDifference)
        {
            var a = minkowskiDifference.Support(Vector2.UnitX);
            var b = minkowskiDifference.Support(-a);

            return Vector2.Dot(a, b) > 0 ? false : CheckSimplex(minkowskiDifference.Support, a, b);
        }

        private static (bool, Vector2, Vector2) OriginInsideWithSimplex(MinkowskiDifference minkowskiDifference)
        {
            var a = minkowskiDifference.Support(Vector2.UnitX);
            var b = minkowskiDifference.Support(-a);

            return Vector2.Dot(a, b) > 0 ? (false, a, b) : Simplex(minkowskiDifference.Support, a, b);
        }

        private static bool CheckSimplex(Func<Vector2, Vector2> support, Vector2 a, Vector2 b)
        {
            var axb = a.Cross(b);
            var c = support((b - a).Perpendicular());
            var axc = a.Cross(c);
            var bxc = b.Cross(c);
            var cxb = -bxc;

            return (b - a) == Vector2.Zero || (axb.Y > 0 != bxc.Y > 0 ? CheckSimplex(support, b, c) : (axc.Y > 0 != cxb.Y > 0 ? CheckSimplex(support, a, c) : true));
        }

        private static (bool, Vector2, Vector2) Simplex(Func<Vector2, Vector2> support, Vector2 a, Vector2 b)
        {
            if ((b - a) == Vector2.Zero)
            {
                return (false, a, b);
            }
            else
            {
                var c = support((b - a).Perpendicular());
                var axb = a.Cross(b);
                var bxc = b.Cross(c);

                if (axb.Y > 0 != bxc.Y > 0)
                {
                    return Simplex(support, b, c);
                }
                else
                {
                    var axc = a.Cross(c);
                    var cxb = -bxc;

                    if (axc.Y > 0 != cxb.Y > 0)
                    {
                        return Simplex(support, a, b);
                    }
                    else
                    {
                        return (true, a, b);
                    }
                }
            }
        }
    }
}
