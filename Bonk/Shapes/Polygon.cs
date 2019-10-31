using System.Linq;
using System;
using System.Collections.Generic;
using System.Numerics;
using Collections.Pooled;
using MoonTools.Core.Structs;
using MoreLinq;

namespace MoonTools.Core.Bonk
{
    /// <summary>
    /// A Shape defined by an arbitrary collection of vertices. WARNING: Polygon must use an Array internally and therefore will create GC pressure.
    /// </summary>
    public struct Polygon : IShape2D, IEquatable<IShape2D>
    {
        private PooledSet<Position2D> vertices;

        public IEnumerable<Position2D> Vertices { get { return vertices == null ? Enumerable.Empty<Position2D>() : vertices; } }

        // vertices are local to the origin
        public Polygon(params Position2D[] vertices)
        {
            this.vertices = new PooledSet<Position2D>(vertices, ClearMode.Always);
        }

        public Vector2 Support(Vector2 direction, Transform2D transform)
        {
            return Vertices.Select(vertex => Vector2.Transform(vertex, transform.TransformMatrix)).MaxBy(transformed => Vector2.Dot(transformed, direction)).First();
        }

        public AABB AABB(Transform2D Transform2D)
        {
            return Bonk.AABB.FromTransformedVertices(Vertices, Transform2D);
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
            if (other is Polygon otherPolygon)
            {
                return vertices.SetEquals(otherPolygon.vertices);
            }

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -1404792980;
            hashCode = hashCode * -1521134295 + EqualityComparer<PooledSet<Position2D>>.Default.GetHashCode(vertices);
            hashCode = hashCode * -1521134295 + EqualityComparer<IEnumerable<Position2D>>.Default.GetHashCode(Vertices);
            return hashCode;
        }

        public static bool operator ==(Polygon a, Polygon b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Polygon a, Polygon b)
        {
            return !(a == b);
        }
    }
}
