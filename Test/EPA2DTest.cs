using NUnit.Framework;
using FluentAssertions;

using System;
using System.Numerics;
using MoonTools.Core.Structs;
using MoonTools.Core.Bonk;

namespace Tests
{
    public class EPA2DTest
    {
        [Test]
        public void RectangleOverlap()
        {
            var squareA = new TransformedShape2D<Rectangle>(new Rectangle(2, 2), Transform2D.DefaultTransform);
            var squareB = new TransformedShape2D<Rectangle>(new Rectangle(2, 2), new Transform2D(new Vector2(1.5f, 0)));

            var (result, simplex) = NarrowPhase.FindCollisionSimplex(squareA, squareB);

            result.Should().BeTrue();

            var intersection = NarrowPhase.Intersect(squareA, squareB, simplex);

            intersection.X.Should().Be(1f);
            intersection.Y.Should().Be(0);

            var movedTransform = new Transform2D(-(intersection * 1.01f)); // move a tiny bit past

            NarrowPhase.TestCollision(squareA.Compose(movedTransform), squareB).Should().BeFalse();
        }

        [Test]
        public void CircleOverlap()
        {
            var circleA = new TransformedShape2D<Circle>(new Circle(2), Transform2D.DefaultTransform);
            var circleB = new TransformedShape2D<Circle>(new Circle(1), new Transform2D(new Vector2(1, 1)));

            var (result, simplex) = NarrowPhase.FindCollisionSimplex(circleA, circleB);

            result.Should().BeTrue();

            var intersection = NarrowPhase.Intersect(circleA, circleB, simplex);

            var ix = (2 * (float)Math.Cos(Math.PI / 4)) - ((1 * (float)Math.Cos(5 * Math.PI / 4)) + 1);
            var iy = (2 * (float)Math.Sin(Math.PI / 4)) - ((1 * (float)Math.Sin(5 * Math.PI / 4)) + 1);

            intersection.X.Should().BeApproximately(ix, 0.01f);
            intersection.Y.Should().BeApproximately(iy, 0.01f);

            var movedTransform = new Transform2D(-(intersection * 1.01f)); // move a tiny bit past

            NarrowPhase.TestCollision(circleA.Compose(movedTransform), circleB).Should().BeFalse();
        }

        [Test]
        public void LineRectangleOverlap()
        {
            var line = new TransformedShape2D<Line>(new Line(new Position2D(-4, -4), new Position2D(4, 4)), Transform2D.DefaultTransform);
            var square = new TransformedShape2D<Rectangle>(new Rectangle(2, 2), Transform2D.DefaultTransform);

            var (result, simplex) = NarrowPhase.FindCollisionSimplex(line, square);

            result.Should().BeTrue();

            var intersection = NarrowPhase.Intersect(line, square, simplex);

            var movedTransform = new Transform2D(-(intersection * 1.01f)); // move a tiny bit past

            NarrowPhase.TestCollision(line.Compose(movedTransform), square).Should().BeFalse();
        }
    }
}
