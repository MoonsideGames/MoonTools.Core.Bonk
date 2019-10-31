﻿using System;
using System.Numerics;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    /// <summary>
    /// A Circle is a shape defined by a radius.
    /// </summary>
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

        public AABB AABB(Transform2D transform2D)
        {
            return new AABB(
                transform2D.Position.X - Radius * transform2D.Scale.X,
                transform2D.Position.Y - Radius * transform2D.Scale.Y,
                transform2D.Position.X + Radius * transform2D.Scale.X,
                transform2D.Position.Y + Radius * transform2D.Scale.Y
            );
        }

        public override bool Equals(object obj)
        {
            if (obj is Circle other)
            {
                return Equals(other);
            }

            return false;
        }

        public bool Equals(IShape2D other)
        {
            if (other is Circle circle)
            {
                return Radius == circle.Radius;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return 598075851 + Radius.GetHashCode();
        }

        public static bool operator ==(Circle a, Circle b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Circle a, Circle b)
        {
            return !(a == b);
        }
    }
}
