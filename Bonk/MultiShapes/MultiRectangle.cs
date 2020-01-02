using System.Collections.Generic;
using System.Collections.Immutable;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    public struct MultiRectangle : IMultiShape2D
    {
        private ImmutableArray<(Rectangle, Transform2D)> Rectangles { get; }

        public IEnumerable<(IShape2D, Transform2D)> ShapeTransformPairs
        {
            get
            {
                foreach (var rectangle in Rectangles) { yield return rectangle; }
            }
        }

        public AABB AABB { get; }

        public MultiRectangle(ImmutableArray<(Rectangle, Transform2D)> rectangles)
        {
            Rectangles = rectangles;

            AABB = AABBFromRectangles(rectangles);
        }

        public AABB TransformedAABB(Transform2D transform)
        {
            return AABB.Transformed(AABB, transform);
        }

        private static AABB AABBFromRectangles(IEnumerable<(Rectangle, Transform2D)> rectangles)
        {
            var minX = float.MaxValue;
            var minY = float.MaxValue;
            var maxX = float.MinValue;
            var maxY = float.MinValue;

            foreach (var (rectangle, transform) in rectangles)
            {
                var transformedAABB = rectangle.TransformedAABB(transform);

                if (transformedAABB.Min.X < minX)
                {
                    minX = transformedAABB.Min.X;
                }
                if (transformedAABB.Min.Y < minY)
                {
                    minY = transformedAABB.Min.Y;
                }
                if (transformedAABB.Max.X > maxX)
                {
                    maxX = transformedAABB.Max.X;
                }
                if (transformedAABB.Max.Y > maxY)
                {
                    maxY = transformedAABB.Max.Y;
                }
            }

            return new AABB(minX, minY, maxX, maxY);
        }
    }
}
