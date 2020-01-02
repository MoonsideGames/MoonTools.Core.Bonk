using System;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    public interface IHasAABB2D
    {
        AABB AABB { get; }

        /// <summary>
        /// Returns a bounding box based on the shape.
        /// </summary>
        /// <param name="transform">A Transform for transforming the shape vertices.</param>
        /// <returns>Returns a bounding box based on the shape.</returns>
        AABB TransformedAABB(Transform2D transform);
    }
}
