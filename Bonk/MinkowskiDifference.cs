using System;
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

        public bool Equals(MinkowskiDifference other)
        {
            return
                shapeA == other.shapeA &&
                transformA.Equals(other.transformA) &&
                shapeB == other.shapeB &&
                transformB.Equals(other.transformB);
        }

        public Vector2 Support(Vector2 direction)
        {
            return shapeA.Support(direction, transformA) - shapeB.Support(-direction, transformB);
        }
    }
}