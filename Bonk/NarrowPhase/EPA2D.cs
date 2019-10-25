/*
 * Implementation of the Expanding Polytope Algorithm
 * as based on the following blog post:
 * https://blog.hamaluik.ca/posts/building-a-collision-engine-part-2-2d-penetration-vectors/
 */

using Collections.Pooled;
using Microsoft.Xna.Framework;
using MoonTools.Core.Structs;
using System;

namespace MoonTools.Core.Bonk
{
    enum PolygonWinding
    {
        Clockwise,
        CounterClockwise
    }

    public static class EPA2D
    {
        // vector returned gives direction from A to B
        public static Vector2 Intersect(IShape2D shapeA, Transform2D Transform2DA, IShape2D shapeB, Transform2D Transform2DB, Simplex simplex)
        {
            var simplexVertices = new PooledList<Vector2>(36, ClearMode.Always);

            foreach (var vertex in simplex.Vertices)
            {
                simplexVertices.Add(vertex);
            }

            var e0 = (simplexVertices[1].X - simplexVertices[0].X) * (simplexVertices[1].Y + simplexVertices[0].Y);
            var e1 = (simplexVertices[2].X - simplexVertices[1].X) * (simplexVertices[2].Y + simplexVertices[1].Y);
            var e2 = (simplexVertices[0].X - simplexVertices[2].X) * (simplexVertices[0].Y + simplexVertices[2].Y);
            var winding = e0 + e1 + e2 >= 0 ? PolygonWinding.Clockwise : PolygonWinding.CounterClockwise;

            Vector2 intersection = default;

            for (int i = 0; i < 32; i++)
            {
                var edge = FindClosestEdge(winding, simplexVertices);
                var support = CalculateSupport(shapeA, Transform2DA, shapeB, Transform2DB, edge.normal);
                var distance = Vector2.Dot(support, edge.normal);

                intersection = edge.normal;
                intersection *= distance;

                if (Math.Abs(distance - edge.distance) <= float.Epsilon)
                {
                    return intersection;
                }
                else
                {
                    simplexVertices.Insert(edge.index, support);
                }
            }

            simplexVertices.Dispose();

            return intersection;
        }

        private static Edge FindClosestEdge(PolygonWinding winding, PooledList<Vector2> simplexVertices)
        {
            var closestDistance = float.PositiveInfinity;
            var closestNormal = Vector2.Zero;
            var closestIndex = 0;

            for (int i = 0; i < simplexVertices.Count; i++)
            {
                var j = i + 1;
                if (j >= simplexVertices.Count) { j = 0; }
                Vector2 edge = simplexVertices[j] - simplexVertices[i];

                Vector2 norm;
                if (winding == PolygonWinding.Clockwise)
                {
                    norm = new Vector2(edge.Y, -edge.X);
                }
                else
                {
                    norm = new Vector2(-edge.Y, edge.X);
                }
                norm.Normalize();

                var dist = Vector2.Dot(norm, simplexVertices[i]);

                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closestNormal = norm;
                    closestIndex = j;
                }
            }

            return new Edge(closestDistance, closestNormal, closestIndex);
        }

        private static Vector2 CalculateSupport(IShape2D shapeA, Transform2D Transform2DA, IShape2D shapeB, Transform2D Transform2DB, Vector2 direction)
        {
            return shapeA.Support(direction, Transform2DA) - shapeB.Support(-direction, Transform2DB);
        }
    }
}
