using System.Collections.Generic;
using System.Collections.Immutable;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    public struct MultiShape<TShape2D> : IHasAABB2D, ICollisionTestable<TShape2D> where TShape2D : struct, IShape2D
    {
        private static ImmutableArray<TransformedShape2D<TShape2D>>.Builder _builder = ImmutableArray.CreateBuilder<TransformedShape2D<TShape2D>>();
        private ImmutableArray<TransformedShape2D<TShape2D>> _transformedShapes;
        public IEnumerable<TransformedShape2D<TShape2D>> TransformedShapes { get { return _transformedShapes; } }

        public AABB AABB { get; }

        public MultiShape(ImmutableArray<(TShape2D, Transform2D)> shapeTransformPairs)
        {
            _builder.Clear();
            foreach (var (shape, transform) in shapeTransformPairs)
            {
                _builder.Add(new TransformedShape2D<TShape2D>(shape, transform));
            }

            _transformedShapes = _builder.ToImmutable();
            AABB = AABBFromShapes(_transformedShapes);
        }

        public MultiShape(ImmutableArray<TransformedShape2D<TShape2D>> transformedShapes)
        {
            _transformedShapes = transformedShapes;
            AABB = AABBFromShapes(transformedShapes);
        }

        public AABB TransformedAABB(Transform2D transform)
        {
            return AABB.Transformed(AABB, transform);
        }

        public IEnumerable<TransformedShape2D<TShape2D>> Compose(Transform2D transform)
        {
            foreach (var transformedShape in TransformedShapes)
            {
                yield return transformedShape.Compose(transform);
            }
        }

        private static AABB AABBFromShapes(IEnumerable<TransformedShape2D<TShape2D>> transformedShapes)
        {
            var minX = float.MaxValue;
            var minY = float.MaxValue;
            var maxX = float.MinValue;
            var maxY = float.MinValue;

            foreach (var transformedShape in transformedShapes)
            {
                var aabb = transformedShape.AABB;

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
