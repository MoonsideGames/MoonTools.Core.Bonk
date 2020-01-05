using System.Collections.Generic;
using System.Collections.Immutable;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    public struct MultiShape : IHasAABB2D
    {
        public ImmutableArray<(IShape2D, Transform2D)> ShapeTransformPairs { get; }

        public AABB AABB { get; }

        public MultiShape(ImmutableArray<(IShape2D, Transform2D)> shapeTransformPairs)
        {
            ShapeTransformPairs = shapeTransformPairs;

            AABB = AABBFromShapes(shapeTransformPairs);
        }

        public AABB TransformedAABB(Transform2D transform)
        {
            return AABB.Transformed(AABB, transform);
        }

        public IEnumerable<(IShape2D, Transform2D)> TransformedShapeTransforms(Transform2D transform)
        {
            foreach (var (shape, shapeTransform) in ShapeTransformPairs)
            {
                yield return (shape, transform.Compose(shapeTransform));
            }
        }

        private static AABB AABBFromShapes(IEnumerable<(IShape2D, Transform2D)> shapeTransforms)
        {
            var minX = float.MaxValue;
            var minY = float.MaxValue;
            var maxX = float.MinValue;
            var maxY = float.MinValue;

            foreach (var (shape, transform) in shapeTransforms)
            {
                var aabb = shape.TransformedAABB(transform);

                if (aabb.Min.X < minX)
                {
                    minX = aabb.Min.X;
                }
                if (aabb.Min.Y < minY)
                {
                    minY = aabb.Min.Y;
                }
                if (aabb.Max.X > maxX)
                {
                    maxX = aabb.Max.X;
                }
                if (aabb.Max.Y > maxY)
                {
                    maxY = aabb.Max.Y;
                }
            }

            return new AABB(minX, minY, maxX, maxY);
        }
    }
}
