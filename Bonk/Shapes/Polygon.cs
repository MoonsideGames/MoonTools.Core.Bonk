﻿using System.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Numerics;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    /// <summary>
    /// A Shape defined by an arbitrary collection of vertices.
    /// NOTE: A Polygon must have more than 2 vertices, be convex, and should not have duplicate vertices.
    /// </summary>
    public struct Polygon : IShape2D, IEquatable<Polygon>
    {
        private ImmutableArray<Position2D> vertices;

        public IEnumerable<Position2D> Vertices { get { return vertices; } }

        public int VertexCount { get { return vertices.Length; } }

        // vertices are local to the origin
        public Polygon(IEnumerable<Position2D> vertices) // TODO: remove this, params is bad because it allocates an array
        {
            this.vertices = vertices.ToImmutableArray();
        }

        public Polygon(ImmutableArray<Position2D> vertices)
        {
            this.vertices = vertices;
        }

        public Vector2 Support(Vector2 direction, Transform2D transform)
        {
            var maxDotProduct = float.NegativeInfinity;
            var maxVertex = vertices[0].ToVector2();
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

        public AABB AABB(Transform2D Transform2D)
        {
            return Bonk.AABB.FromTransformedVertices(Vertices, Transform2D);
        }

        public override bool Equals(object obj)
        {
            return obj is IShape2D other && Equals(other);
        }

        public bool Equals(IShape2D other)
        {
            return (other is Polygon otherPolygon && Equals(otherPolygon)) || (other is Rectangle rectangle && Equals(rectangle));
        }

        public bool Equals(Polygon other)
        {
            var q = from a in vertices
                    join b in other.Vertices on a equals b
                    select a;

            return vertices.Length == other.VertexCount && q.Count() == vertices.Length;
        }

        public bool Equals(Rectangle rectangle)
        {
            return RectanglePolygonComparison.Equals(this, rectangle);
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
