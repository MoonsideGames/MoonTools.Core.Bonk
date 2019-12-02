using System.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Numerics;
using MoonTools.Core.Structs;
using MoreLinq;

namespace MoonTools.Core.Bonk
{
    /// <summary>
    /// A Shape defined by an arbitrary collection of vertices.
    /// NOTE: A Polygon must have more than 2 vertices, be convex, and should not have duplicate vertices.
    /// </summary>
    public struct Polygon : IShape2D, IEquatable<IShape2D>
    {
        private ImmutableArray<Position2D> vertices;

        public IEnumerable<Position2D> Vertices { get { return vertices == null ? Enumerable.Empty<Position2D>() : vertices; } }

        // vertices are local to the origin
        public Polygon(params Position2D[] vertices)
        {
            this.vertices = ImmutableArray.Create<Position2D>(vertices);
        }

        public Polygon(ImmutableArray<Position2D> vertices)
        {
            this.vertices = vertices;
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
                var q = from a in vertices
                        join b in otherPolygon.vertices on a equals b
                        select a;

                return vertices.Length == otherPolygon.vertices.Length && q.Count() == vertices.Length;
            }
            else if (other is Rectangle rectangle)
            {
                var q = from a in vertices
                        join b in rectangle.Vertices on a equals b
                        select a;

                return vertices.Length == 4 && q.Count() == vertices.Length;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(vertices, Vertices);
        }

        public static bool operator ==(Polygon a, Polygon b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Polygon a, Polygon b)
        {
            return !(a == b);
        }

        public static bool operator ==(Polygon a, Rectangle b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Polygon a, Rectangle b)
        {
            return !(a == b);
        }
    }
}
