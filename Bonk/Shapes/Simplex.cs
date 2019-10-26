using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MoonTools.Core.Structs;
using MoonTools.Core.Bonk.Extensions;
using MoreLinq;

namespace MoonTools.Core.Bonk
{
    /// <summary>
    /// A simplex is a shape with up to n - 2 vertices in the nth dimension.
    /// </summary>
    public struct Simplex2D : IShape2D
    {
        Vector2 a;
        Vector2? b;
        Vector2? c;

        public Vector2 A => a;
        public Vector2? B => b;
        public Vector2? C => c;

        public bool ZeroSimplex { get { return !b.HasValue && !c.HasValue; } }
        public bool OneSimplex { get { return b.HasValue && !c.HasValue; } }
        public bool TwoSimplex { get { return b.HasValue && c.HasValue; } }

        public int Count => TwoSimplex ? 3 : (OneSimplex ? 2 : 1);

        public Simplex2D(Vector2 a)
        {
            this.a = a;
            this.b = null;
            this.c = null;
        }

        public Simplex2D(Vector2 a, Vector2 b)
        {
            this.a = a;
            this.b = b;
            this.c = null;
        }

        public Simplex2D(Vector2 a, Vector2 b, Vector2 c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public IEnumerable<Position2D> Vertices
        {
            get
            {
                yield return (Position2D)a;
                if (b.HasValue) { yield return (Position2D)b; }
                if (c.HasValue) { yield return (Position2D)c; }
            }
        }

        public AABB AABB(Transform2D transform)
        {
            return Bonk.AABB.FromTransformedVertices(Vertices, transform);
        }

        public Vector2 Support(Vector2 direction)
        {
            return Vertices.MaxBy(vertex => Vector2.Dot(vertex, direction)).First();
        }

        public Vector2 Support(Vector2 direction, Transform2D transform)
        {
            return Vector2.Transform(Support(direction), transform.TransformMatrix);
        }

        public override bool Equals(object obj)
        {
            if (obj is IShape2D other)
            {
                return Equals(other);
            }

            return false;
        }

        public bool Equals(IShape2D other)
        {
            if (other is Simplex2D otherSimplex)
            {
                if (Count != otherSimplex.Count) { return false; }
                return Vertices.Intersect(otherSimplex.Vertices).Count() == Count;
            }

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -495772172;
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector2>.Default.GetHashCode(a);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector2?>.Default.GetHashCode(b);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector2?>.Default.GetHashCode(c);
            hashCode = hashCode * -1521134295 + ZeroSimplex.GetHashCode();
            hashCode = hashCode * -1521134295 + OneSimplex.GetHashCode();
            hashCode = hashCode * -1521134295 + TwoSimplex.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<IEnumerable<Position2D>>.Default.GetHashCode(Vertices);
            return hashCode;
        }

        public static bool operator ==(Simplex2D a, Simplex2D b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Simplex2D a, Simplex2D b)
        {
            return !(a == b);
        }
    }
}