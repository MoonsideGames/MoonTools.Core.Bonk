using System;
using System.Numerics;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    public static class SweepTest
    {
        /// <summary>
        /// Performs a sweep test on and against rectangles. Returns the position 1 pixel before overlap occurs.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spatialHash">A spatial hash.</param>
        /// <param name="rectangle"></param>
        /// <param name="transform">A transform by which to transform the IHasAABB2D.</param>
        /// <param name="ray">Given in world-space.</param>
        /// <returns></returns>
        public static SweepResult<T> Test<T>(SpatialHash<T> spatialHash, Rectangle rectangle, Transform2D transform, Vector2 ray) where T : IEquatable<T>
        {
            var transformedAABB = rectangle.TransformedAABB(transform);
            var sweepBox = SweepBox(transformedAABB, ray);

            var shortestDistance = float.MaxValue;
            var nearestID = default(T);
            Rectangle? nearestRectangle = null;
            Transform2D? nearestTransform = null;

            foreach (var (id, shape, shapeTransform) in spatialHash.Retrieve(sweepBox))
            {
                Rectangle otherRectangle;
                Transform2D otherTransform;
                AABB otherTransformedAABB;
                if (shape is Rectangle)
                {
                    otherRectangle = (Rectangle)shape;
                    otherTransformedAABB = shape.TransformedAABB(shapeTransform);
                    otherTransform = shapeTransform;
                }
                else if (shape is MultiShape multiShape && multiShape.IsSingleShape<Rectangle>())
                {
                    Transform2D rectangleOffset;
                    (otherRectangle, rectangleOffset) = multiShape.ShapeTransformPair<Rectangle>();
                    otherTransform = shapeTransform.Compose(rectangleOffset);
                    otherTransformedAABB = shape.TransformedAABB(otherTransform);
                }
                else
                {
                    continue;
                }

                float xInvEntry, yInvEntry;

                if (ray.X > 0)
                {
                    xInvEntry = otherTransformedAABB.Left - (transformedAABB.Right);
                }
                else
                {
                    xInvEntry = (otherTransformedAABB.Right) - transformedAABB.Left;
                }

                if (ray.Y > 0)
                {
                    yInvEntry = otherTransformedAABB.Top - (transformedAABB.Bottom);
                }
                else
                {
                    yInvEntry = (otherTransformedAABB.Bottom) - transformedAABB.Top;
                }

                float xEntry, yEntry;

                if (ray.X == 0)
                {
                    xEntry = float.MinValue;
                }
                else
                {
                    xEntry = xInvEntry / ray.X;
                }

                if (ray.Y == 0)
                {
                    yEntry = float.MinValue;
                }
                else
                {
                    yEntry = yInvEntry / ray.Y;
                }

                var entryTime = Math.Max(xEntry, yEntry);

                if (entryTime >= 0 && entryTime <= 1)
                {
                    if (entryTime < shortestDistance)
                    {
                        shortestDistance = entryTime;
                        nearestID = id;
                        nearestRectangle = otherRectangle;
                        nearestTransform = shapeTransform;
                    }
                }
                
            }

            if (nearestRectangle.HasValue)
            {
                var overlapPosition = ray * shortestDistance;
                var correctionX = -Math.Sign(ray.X);
                var correctionY = -Math.Sign(ray.Y);
                return new SweepResult<T>(true, new Position2D((int)overlapPosition.X + correctionX, (int)overlapPosition.Y + correctionY), nearestID);
            }
            else
            {
                return SweepResult<T>.False;
            }
        }

        public static SweepResult<T> Test<T>(SpatialHash<T> spatialHash, Point point, Transform2D transform, Vector2 ray) where T : IEquatable<T>
        {
            return Test(spatialHash, new Rectangle(0, 0), transform, ray);
        }

        private static AABB SweepBox(AABB aabb, Vector2 ray)
        {
            return new AABB(
                Math.Min(aabb.Min.X, aabb.Min.X + ray.X),
                Math.Min(aabb.Min.Y, aabb.Min.Y + ray.Y),
                Math.Max(aabb.Max.X, aabb.Max.X + ray.X),
                Math.Max(aabb.Max.Y, aabb.Max.Y + ray.Y)
            );
        }
    }
}
