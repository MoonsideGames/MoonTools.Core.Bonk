using System.Collections.Generic;

namespace MoonTools.Core.Bonk
{
    public interface ICollisionTestable
    {
        bool TestCollision<T>(ICollisionTestable collisionTestable) where T : struct, IShape2D;
        IEnumerable<TransformedShape2D<T>> TransformedShapes<T>() where T : struct, IShape2D;
    }

    public interface ICollisionTestable<T> : ICollisionTestable where T : struct, IShape2D
    {
        IEnumerable<TransformedShape2D<T>> TransformedShapes { get; }
    }
}
