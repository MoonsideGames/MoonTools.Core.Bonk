using System;
using Microsoft.Xna.Framework;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    /// <summary>
    /// A Shape defined by an arbitrary collection of vertices. WARNING: Polygon must use an Array internally and therefore will create GC pressure.
    /// </summary>
    public struct Polygon : IShape2D, IEquatable<IShape2D>
    {
        public Position2D[] Vertices { get; private set; }

        // vertices are local to the origin
        public Polygon(params Position2D[] vertices)
        {
            Vertices = vertices;
        }

        public Vector2 Support(Vector2 direction, Transform2D transform)
        {
            var furthest = float.NegativeInfinity;
            var furthestVertex = Vector2.Transform(Vertices[0], transform.TransformMatrix);

            foreach (var vertex in Vertices)
            {
                var TransformedVertex = Vector2.Transform(vertex, transform.TransformMatrix);
                var distance = Vector2.Dot(TransformedVertex, direction);
                if (distance > furthest)
                {
                    furthest = distance;
                    furthestVertex = TransformedVertex;
                }
            }

            return furthestVertex;
        }

        public AABB AABB(Transform2D Transform2D)
        {
            return Bonk.AABB.FromTransformedVertices(Vertices, Transform2D);
        }

        public bool Equals(IShape2D other)
        {
            if (other is Polygon)
            {
                var otherPolygon = (Polygon)other;

                if (Vertices.Length != otherPolygon.Vertices.Length) { return false; }

                for (int i = 0; i < Vertices.Length; i++)
                {
                    if (Vertices[i].ToVector2() != otherPolygon.Vertices[i].ToVector2()) { return false; }
                }

                return true;
            }

            return false;
        }
    }
}
