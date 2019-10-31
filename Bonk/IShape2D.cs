using System;
using System.Numerics;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    public interface IShape2D : IEquatable<IShape2D>
    {
        /// <summary>
        /// A Minkowski support function. Gives the farthest point on the edge of a shape along the given direction.
        /// </summary>
        /// <param name="direction">A normalized Vector2.</param>
        /// <param name="transform">A Transform for transforming the shape vertices.</param>
        /// <returns>The farthest point on the edge of the shape along the given direction.</returns>
        Vector2 Support(Vector2 direction, Transform2D transform);

        /// <summary>
        /// Returns a bounding box based on the shape.
        /// </summary>
        /// <param name="transform">A Transform for transforming the shape vertices.</param>
        /// <returns>Returns a bounding box based on the shape.</returns>
        AABB AABB(Transform2D transform);
    }
}
