using NUnit.Framework;
using MoonTools.Core.Bonk;
using MoonTools.Core.Structs;
using System.Numerics;
using FluentAssertions;

namespace Tests
{
    public class GJK2DTest
    {
        [Test]
        public void LineLineOverlapping()
        {
            var lineA = new Line(new Position2D(-1, -1), new Position2D(1, 1));
            var lineB = new Line(new Position2D(-1, 1), new Position2D(1, -1));

            GJK2D.TestCollision(lineA, Transform2D.DefaultTransform, lineB, Transform2D.DefaultTransform).Should().BeTrue();
        }

        [Test]
        public void ScaledLinesOverlapping()
        {
            var lineA = new Line(new Position2D(-1, -1), new Position2D(1, 1));
            var lineB = new Line(new Position2D(-1, 1), new Position2D(1, -1));

            var transform = new Transform2D(new Position2D(0, 0), 0f, new Vector2(2, 2));

            GJK2D.TestCollision(lineA, transform, lineB, transform).Should().BeTrue();
        }

        [Test]
        public void LineLineNotOverlapping()
        {
            var lineA = new Line(new Position2D(0, 1), new Position2D(1, 0));
            var lineB = new Line(new Position2D(-1, -1), new Position2D(-2, -2));

            GJK2D.TestCollision(lineA, Transform2D.DefaultTransform, lineB, Transform2D.DefaultTransform).Should().BeFalse();
        }

        [Test]
        public void ScaledLinesNotOverlapping()
        {
            var lineA = new Line(new Position2D(0, 1), new Position2D(1, 0));
            var lineB = new Line(new Position2D(-1, -1), new Position2D(-2, -2));

            var transform = new Transform2D(new Position2D(0, 0), 0f, new Vector2(2, 2));

            GJK2D.TestCollision(lineA, transform, lineB, transform).Should().BeFalse();
        }

        [Test]
        public void CircleCircleOverlapping()
        {
            var circleA = new Circle(2);
            var transformA = new Transform2D(new Vector2(-1, -1));
            var circleB = new Circle(2);
            var transformB = new Transform2D(new Vector2(1, 1));

            GJK2D.TestCollision(circleA, transformA, circleB, transformB).Should().BeTrue();
        }

        [Test]
        public void ScaledCirclesOverlapping()
        {
            var circleA = new Circle(2);
            var transformA = new Transform2D(new Vector2(-3, 0), 0f, new Vector2(2, 2));
            var circleB = new Circle(2);
            var transformB = new Transform2D(new Vector2(3, 0), 0f, new Vector2(2, 2));

            GJK2D.TestCollision(circleA, transformA, circleB, transformB).Should().BeTrue();
        }

        [Test]
        public void CircleCircleNotOverlapping()
        {
            var circleA = new Circle(2);
            var transformA = new Transform2D(new Vector2(-5, -5));
            var circleB = new Circle(2);
            var transformB = new Transform2D(new Vector2(5, 5));

            GJK2D.TestCollision(circleA, transformA, circleB, transformB).Should().BeFalse();
        }

        [Test]
        public void ScaledCirclesNotOverlapping()
        {
            var circleA = new Circle(2);
            var transformA = new Transform2D(new Vector2(-5, -5), 0, new Vector2(0.2f, 0.2f));
            var circleB = new Circle(2);
            var transformB = new Transform2D(new Vector2(5, 5), 0, new Vector2(0.2f, 0.2f));

            GJK2D.TestCollision(circleA, transformA, circleB, transformB).Should().BeFalse();
        }

        [Test]
        public void PolygonPolygonOverlapping()
        {
            var shapeA = new Polygon(
                new Position2D(-1, 1), new Position2D(1, 1),
                new Position2D(-1, -1), new Position2D(1, -1)
            );

            var transformA = Transform2D.DefaultTransform;

            var shapeB = new Polygon(
                new Position2D(-1, 1), new Position2D(1, 1),
                new Position2D(-1, -1), new Position2D(1, -1)
            );

            var transformB = new Transform2D(new Vector2(0.5f, 0.5f));

            GJK2D.TestCollision(shapeA, transformA, shapeB, transformB).Should().BeTrue();
        }

        [Test]
        public void ScaledPolygonsOverlapping()
        {
            var shapeA = new Polygon(
                new Position2D(-1, 1), new Position2D(1, 1),
                new Position2D(-1, -1), new Position2D(1, -1)
            );

            var transformA = Transform2D.DefaultTransform;

            var shapeB = new Polygon(
                new Position2D(-1, 1), new Position2D(1, 1),
                new Position2D(-1, -1), new Position2D(1, -1)
            );

            var transformB = new Transform2D(new Vector2(3f, 0f), 0f, new Vector2(3f, 3f));

            GJK2D.TestCollision(shapeA, transformA, shapeB, transformB).Should().BeTrue();
        }

        [Test]
        public void PolygonPolygonNotOverlapping()
        {
            var shapeA = new Polygon(
                new Position2D(-1, 1), new Position2D(1, 1),
                new Position2D(-1, -1), new Position2D(1, -1)
            );

            var transformA = Transform2D.DefaultTransform;

            var shapeB = new Polygon(
                new Position2D(-1, 1), new Position2D(1, 1),
                new Position2D(-1, -1), new Position2D(1, -1)
            );

            var transformB = new Transform2D(new Vector2(5, 0));

            GJK2D.TestCollision(shapeA, transformA, shapeB, transformB).Should().BeFalse();
        }

