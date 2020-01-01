using MoonTools.Core.Structs;
using System.Numerics;
using System.Collections.Generic;

namespace MoonTools.Core.Bonk
{
    internal unsafe struct SimplexVertexBuffer
    {
        private const int Size = 35;
        
        public int Length { get; private set; }

        public SimplexVertexBuffer(IEnumerable<Position2D> positions)
        {
            var i = 0;
            foreach (var position in positions)
            {
                if (i == Size) { break; }
                var vertex = position.ToVector2();
                _simplexXBuffer[i] = vertex.X;
                _simplexYBuffer[i] = vertex.Y;
                i++;
            }
            Length = i;
        }

        public Vector2 this[int key]
        {
            get => new Vector2(_simplexXBuffer[key], _simplexYBuffer[key]);
            private set 
            {
                _simplexXBuffer[key] = value.X;
                _simplexYBuffer[key] = value.Y;
            }
        }

        public void Insert(int index, Vector2 value)
        {
            for (var i = Length; i > index; i--)
            {
                this[i] = this[i - 1];
            }
            this[index] = value;
            Length++;
        }

        private fixed float _simplexXBuffer[Size];
        private fixed float _simplexYBuffer[Size];
    }

    public static class NarrowPhase
    {
        private enum PolygonWinding
        {
            Clockwise,
            CounterClockwise
        }

        /// <summary>
        /// Tests if the two shape-transform pairs are overlapping.
        /// </summary>
        public static bool TestCollision(IShape2D shapeA, Transform2D transformA, IShape2D shapeB, Transform2D transformB)
        {
            if (shapeA is Rectangle rectangleA && shapeB is Rectangle rectangleB && transformA.Rotation == 0 && transformB.Rotation == 0)
            {
                return TestRectangleOverlap(rectangleA, transformA, rectangleB, transformB);
            }
            else if (shapeA is Point && shapeB is Rectangle && transformB.Rotation == 0)
            {
                return TestPointRectangleOverlap((Point)shapeA, transformA, (Rectangle)shapeB, transformB);
            }
            else if (shapeA is Rectangle && shapeB is Point && transformA.Rotation == 0)
            {
                return TestPointRectangleOverlap((Point)shapeB, transformB, (Rectangle)shapeA, transformA);
            }
            else if (shapeA is Circle circleA && shapeB is Circle circleB && transformA.Scale.X == transformA.Scale.Y && transformB.Scale.X == transformB.Scale.Y)
            {
                return TestCircleOverlap(circleA, transformA, circleB, transformB);
            }
            return FindCollisionSimplex(shapeA, transformA, shapeB, transformB).Item1;
        }

        /// <summary>
        /// Fast path for axis-aligned rectangles. If the transforms have non-zero rotation this will be inaccurate.
        /// </summary>
        /// <param name="rectangleA"></param>
        /// <param name="transformA"></param>
        /// <param name="rectangleB"></param>
        /// <param name="transformB"></param>
        /// <returns></returns>
        public static bool TestRectangleOverlap(Rectangle rectangleA, Transform2D transformA, Rectangle rectangleB, Transform2D transformB)
        {
            var firstAABB = rectangleA.TransformedAABB(transformA);
            var secondAABB = rectangleB.TransformedAABB(transformB);

            return firstAABB.Left <= secondAABB.Right && firstAABB.Right >= secondAABB.Left && firstAABB.Top <= secondAABB.Bottom && firstAABB.Bottom >= secondAABB.Top;
        }

        /// <summary>
        /// Fast path for overlapping point and axis-aligned rectangle. The rectangle transform must have non-zero rotation.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="pointTransform"></param>
        /// <param name="rectangle"></param>
        /// <param name="rectangleTransform"></param>
        /// <returns></returns>
        public static bool TestPointRectangleOverlap(Point point, Transform2D pointTransform, Rectangle rectangle, Transform2D rectangleTransform)
        {
            var transformedPoint = pointTransform.Position;
            var AABB = rectangle.TransformedAABB(rectangleTransform);

            return transformedPoint.X >= AABB.Left && transformedPoint.X <= AABB.Right && transformedPoint.Y <= AABB.Bottom && transformedPoint.Y >= AABB.Top;
        }

