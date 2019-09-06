/*
 * Implementation of the GJK collision algorithm
 * Based on some math blogs
 * https://blog.hamaluik.ca/posts/building-a-collision-engine-part-1-2d-gjk-collision-detection/
 * and some code from https://github.com/kroitor/gjk.c
 */

using Microsoft.Xna.Framework;
using MoonTools.Core.Structs;
using System;

namespace MoonTools.Core.Bonk
{
    public static class GJK2D
    {
        private enum SolutionStatus
        {
            NoIntersection,
            Intersection,
            StillSolving
        }

        public static ValueTuple<bool, SimplexVertices> TestCollision(IShape2D shapeA, Transform2D Transform2DA, IShape2D shapeB, Transform2D Transform2DB)
        {
            var vertices = new SimplexVertices(new Vector2?[] { null, null, null, null });

            const SolutionStatus solutionStatus = SolutionStatus.StillSolving;
            var direction = Transform2DB.Position - Transform2DA.Position;

            var result = (solutionStatus, direction);

            while (result.solutionStatus == SolutionStatus.StillSolving)
            {
                result = EvolveSimplex(shapeA, Transform2DA, shapeB, Transform2DB, vertices, result.direction);
            }

            return ValueTuple.Create(result.solutionStatus == SolutionStatus.Intersection, vertices);
        }

        private static (SolutionStatus, Vector2) EvolveSimplex(IShape2D shapeA, Transform2D Transform2DA, IShape2D shapeB, Transform2D Transform2DB, SimplexVertices vertices, Vector2 direction)
        {
            switch(vertices.Count)
            {
                case 0:
                    if (direction == Vector2.Zero)
                    {
                        direction = Vector2.UnitX;
                    }
                    break;

                case 1:
                    direction *= -1;
                    break;

                case 2:
                    var ab = vertices[1] - vertices[0];
                    var a0 = vertices[0] * -1;

                    direction = TripleProduct(ab, a0, ab);
                    if (direction == Vector2.Zero)
                    {
                        direction = Perpendicular(ab);
                    }
                    break;

                case 3:
                    var c0 = vertices[2] * -1;
                    var bc = vertices[1] - vertices[2];
                    var ca = vertices[0] - vertices[2];

                    var bcNorm = TripleProduct(ca, bc, bc);
                    var caNorm = TripleProduct(bc, ca, ca);

                    // the origin is outside line bc
                    // get rid of a and add a new support in the direction of bcNorm
                    if (Vector2.Dot(bcNorm, c0) > 0)
                    {
                        vertices.RemoveAt(0);
                        direction = bcNorm;
                    }
                    // the origin is outside line ca
                    // get rid of b and add a new support in the direction of caNorm
                    else if (Vector2.Dot(caNorm, c0) > 0)
                    {
                        vertices.RemoveAt(1);
                        direction = caNorm;
                    }
                    // the origin is inside both ab and ac,
                    // so it must be inside the triangle!
                    else
                    {
                        return (SolutionStatus.Intersection, direction);
                    }
                    break;
            }

            return (AddSupport(shapeA, Transform2DA, shapeB, Transform2DB, vertices, direction) ?
                SolutionStatus.StillSolving :
                SolutionStatus.NoIntersection, direction);
        }

        private static bool AddSupport(IShape2D shapeA, Transform2D Transform2DA, IShape2D shapeB, Transform2D Transform2DB, SimplexVertices vertices, Vector2 direction)
        {
            var newVertex = shapeA.Support(direction, Transform2DA) - shapeB.Support(-direction, Transform2DB);
            vertices.Add(newVertex);
            return Vector2.Dot(direction, newVertex) >= 0;
        }

        private static Vector2 TripleProduct(Vector2 a, Vector2 b, Vector2 c)
        {
            var A = new Vector3(a.X, a.Y, 0);
            var B = new Vector3(b.X, b.Y, 0);
            var C = new Vector3(c.X, c.Y, 0);

            var first = Vector3.Cross(A, B);
            var second = Vector3.Cross(first, C);

            return new Vector2(second.X, second.Y);
        }

        private static Vector2 Perpendicular(Vector2 v)
        {
            return new Vector2(v.Y, -v.X);
        }
    }
}
