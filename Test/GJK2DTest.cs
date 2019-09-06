using NUnit.Framework;
using MoonTools.Core.Bonk;
using MoonTools.Core.Structs;
using Microsoft.Xna.Framework;

namespace Tests
{
    public class GJK2DTest
    {
        [Test]
        public void LineLineOverlapping()
        {
            var lineA = new Line(new Position2D(-1, -1), new Position2D(1, 1));
            var lineB = new Line(new Position2D(-1, 1), new Position2D(1, -1));

            Assert.IsTrue(GJK2D.TestCollision(lineA, Transform2D.DefaultTransform, lineB, Transform2D.DefaultTransform).Item1);
        }

        [Test]
        public void LineLineNotOverlapping()
        {
            var lineA = new Line(new Position2D(0, 1), new Position2D(1, 0));
            var lineB = new Line(new Position2D(-1, -1), new Position2D(-2, -2));

            Assert.IsFalse(GJK2D.TestCollision(lineA, Transform2D.DefaultTransform, lineB, Transform2D.DefaultTransform).Item1);
        }

        [Test]
        public void CircleCircleOverlapping()
        {
            var circleA = new Circle(2);
            var transformA = new Transform2D(new Vector2(-1, -1));
            var circleB = new Circle(2);
            var transformB = new Transform2D(new Vector2(1, 1));

            Assert.IsTrue(GJK2D.TestCollision(circleA, transformA, circleB, transformB).Item1);
        }

        [Test]
        public void CircleCircleNotOverlapping()
        {
            var circleA = new Circle(2);
            var transformA = new Transform2D(new Vector2(-5, -5));
            var circleB = new Circle(2);
            var transformB = new Transform2D(new Vector2(5, 5));

            Assert.IsFalse(GJK2D.TestCollision(circleA, transformA, circleB, transformB).Item1);
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

            Assert.IsTrue(GJK2D.TestCollision(shapeA, transformA, shapeB, transformB).Item1);
        }

        [Test]
        public void PolygonPolygonNotOverlapping()
        {
            var shapeA = new Polygon(new Position2D(0, 0),
                new Position2D(-1, 1), new Position2D(1, 1),
                new Position2D(-1, -1), new Position2D(1, -1)
            );

            var transformA = Transform2D.DefaultTransform;

            var shapeB = new Polygon(
                new Position2D(-1, 1), new Position2D(1, 1),
                new Position2D(-1, -1), new Position2D(1, -1)
            );

            var transformB = new Transform2D(new Vector2(5, 0));

            Assert.IsFalse(GJK2D.TestCollision(shapeA, transformA, shapeB, transformB).Item1);
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

            Assert.IsTrue(GJK2D.TestCollision(line, transformA, polygon, transformB).Item1);
        }

        [Test]
        public void LinePolygonNotOverlapping()
        {
            var line = new Line(new Position2D(-5, 5), new Position2D(-5, 5));

            var transformA = Transform2D.DefaultTransform;

            var polygon = new Polygon(new Position2D(0, 0),
                new Position2D(-1, -1), new Position2D(1, -1),
                new Position2D(1, 1), new Position2D(-1, 1)
            );

            var transformB = Transform2D.DefaultTransform;

            Assert.IsFalse(GJK2D.TestCollision(line, transformA, polygon, transformB).Item1);
        }

        [Test]
        public void LineCircleOverlapping()
        {
            var line = new Line(new Position2D(-1, -1), new Position2D(1, 1));
            var transformA = Transform2D.DefaultTransform;
            var circle = new Circle(1);
            var transformB = Transform2D.DefaultTransform;

            Assert.IsTrue(GJK2D.TestCollision(line, transformA, circle, transformB).Item1);
        }

        [Test]
        public void LineCircleNotOverlapping()
        {
            var line = new Line(new Position2D(-5, -5), new Position2D(-4, -4));
            var transformA = Transform2D.DefaultTransform;
            var circle = new Circle(1);
            var transformB = Transform2D.DefaultTransform;

            Assert.IsFalse(GJK2D.TestCollision(line, transformA, circle, transformB).Item1);
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

            Assert.IsTrue(GJK2D.TestCollision(circle, transformA, square, transformB).Item1);
        }

        [Test]
        public void CirclePolygonNotOverlapping()
        {
            var circle = new Circle(1);
            var circleTransform = new Transform2D(new Vector2(5, 0));

            var square = new Polygon(new Position2D(0, 0),
                new Position2D(-1, -1), new Position2D(1, -1),
                new Position2D(1, 1), new Position2D(-1, 1)
            );
            var squareTransform = Transform2D.DefaultTransform;

            Assert.IsFalse(GJK2D.TestCollision(circle, circleTransform, square, squareTransform).Item1);
        }

        [Test]
        public void RotatedRectanglesOverlapping()
        {
            var rectangleA = new MoonTools.Core.Bonk.Rectangle(-1, -1, 2, 2);
            var transformA = new Transform2D(new Vector2(-1, 0), -90f);

            var rectangleB = new MoonTools.Core.Bonk.Rectangle(-1, -1, 1, 1);
            var transformB = new Transform2D(new Vector2(1, 0));

            Assert.IsTrue(GJK2D.TestCollision(rectangleA, transformA, rectangleB, transformB).Item1);
        }

        [Test]
        public void RectanglesTouching()
        {
            var rectangleA = new MoonTools.Core.Bonk.Rectangle(-1, -1, 1, 1);
            var transformA = new Transform2D(new Position2D(-1, 0));

            var rectangleB = new MoonTools.Core.Bonk.Rectangle(-1, -1, 1, 1);
            var transformB = new Transform2D(new Vector2(1, 0));

            Assert.IsTrue(GJK2D.TestCollision(rectangleA, transformA, rectangleB, transformB).Item1);
        }
    }
}
