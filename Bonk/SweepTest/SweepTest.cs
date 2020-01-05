using System;
using System.Numerics;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    public static class SweepTest
    {
        /// <summary>
        /// Performs a sweep test on rectangles.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spatialHash">A spatial hash.</param>
        /// <param name="rectangle"></param>
        /// <param name="transform">A transform by which to transform the IHasAABB2D.</param>
        /// <param name="ray">Given in world-space.</param>
        /// <returns></returns>
        public static SweepResult<T, Rectangle> Rectangle<T>(SpatialHash<T> spatialHash, Rectangle rectangle, Transform2D transform, Vector2 ray) where T : IEquatable<T>
        {
            var transformedAABB = rectangle.TransformedAABB(transform);
            var sweepBox = SweepBox(transformedAABB, ray);

            var shortestDistance = float.MaxValue;
            var nearestID = default(T);
            Rectangle? nearestRectangle = null;
            Transform2D? nearestTransform = null;

            foreach (var (id, shape, shapeTransform) in spatialHash.Retrieve(sweepBox))
            {
                if (shape is Rectangle otherRectangle)
                {
                    var otherTransformedAABB = otherRectangle.TransformedAABB(shapeTransform);
                    float xInvEntry, yInvEntry;

                    if (ray.X > 0)
                    {
                        xInvEntry = shapeTransform.Position.X - (transform.Position.X + transformedAABB.Width);
                    }
                    else
                    {
                        xInvEntry = (shapeTransform.Position.X + otherTransformedAABB.Width) - transform.Position.X;
                    }

                    if (ray.Y > 0)
                    {
                        yInvEntry = shapeTransform.Position.Y - (transform.Position.Y + transformedAABB.Height);
                    }
                    else
                    {
                        yInvEntry = (shapeTransform.Position.Y + otherTransformedAABB.Height) - shapeTransform.Position.Y;
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

                    if (entryTime > 0 && entryTime < 1)
                    {
                        if (entryTime < shortestDistance)
                        {
                            shortestDistance = entryTime;
                            nearestID = id;
                            nearestRectangle = rectangle;
                            nearestTransform = shapeTransform;
                        }
                    }
                }
            }

            if (nearestRectangle.HasValue)
            {
                return new SweepResult<T, Rectangle>(true, ray * shortestDistance, nearestID, nearestRectangle.Value, nearestTransform.Value);
            }
            else
            {
                return SweepResult<T, Rectangle>.False;
            }
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
