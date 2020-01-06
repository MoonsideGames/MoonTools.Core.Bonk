using System.Collections.Generic;
using System.Collections.Immutable;
using System.Numerics;
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

        /// <summary>
        /// Moves the shapes by pivoting with an offset transform. 
        /// </summary>
        /// <param name="offsetTransform"></param>
        /// <returns></returns>
        public IEnumerable<(IShape2D, Transform2D)> TransformShapesUsingOffset(Transform2D offsetTransform)
        {
            foreach (var (shape, shapeTransform) in ShapeTransformPairs)
            {
                var newTransform = new Transform2D(Vector2.Transform(shapeTransform.Position, offsetTransform.TransformMatrix), offsetTransform.Rotation, offsetTransform.Scale);
                yield return (shape, newTransform);
            }
        }

        public bool IsSingleShape<T>() where T : struct, IShape2D
        {
            return ShapeTransformPairs.Length == 1 && ShapeTransformPairs[0].Item1 is T;
        }

        public (T, Transform2D) ShapeTransformPair<T>() where T : struct, IShape2D
        {
            return ((T, Transform2D))ShapeTransformPairs[0];
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
