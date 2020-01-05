using MoonTools.Core.Structs;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace MoonTools.Core.Bonk
{
    public static class NarrowPhase
    {
        private enum PolygonWinding
        {
            Clockwise,
            CounterClockwise
        }

        public static bool TestCollision<T, U>(ICollisionTestable<T> a, ICollisionTestable<U> b) where T : struct, IShape2D where U : struct, IShape2D
        {
            foreach (var shape in a.TransformedShapes)
            {
                foreach (var shapeB in b.TransformedShapes)
                {
                    return TestCollision(shape, shapeB);
                }
            }
            return false;
        }

        /// <summary>
        /// Tests if two shape-transform pairs are overlapping.
        /// </summary>
        public static bool TestCollision<T, U>(T shape, Transform2D transform, U shapeB, Transform2D transformB) where T : struct, IShape2D where U : struct, IShape2D
        {
            return TestCollision(new TransformedShape2D<T>(shape, transform), new TransformedShape2D<U>(shapeB, transformB));
        }

        /// <summary>
        /// Tests if two TransformedShapes are overlapping.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="transformedShapeA"></param>
        /// <param name="transformedShapeB"></param>
        /// <returns></returns>
        public static bool TestCollision<T, U>(TransformedShape2D<T> transformedShapeA, TransformedShape2D<U> transformedShapeB) where T : struct, IShape2D where U : struct, IShape2D
        {
            return FindCollisionSimplex(transformedShapeA, transformedShapeB).Item1;
        }

        /// <summary>
        /// Tests if a multishape-transform and shape-transform pair are overlapping. 
        /// Note that this must perform pairwise comparison so the worst-case performance of this method will vary inversely with the amount of shapes in the multishape.
        /// </summary>
        /// <param name="multiShape"></param>
        /// <param name="multiShapeTransform"></param>
        /// <param name="shape"></param>
        /// <returns></returns>
        public static bool TestCollision<T, U>(MultiShape<T> multiShape, Transform2D multiShapeTransform, TransformedShape2D<U> shape) where T : struct, IShape2D where U : struct, IShape2D
        {
            foreach (var transformedShape in multiShape.Compose(multiShapeTransform))
            {
                if (TestCollision(shape, transformedShape)) { return true; }
            }
            return false;
        }

        public static bool TestCollison<T>(IEnumerable<TransformedShape2D<T>> transformedShapes, Transform2D multiShapeTransform, TransformedShape2D<U> shape) where T : struct, IShape2D where U : struct, IShape2D
        {
            foreach (var transformedShape in transformedShapes)
            {
                if (TestCollision(transformedShape.Compose(multiShapeTransform), shape)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Tests if a multishape-transform and shape-transform pair are overlapping. 
        /// Note that this must perform pairwise comparison so the worst-case performance of this method will vary inversely with the amount of shapes in the multishape.
        /// </summary>
        /// <param name="multiShape"></param>
        /// <param name="multiShapeTransform"></param>
        /// <param name="shape"></param>
        /// <returns></returns>
        public static bool TestCollision<T, U>(TransformedShape2D<T> shape, MultiShape<U> multiShape, Transform2D multiShapeTransform) where T : struct, IShape2D where U : struct, IShape2D
        {
            return TestCollision(multiShape, multiShapeTransform, shape);
        }

        /// <summary>
        /// Tests if two multishape-transform pairs are overlapping. 
        /// Note that this must perform pairwise comparison so the worst-case performance of this method will vary inversely with the amount of shapes in the multishapes.
        /// </summary>
        /// <param name="multiShapeA"></param>
        /// <param name="transformA"></param>
        /// <param name="multiShapeB"></param>
        /// <param name="transformB"></param>
        /// <returns></returns>
        public static bool TestCollision<T, U>(MultiShape<T> multiShapeA, Transform2D transformA, MultiShape<U> multiShapeB, Transform2D transformB) where T : struct, IShape2D where U : struct, IShape2D
        {
            foreach (var transformedShapeA in multiShapeA.Compose(transformA))
            {
                foreach (var transformedShapeB in multiShapeB.Compose(transformB))
                {
                    if (TestCollision(transformedShapeA, transformedShapeB)) { return true; }
                }
            }
            return false;
        }

        /// <summary>
        /// Fast path for axis-aligned rectangles. If the transforms have non-zero rotation this will be inaccurate.
        /// </summary>
        /// <param name="rectangleA"></param>
        /// <param name="rectangleB"></param>
        /// <returns></returns>
        public static bool TestCollision(TransformedShape2D<Rectangle> rectangleA, TransformedShape2D<Rectangle> rectangleB)
        {
            var firstAABB = rectangleA.AABB;
            var secondAABB = rectangleB.AABB;

            return firstAABB.Left <= secondAABB.Right && firstAABB.Right >= secondAABB.Left && firstAABB.Top <= secondAABB.Bottom && firstAABB.Bottom >= secondAABB.Top;
        }

        /// <summary>
        /// Fast path for overlapping point and axis-aligned rectangle. The rectangle transform must have non-zero rotation.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static bool TestCollision(TransformedShape2D<Point> point, TransformedShape2D<Rectangle> rectangle)
        {
            var transformedPoint = point.Transform.Position;
            var AABB = rectangle.AABB;

            return transformedPoint.X >= AABB.Left && transformedPoint.X <= AABB.Right && transformedPoint.Y <= AABB.Bottom && transformedPoint.Y >= AABB.Top;
        }

        public static bool TestCollision(TransformedShape2D<Rectangle> rectangle, TransformedShape2D<Point> point)
        {
            return TestCollision(point, rectangle);
        }

        /// <summary>
        /// Fast path for overlapping circles. The circles must have uniform scaling.
        /// </summary>
        /// <param name="circleA"></param>
        /// <param name="circleB"></param>
        /// <returns></returns>
        public static bool TestCollision(TransformedShape2D<Circle> circleA, TransformedShape2D<Circle> circleB)
        {
            var radiusA = circleA.Shape.Radius * circleA.Transform.Scale.X;
            var radiusB = circleB.Shape.Radius * circleB.Transform.Scale.Y;

            var centerA = circleA.Transform.Position;
            var centerB = circleB.Transform.Position;

            var distanceSquared = (centerA - centerB).LengthSquared();
            var radiusSumSquared = (radiusA + radiusB) * (radiusA + radiusB);

            return distanceSquared <= radiusSumSquared;
        }

        /// <summary>
        /// Tests if the two shape-transform pairs are overlapping, and returns a simplex that can be used by the EPA algorithm to determine a miminum separating vector.
        /// </summary>
        public static (bool, Simplex2D) FindCollisionSimplex<T, U>(TransformedShape2D<T> shapeA, TransformedShape2D<U> shapeB) where T : struct, IShape2D where U : struct, IShape2D
        {
            var minkowskiDifference = new MinkowskiDifference<T, U>(shapeA, shapeB);
            var c = minkowskiDifference.Support(Vector2.UnitX);
            var b = minkowskiDifference.Support(-Vector2.UnitX);
            return Check(minkowskiDifference, c, b);
        }

        /// <summary>
        /// Returns a minimum separating vector in the direction from A to B.
        /// </summary>
        /// <param name="simplex">A simplex returned by the GJK algorithm.</param>
        public unsafe static Vector2 Intersect<T, U>(TransformedShape2D<T> shapeA, TransformedShape2D<U> shapeB, Simplex2D simplex) where T : struct, IShape2D where U : struct, IShape2D
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
                var support = CalculateSupport(shapeA, shapeB, edge.normal);
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

        private static Vector2 CalculateSupport<T, U>(TransformedShape2D<T> shapeA, TransformedShape2D<U> shapeB, Vector2 direction) where T : struct, IShape2D where U : struct, IShape2D
        {
            return shapeA.Support(direction) - shapeB.Support(-direction);
        }

        private static (bool, Simplex2D) Check<T, U>(MinkowskiDifference<T, U> minkowskiDifference, Vector2 c, Vector2 b) where T : struct, IShape2D where U : struct, IShape2D
        {
            var cb = c - b;
            var c0 = -c;
            var d = Direction(cb, c0);
            return DoSimplex(minkowskiDifference, new Simplex2D(b, c), d);
        }

        private static (bool, Simplex2D) DoSimplex<T, U>(MinkowskiDifference<T, U> minkowskiDifference, Simplex2D simplex, Vector2 direction) where T : struct, IShape2D where U : struct, IShape2D
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
