using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    /// <summary>
    /// A Minkowski difference between two shapes.
    /// </summary>
    public struct MinkowskiDifference : IEquatable<MinkowskiDifference>
    {
        private IShape2D shapeA;
        private Transform2D transformA;
        private IShape2D shapeB;
        private Transform2D transformB;

        public MinkowskiDifference(IShape2D shapeA, Transform2D transformA, IShape2D shapeB, Transform2D transformB)
        {
            this.shapeA = shapeA;
            this.transformA = transformA;
            this.shapeB = shapeB;
            this.transformB = transformB;
        }

        public Vector2 Support(Vector2 direction)
        {
            return shapeA.Support(direction, transformA) - shapeB.Support(-direction, transformB);
        }

        public override bool Equals(object other)
        {
            if (other is MinkowskiDifference otherMinkowskiDifference)
            {
                return Equals(otherMinkowskiDifference);
            }

            return false;
        }

        public bool Equals(MinkowskiDifference other)
        {
            return
                shapeA == other.shapeA &&
                transformA == other.transformA &&
                shapeB == other.shapeB &&
                transformB == other.transformB;
        }

        public override int GetHashCode()
        {
            var hashCode = 974363698;
            hashCode = hashCode * -1521134295 + EqualityComparer<IShape2D>.Default.GetHashCode(shapeA);
            hashCode = hashCode * -1521134295 + EqualityComparer<Transform2D>.Default.GetHashCode(transformA);
            hashCode = hashCode * -1521134295 + EqualityComparer<IShape2D>.Default.GetHashCode(shapeB);
            hashCode = hashCode * -1521134295 + EqualityComparer<Transform2D>.Default.GetHashCode(transformB);
            return hashCode;
        }

        public static bool operator ==(MinkowskiDifference a, MinkowskiDifference b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(MinkowskiDifference a, MinkowskiDifference b)
        {
            return !(a == b);
        }
    }
}