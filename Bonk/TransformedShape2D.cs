using System;
using System.Collections.Generic;
using System.Numerics;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    public struct TransformedShape2D<TShape2D> : IEquatable<TransformedShape2D<TShape2D>>, ICollisionTestable<TShape2D>, IHasAABB2D where TShape2D : struct, IShape2D
    {
        public TShape2D Shape { get; }
        public Transform2D Transform { get; }
        public AABB AABB { get; }
        public IEnumerable<TransformedShape2D<TShape2D>> TransformedShapes { get { yield return this; } }

        public TransformedShape2D(TShape2D shape, Transform2D transform)
        {
            Shape = shape;
            Transform = transform;
            AABB = shape.TransformedAABB(transform);
        }

        public TransformedShape2D<TShape2D> Compose(Transform2D transform)
        {
            return new TransformedShape2D<TShape2D>(Shape, Transform.Compose(transform));
        }

        public Vector2 Support(Vector2 direction)
        {
            return Shape.Support(direction, Transform);
        }

        public void Deconstruct(out TShape2D shape, out Transform2D transform)
        {
            shape = Shape;
            transform = Transform;
        }

        public bool TestCollision<U>(ICollisionTestable collisionTestable) where U : struct, IShape2D
        {
            return NarrowPhase.TestCollision(TransformedShapes, collisionTestable.TransformedShapes);
            if (collisionTestable is MultiShape<U> multiShape)
            {
                return NarrowPhase.TestCollision(this, multiShape);
            }
            else if (collisionTestable is TransformedShape2D<U> shape)
            {
                return NarrowPhase.TestCollision(this, shape);
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            return obj is TransformedShape2D<TShape2D> d && Equals(d);
        }

        public bool Equals(TransformedShape2D<TShape2D> other)
        {
            return Shape.Equals(other.Shape) &&
                   Transform.Equals(other.Transform);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Shape, Transform);
        }

        public static bool operator ==(TransformedShape2D<TShape2D> left, TransformedShape2D<TShape2D> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TransformedShape2D<TShape2D> left, TransformedShape2D<TShape2D> right)
        {
            return !(left == right);
        }
    }
}
