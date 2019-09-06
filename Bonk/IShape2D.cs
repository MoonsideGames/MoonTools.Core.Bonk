using System;
using Microsoft.Xna.Framework;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    public interface IShape2D : IEquatable<IShape2D>
    {
        // A Support function for a Minkowski sum.
        // A Support function gives the point on the edge of a shape based on a direction.
        Vector2 Support(Vector2 direction, Transform2D transform);

        AABB AABB(Transform2D transform);
    }
}