        [Test]
        public void ScaledPolygonsNotOverlapping()
        {
            var shapeA = new Polygon(
                new Position2D(-1, 1), new Position2D(1, 1),
                new Position2D(-1, -1), new Position2D(1, -1)
            );

            var transformA = Transform2D.DefaultTransform;

            var shapeB = new Polygon(
                new Position2D(-2, 2), new Position2D(2, 2),
                new Position2D(-2, -2), new Position2D(2, -2)
            );

            var transformB = new Transform2D(new Vector2(3f, 0), 0f, new Vector2(0.5f, 0.5f));

            GJK2D.TestCollision(shapeA, transformA, shapeB, transformB).Should().BeFalse();
        }

        [Test]
        public void LinePolygonOverlapping()
        {
            var line = new Line(new Position2D(-1, -1), new Position2D(1, 1));

            var transformA = Transform2D.DefaultTransform;

            var polygon = new Polygon(
                new Position2D(-1, -1), new Position2D(1, -1),
                new Position2D(1, 1), new Position2D(-1, 1)
            );

            var transformB = Transform2D.DefaultTransform;

            GJK2D.TestCollision(line, transformA, polygon, transformB).Should().BeTrue();
        }

        [Test]
        public void LinePolygonNotOverlapping()
        {
            var line = new Line(new Position2D(-5, 5), new Position2D(-5, 5));

            var transformA = Transform2D.DefaultTransform;

            var polygon = new Polygon(
                new Position2D(-1, -1), new Position2D(1, -1),
                new Position2D(1, 1), new Position2D(-1, 1)
            );

            var transformB = Transform2D.DefaultTransform;

            GJK2D.TestCollision(line, transformA, polygon, transformB).Should().BeFalse();
        }

        [Test]
        public void LineCircleOverlapping()
        {
            var line = new Line(new Position2D(-1, -1), new Position2D(1, 1));
            var transformA = Transform2D.DefaultTransform;
            var circle = new Circle(1);
            var transformB = Transform2D.DefaultTransform;

            GJK2D.TestCollision(line, transformA, circle, transformB).Should().BeTrue();
        }

        [Test]
        public void LineCircleNotOverlapping()
        {
            var line = new Line(new Position2D(-5, -5), new Position2D(-4, -4));
            var transformA = Transform2D.DefaultTransform;
            var circle = new Circle(1);
            var transformB = Transform2D.DefaultTransform;

            GJK2D.TestCollision(line, transformA, circle, transformB).Should().BeFalse();
        }

        [Test]
        public void CirclePolygonOverlapping()
        {
            var circle = new Circle(1);
            var transformA = new Transform2D(new Vector2(0.25f, 0));

            var square = new Polygon(
                new Position2D(-1, -1), new Position2D(1, -1),
                new Position2D(1, 1), new Position2D(-1, 1)
            );

            var transformB = Transform2D.DefaultTransform;

            GJK2D.TestCollision(circle, transformA, square, transformB).Should().BeTrue();
        }

        [Test]
        public void CirclePolygonNotOverlapping()
        {
            var circle = new Circle(1);
            var circleTransform = new Transform2D(new Vector2(5, 0));

            var square = new Polygon(
                new Position2D(-1, -1), new Position2D(1, -1),
                new Position2D(1, 1), new Position2D(-1, 1)
            );
            var squareTransform = Transform2D.DefaultTransform;

            GJK2D.TestCollision(circle, circleTransform, square, squareTransform).Should().BeFalse();
        }

        [Test]
        public void RectanglesNotOverlapping()
        {
            var rectangleA = new MoonTools.Core.Bonk.Rectangle(-6, -6, 6, 6);
            var transformA = new Transform2D(new Position2D(39, 249));

            var rectangleB = new MoonTools.Core.Bonk.Rectangle(0, 0, 16, 16);
            var transformB = new Transform2D(new Position2D(16, 240));

            GJK2D.TestCollision(rectangleA, transformA, rectangleB, transformB).Should().BeFalse();
        }

        [Test]
        public void RotatedRectanglesOverlapping()
        {
            var rectangleA = new MoonTools.Core.Bonk.Rectangle(-1, -1, 2, 2);
            var transformA = new Transform2D(new Vector2(-1, 0), -90f);

            var rectangleB = new MoonTools.Core.Bonk.Rectangle(-1, -1, 1, 1);
            var transformB = new Transform2D(new Vector2(1, 0));

            GJK2D.TestCollision(rectangleA, transformA, rectangleB, transformB).Should().BeTrue();
        }

        [Test]
        public void RectanglesTouching()
        {
            var rectangleA = new MoonTools.Core.Bonk.Rectangle(-1, -1, 1, 1);
            var transformA = new Transform2D(new Position2D(-1, 0));

            var rectangleB = new MoonTools.Core.Bonk.Rectangle(-1, -1, 1, 1);
            var transformB = new Transform2D(new Vector2(1, 0));

            GJK2D.TestCollision(rectangleA, transformA, rectangleB, transformB).Should().BeTrue();
        }
    }
}
