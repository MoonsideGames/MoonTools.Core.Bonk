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

            var test = GJK2D.TestCollision(squareA, transformA, squareB, transformB);

            Assert.That(test.Item1, Is.True);

            var intersection = EPA2D.Intersect(squareA, transformA, squareB, transformB, test.Item2);

            intersection.X.Should().Be(1f);
            intersection.Y.Should().Be(0);
        }

        [Test]
        public void CircleOverlap()
        {
            var circleA = new Circle(2);
            var transformA = Transform2D.DefaultTransform;
            var circleB = new Circle(1);
            var transformB = new Transform2D(new Vector2(1, 1));

            var test = GJK2D.TestCollision(circleA, transformA, circleB, transformB);

            Assert.That(test.Item1, Is.True);

            var intersection = EPA2D.Intersect(circleA, transformA, circleB, transformB, test.Item2);

            var ix = circleA.Radius * (float)Math.Cos(Math.PI / 4) - (circleB.Radius * (float)Math.Cos(5 * Math.PI / 4) + transformB.Position.X);
            var iy = circleA.Radius * (float)Math.Sin(Math.PI / 4) - (circleB.Radius * (float)Math.Sin(5 * Math.PI / 4) + transformB.Position.Y);

            intersection.X.Should().BeApproximately(ix, 0.01f);
            intersection.Y.Should().BeApproximately(iy, 0.01f);
        }

        [Test]
        public void LineRectangleOverlap()
        {
            var line = new Line(new Position2D(-4, -4), new Position2D(4, 4));
            var transformA = Transform2D.DefaultTransform;
            var square = new MoonTools.Core.Bonk.Rectangle(-1, -1, 1, 1);
            var transformB = Transform2D.DefaultTransform;

            var test = GJK2D.TestCollision(line, transformA, square, transformB);

            Assert.That(test.Item1, Is.True);

            var intersection = EPA2D.Intersect(line, transformA, square, transformB, test.Item2);

            intersection.X.Should().Be(-1);
            intersection.Y.Should().Be(1);
        }
    }
}
