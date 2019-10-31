using MoonTools.Core.Structs;
using MoonTools.Core.Bonk.Extensions;
using System.Numerics;

namespace MoonTools.Core.Bonk
{
    public static class GJK2D
    {
        /// <summary>
        /// Tests if the two shape-transform pairs are overlapping.
        /// </summary>
        public static bool TestCollision(IShape2D shapeA, Transform2D transformA, IShape2D shapeB, Transform2D transformB)
        {
            return FindCollisionSimplex(shapeA, transformA, shapeB, transformB).Item1;
        }

        /// <summary>
        /// Tests if the two shape-transform pairs are overlapping, and returns a simplex that can be used by the EPA algorithm to determine a miminum separating vector.
        /// </summary>
        public static (bool, Simplex2D) FindCollisionSimplex(IShape2D shapeA, Transform2D transformA, IShape2D shapeB, Transform2D transformB)
        {
            var minkowskiDifference = new MinkowskiDifference(shapeA, transformA, shapeB, transformB);
            var c = minkowskiDifference.Support(Vector2.UnitX);
            var b = minkowskiDifference.Support(-Vector2.UnitX);
            return Check(minkowskiDifference, c, b);
        }

        private static (bool, Simplex2D) Check(MinkowskiDifference minkowskiDifference, Vector2 c, Vector2 b)
        {
            var cb = c - b;
            var c0 = -c;
            var d = Direction(cb, c0);
            return DoSimplex(minkowskiDifference, new Simplex2D(b, c), d);
        }

        private static (bool, Simplex2D) DoSimplex(MinkowskiDifference minkowskiDifference, Simplex2D simplex, Vector2 direction)
        {
            var a = minkowskiDifference.Support(direction);
            var notPastOrigin = Vector2.Dot(a, direction) < 0;
            var (intersects, newSimplex, newDirection) = EnclosesOrigin(a, simplex);

            if (notPastOrigin)
            {
                return (false, default(Simplex2D));
            }
            else if (intersects)
            {
                return (true, new Simplex2D(simplex.A, simplex.B.Value, a));
            }
            else
            {
                return DoSimplex(minkowskiDifference, newSimplex, newDirection);
            }
        }

        private static (bool, Simplex2D, Vector2) EnclosesOrigin(Vector2 a, Simplex2D simplex)
        {
            if (simplex.ZeroSimplex)
            {
                return HandleZeroSimplex(a, simplex.A);
            }
            else if (simplex.OneSimplex)
            {
                return HandleOneSimplex(a, simplex.A, simplex.B.Value);
            }
            else
            {
                return (false, simplex, Vector2.Zero);
            }
        }

        private static (bool, Simplex2D, Vector2) HandleZeroSimplex(Vector2 a, Vector2 b)
        {
            var ab = b - a;
            var a0 = -a;
            var (newSimplex, newDirection) = SameDirection(ab, a0) ? (new Simplex2D(a, b), Perpendicular(ab, a0)) : (new Simplex2D(a), a0);
            return (false, newSimplex, newDirection);
        }

        private static (bool, Simplex2D, Vector2) HandleOneSimplex(Vector2 a, Vector2 b, Vector2 c)
        {
            var a0 = -a;
            var ab = b - a;
            var ac = c - a;
            var abp = Perpendicular(ab, -ac);
            var acp = Perpendicular(ac, -ab);

            if (SameDirection(abp, a0))
            {
                if (SameDirection(ab, a0))
                {
                    return (false, new Simplex2D(a, b), abp);
                }
                else
                {
                    return (false, new Simplex2D(a), a0);
                }
            }
            else if (SameDirection(acp, a0))
            {
                if (SameDirection(ac, a0))
                {
                    return (false, new Simplex2D(a, c), acp);
                }
                else
                {
                    return (false, new Simplex2D(a), a0);
                }
            }
            else
            {
                return (true, new Simplex2D(b, c), a0);
            }
        }

        private static Vector2 TripleProduct(Vector2 a, Vector2 b, Vector2 c)
        {
            var A = new Vector3(a.X, a.Y, 0);
            var B = new Vector3(b.X, b.Y, 0);
            var C = new Vector3(c.X, c.Y, 0);

            var first = Vector3.Cross(A, B);
            var second = Vector3.Cross(first, C);

            return new Vector2(second.X, second.Y);
        }

        private static Vector2 Direction(Vector2 a, Vector2 b)
        {
            var d = TripleProduct(a, b, a);
            var collinear = d == Vector2.Zero;
            return collinear ? new Vector2(a.Y, -a.X) : d;
        }

        private static bool SameDirection(Vector2 a, Vector2 b)
        {
            return Vector2.Dot(a, b) > 0;
        }

        private static Vector2 Perpendicular(Vector2 a, Vector2 b)
        {
            return TripleProduct(a, b, a);
        }
    }
}
