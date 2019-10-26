using NUnit.Framework;
using FluentAssertions;

using Microsoft.Xna.Framework;
using System;
using MoonTools.Core.Structs;
using MoonTools.Core.Bonk;

namespace Tests
{
    public class EPA2DTest
    {
        [Test]
        public void RectangleOverlap()
        {
            var squareA = new MoonTools.Core.Bonk.Rectangle(-1, -1, 1, 1);
            var transformA = Transform2D.DefaultTransform;
            var squareB = new MoonTools.Core.Bonk.Rectangle(-1, -1, 1, 1);
            var transformB = new Transform2D(new Vector2(1.5f, 0));

            var (result, simplex) = GJK2D.FindCollisionSimplex(squareA, transformA, squareB, transformB);

            result.Should().BeTrue();

            var intersection = EPA2D.Intersect(squareA, transformA, squareB, transformB, simplex);

            intersection.X.Should().Be(1f);
            intersection.Y.Should().Be(0);

            var movedTransform = new Transform2D(transformA.Position - intersection * 1.01f); // move a tiny bit past

            GJK2D.TestCollision(squareA, movedTransform, squareB, transformB).Should().BeFalse();
        }

        [Test]
        public void CircleOverlap()
        {
            var circleA = new Circle(2);
            var transformA = Transform2D.DefaultTransform;
            var circleB = new Circle(1);
            var transformB = new Transform2D(new Vector2(1, 1));

            var (result, simplex) = GJK2D.FindCollisionSimplex(circleA, transformA, circleB, transformB);

            result.Should().BeTrue();

            var intersection = EPA2D.Intersect(circleA, transformA, circleB, transformB, simplex);

            var ix = circleA.Radius * (float)Math.Cos(Math.PI / 4) - (circleB.Radius * (float)Math.Cos(5 * Math.PI / 4) + transformB.Position.X);
            var iy = circleA.Radius * (float)Math.Sin(Math.PI / 4) - (circleB.Radius * (float)Math.Sin(5 * Math.PI / 4) + transformB.Position.Y);

            intersection.X.Should().BeApproximately(ix, 0.01f);
            intersection.Y.Should().BeApproximately(iy, 0.01f);

            var movedTransform = new Transform2D(transformA.Position - intersection * 1.01f); // move a tiny bit past

            GJK2D.TestCollision(circleA, movedTransform, circleB, transformB).Should().BeFalse();
        }

        [Test]
        public void LineRectangleOverlap()
        {
            var line = new Line(new Position2D(-4, -4), new Position2D(4, 4));
            var transformA = Transform2D.DefaultTransform;
            var square = new MoonTools.Core.Bonk.Rectangle(-1, -1, 1, 1);
            var transformB = Transform2D.DefaultTransform;

            var (result, simplex) = GJK2D.FindCollisionSimplex(line, transformA, square, transformB);

            result.Should().BeTrue();

            var intersection = EPA2D.Intersect(line, transformA, square, transformB, simplex);

            var movedTransform = new Transform2D(transformA.Position - intersection * 1.01f); // move a tiny bit past

            GJK2D.TestCollision(line, movedTransform, square, transformB).Should().BeFalse();
        }
    }
}