        /// <summary>
        /// Fast path for overlapping circles. The circles must have uniform scaling.
        /// </summary>
        /// <param name="circleA"></param>
        /// <param name="transformA"></param>
        /// <param name="circleB"></param>
        /// <param name="transformB"></param>
        /// <returns></returns>
        public static bool TestCircleOverlap(Circle circleA, Transform2D transformA, Circle circleB, Transform2D transformB)
        {
            var radiusA = circleA.Radius * transformA.Scale.X;
            var radiusB = circleB.Radius * transformB.Scale.Y;

            var centerA = transformA.Position;
            var centerB = transformB.Position;

            var distanceSquared = (centerA - centerB).LengthSquared();
            var radiusSumSquared = (radiusA + radiusB) * (radiusA + radiusB);

            return distanceSquared <= radiusSumSquared;
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

        /// <summary>
        /// Returns a minimum separating vector in the direction from A to B.
        /// </summary>
        /// <param name="simplex">A simplex returned by the GJK algorithm.</param>
        public unsafe static Vector2 Intersect(IShape2D shapeA, Transform2D Transform2DA, IShape2D shapeB, Transform2D Transform2DB, Simplex2D simplex)
        {
            if (shapeA == null) { throw new System.ArgumentNullException(nameof(shapeA)); }
            if (shapeB == null) { throw new System.ArgumentNullException(nameof(shapeB)); }
            if (!simplex.TwoSimplex) { throw new System.ArgumentException("Simplex must be a 2-Simplex.", nameof(simplex)); }

            var a = simplex.A;
            var b = simplex.B.Value;
            var c = simplex.C.Value;

            var e0 = (b.X - a.X) * (b.Y + a.Y);
            var e1 = (c.X - b.X) * (c.Y + b.Y);
            var e2 = (a.X - c.X) * (a.Y + c.Y);
            var winding = e0 + e1 + e2 >= 0 ? PolygonWinding.Clockwise : PolygonWinding.CounterClockwise;

            var simplexVertices = new SimplexVertexBuffer(simplex.Vertices);

            Vector2 intersection = default;

            for (var i = 0; i < 32; i++)
            {
                var edge = FindClosestEdge(winding, simplexVertices);
                var support = CalculateSupport(shapeA, Transform2DA, shapeB, Transform2DB, edge.normal);
                var distance = Vector2.Dot(support, edge.normal);

                intersection = edge.normal;
                intersection *= distance;

                if (System.Math.Abs(distance - edge.distance) <= float.Epsilon)
                {
                    return intersection;
                }
                else
                {
                    simplexVertices.Insert(edge.index, support);
                }
            }

            return intersection;
        }

        private static Edge FindClosestEdge(PolygonWinding winding, SimplexVertexBuffer simplexVertices)
        {
            var closestDistance = float.PositiveInfinity;
            var closestNormal = Vector2.Zero;
            var closestIndex = 0;

            for (var i = 0; i < simplexVertices.Length; i++)
            {
                var j = i + 1;
                if (j >= simplexVertices.Length) { j = 0; }
                var edge = simplexVertices[j] - simplexVertices[i];

                Vector2 norm;
                if (winding == PolygonWinding.Clockwise)
                {
                    norm = Vector2.Normalize(new Vector2(edge.Y, -edge.X));
                }
                else
                {
                    norm = Vector2.Normalize(new Vector2(-edge.Y, edge.X));
                }

                var dist = Vector2.Dot(norm, simplexVertices[i]);

                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closestNormal = norm;
                    closestIndex = j;
                }
            }

            return new Edge(closestDistance, closestNormal, closestIndex);
        }

        private static Vector2 CalculateSupport(IShape2D shapeA, Transform2D Transform2DA, IShape2D shapeB, Transform2D Transform2DB, Vector2 direction)
        {
            return shapeA.Support(direction, Transform2DA) - shapeB.Support(-direction, Transform2DB);
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
