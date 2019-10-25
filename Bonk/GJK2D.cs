using Microsoft.Xna.Framework;
using MoonTools.Core.Structs;
using System;
using MoonTools.Core.Bonk.Extensions;

namespace MoonTools.Core.Bonk
{
    // TODO: get rid of minkowski closure for GC purposes
    public static class GJK2D
    {
        public static bool TestCollision(IShape2D shapeA, Transform2D transformA, IShape2D shapeB, Transform2D transformB)
        {
            return OriginInside(MinkowskiDifference(shapeA, transformA, shapeB, transformB));
        }

        public static (bool, (Func<Vector2, Vector2>, Vector2, Vector2)) CollisionAndSimplex(IShape2D shapeA, Transform2D transformA, IShape2D shapeB, Transform2D transformB)
        {
            var support = MinkowskiDifference(shapeA, transformA, shapeB, transformB);
            var result = OriginInsideWithSimplex(support);
            return (result.Item1, (support, result.Item2, result.Item3));
        }

        private static Func<Vector2, Vector2> MinkowskiDifference(IShape2D shapeA, Transform2D transformA, IShape2D shapeB, Transform2D transformB)
        {
            return direction => shapeA.Support(direction, transformA) - shapeB.Support(-direction, transformB);
        }

        private static bool OriginInside(Func<Vector2, Vector2> support)
        {
            var a = support(Vector2.UnitX);
            var b = support(-a);

            return Vector2.Dot(a, b) > 0 ? false : CheckSimplex(support, a, b);
        }

        private static (bool, Vector2, Vector2) OriginInsideWithSimplex(Func<Vector2, Vector2> support)
        {
            var a = support(Vector2.UnitX);
            var b = support(-a);

            return Vector2.Dot(a, b) > 0 ? (false, a, b) : Simplex(support, a, b);
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
