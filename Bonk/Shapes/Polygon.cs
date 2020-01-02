using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Numerics;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    /// <summary>
    /// A Shape defined by an arbitrary collection of vertices.
    /// NOTE: A Polygon must be defined in clockwise order, have more than 2 vertices, be convex, and have no duplicate vertices.
    /// </summary>
    public struct Polygon : IShape2D, IEquatable<Polygon>
    {
        public ImmutableArray<Position2D> Vertices { get; private set; }
        public AABB AABB { get; }

        public int VertexCount { get { return Vertices.Length; } }

        // vertices are local to the origin
        public Polygon(IEnumerable<Position2D> vertices)
        {
            Vertices = vertices.ToImmutableArray();
            AABB = AABB.FromVertices(vertices);
        }

        public Polygon(ImmutableArray<Position2D> vertices)
        {
            Vertices = vertices;
            AABB = AABB.FromVertices(vertices);
        }

        public Vector2 Support(Vector2 direction, Transform2D transform)
        {
            var maxDotProduct = float.NegativeInfinity;
            var maxVertex = Vertices[0].ToVector2();
            foreach (var vertex in Vertices)
            {
                var transformed = Vector2.Transform(vertex, transform.TransformMatrix);
                var dot = Vector2.Dot(transformed, direction);
                if (dot > maxDotProduct)
                {
                    maxVertex = transformed;
                    maxDotProduct = dot;
                }
            }
            return maxVertex;
        }

        public AABB TransformedAABB(Transform2D transform)
        {
            return AABB.Transformed(AABB, transform);
        }

        public override bool Equals(object obj)
        {
            return obj is IShape2D other && Equals(other);
        }

        public bool Equals(IShape2D other)
        {
            return (other is Polygon otherPolygon && Equals(otherPolygon));
        }

        public bool Equals(Polygon other)
        {
            if (VertexCount != other.VertexCount) { return false; }

            int? offset = null;
            for (var i = 0; i < VertexCount; i++)
            {
                if (Vertices[0] == other.Vertices[i]) { offset = i; break; }
            }

            if (!offset.HasValue) { return false; }

            for (var i = 0; i < VertexCount; i++)
            {
                if (Vertices[i] != other.Vertices[(i + offset.Value) % VertexCount]) { return false; }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Vertices);
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
