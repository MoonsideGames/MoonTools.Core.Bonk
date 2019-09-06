using System;
using Microsoft.Xna.Framework;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    public struct Circle : IShape2D, IEquatable<IShape2D>
    {
        public int Radius { get; private set; }

        public Circle(int radius)
        {
            Radius = radius;
        }

        public Vector2 Support(Vector2 direction, Transform2D transform)
        {
            return Vector2.Transform(Vector2.Normalize(direction) * Radius, transform.TransformMatrix);
        }

        public AABB AABB(Transform2D Transform2D)
        {
            return new AABB(
                Transform2D.Position.X - Radius,
                Transform2D.Position.Y - Radius,
                Transform2D.Position.X + Radius,
                Transform2D.Position.Y + Radius
            );
        }

        public bool Equals(IShape2D other)
        {
            if (other is Circle circle)
            {
                return Radius == circle.Radius;
            }

            return false;
        }
    }
}
