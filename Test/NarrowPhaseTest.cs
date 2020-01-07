using NUnit.Framework;
using MoonTools.Core.Bonk;
using MoonTools.Core.Structs;
using System.Numerics;
using FluentAssertions;
using System.Collections.Immutable;

namespace Tests
{
    public class NarrowPhaseTest
    {
        [Test]
        public void PointLineOverlapping()
        {
            var point = new Point();
            var pointTransform = new Transform2D(new Position2D(1, 1));
            var line = new Line(new Position2D(-2, -2), new Position2D(2, 2));

            NarrowPhase.TestCollision(point, pointTransform, line, Transform2D.DefaultTransform).Should().BeTrue();
        }

        [Test]
        public void PointLineNotOverlapping()
        {
            var point = new Point();
            var pointTransform = new Transform2D(new Position2D(1, 1));
            var line = new Line(new Position2D(-3, -2), new Position2D(-9, -5));

            NarrowPhase.TestCollision(point, pointTransform, line, Transform2D.DefaultTransform).Should().BeFalse();
        }

        [Test]
        public void PointCircleOverlapping()
        {
            var point = new Point();
            var circle = new Circle(3);

            var pointTransform = new Transform2D(new Position2D(1, 1));
            var circleTransform = new Transform2D(new Position2D(-1, 0));

            NarrowPhase.TestCollision(point, pointTransform, circle, circleTransform).Should().BeTrue();
        }

        [Test]
        public void PointCircleNotOverlapping()
        {
            var point = new Point();
            var pointTransform = new Transform2D(new Position2D(3, 0));
            var circle = new Circle(1);

            NarrowPhase.TestCollision(point, pointTransform, circle, Transform2D.DefaultTransform).Should().BeFalse();
        }

        [Test]
        public void PointRectangleOverlapping()
        {
            var point = new Point();
            var rectangle = new Rectangle(-2, -2, 4, 4);

            NarrowPhase.TestCollision(point, Transform2D.DefaultTransform, rectangle, Transform2D.DefaultTransform).Should().BeTrue();
        }

        [Test]
        public void PointRectangleNotOverlapping()
        {
            var point = new Point();
            var pointTransform = new Transform2D(new Position2D(5, 5));
            var rectangle = new Rectangle(-2, -2, 4, 4);

            NarrowPhase.TestCollision(point, pointTransform, rectangle, Transform2D.DefaultTransform).Should().BeFalse();
        }

        [Test]
        public void PointPolygonOverlapping()
        {
            var point = new Point();
            var pointTransform = new Transform2D(new Position2D(1, 1));
            var polygon = new Polygon(ImmutableArray.Create(
                new Position2D(-2, -2),
                new Position2D(-3, 2),
                new Position2D(3, 2),
                new Position2D(3, -2)
            ));

            NarrowPhase.TestCollision(point, pointTransform, polygon, Transform2D.DefaultTransform).Should().BeTrue();
        }

        [Test]
        public void PointPolygonNotOverlapping()
        {
            var point = new Point();
            var pointTransform = new Transform2D(new Position2D(5, 5));
            var polygon = new Polygon(ImmutableArray.Create(
                new Position2D(-2, -2),
                new Position2D(-3, 2),
                new Position2D(3, 2),
                new Position2D(3, -2)
            ));

            NarrowPhase.TestCollision(point, pointTransform, polygon, Transform2D.DefaultTransform).Should().BeFalse();
        }

        [Test]
        public void LineLineOverlapping()
        {
            var lineA = new Line(new Position2D(-1, -1), new Position2D(1, 1));
            var lineB = new Line(new Position2D(-1, 1), new Position2D(1, -1));

            NarrowPhase.TestCollision(lineA, Transform2D.DefaultTransform, lineB, Transform2D.DefaultTransform).Should().BeTrue();
        }

        [Test]
        public void ScaledLinesOverlapping()
        {
            var lineA = new Line(new Position2D(-1, -1), new Position2D(1, 1));
            var lineB = new Line(new Position2D(-1, 1), new Position2D(1, -1));

            var transform = new Transform2D(new Position2D(0, 0), 0f, new Vector2(2, 2));

            NarrowPhase.TestCollision(lineA, transform, lineB, transform).Should().BeTrue();
        }

        [Test]
        public void LineLineNotOverlapping()
        {
            var lineA = new Line(new Position2D(0, 1), new Position2D(1, 0));
            var lineB = new Line(new Position2D(-1, -1), new Position2D(-2, -2));

            NarrowPhase.TestCollision(lineA, Transform2D.DefaultTransform, lineB, Transform2D.DefaultTransform).Should().BeFalse();
        }

        [Test]
        public void ScaledLinesNotOverlapping()
        {
            var lineA = new Line(new Position2D(0, 1), new Position2D(1, 0));
            var lineB = new Line(new Position2D(-1, -1), new Position2D(-2, -2));

            var transform = new Transform2D(new Position2D(0, 0), 0f, new Vector2(2, 2));

            NarrowPhase.TestCollision(lineA, transform, lineB, transform).Should().BeFalse();
        }

        [Test]
        public void CircleCircleOverlapping()
        {
            var circleA = new Circle(2);
            var transformA = new Transform2D(new Vector2(-1, -1));
            var circleB = new Circle(2);
            var transformB = new Transform2D(new Vector2(1, 1));

            NarrowPhase.TestCollision(circleA, transformA, circleB, transformB).Should().BeTrue();
        }

        [Test]
        public void ScaledCirclesOverlapping()
        {
            var circleA = new Circle(2);
            var transformA = new Transform2D(new Vector2(-3, 0), 0f, new Vector2(2, 2));
            var circleB = new Circle(2);
            var transformB = new Transform2D(new Vector2(3, 0), 0f, new Vector2(2, 2));

            NarrowPhase.TestCollision(circleA, transformA, circleB, transformB).Should().BeTrue();
        }

        [Test]
        public void CircleCircleNotOverlapping()
        {
            var circleA = new Circle(2);
            var transformA = new Transform2D(new Vector2(-5, -5));
            var circleB = new Circle(2);
            var transformB = new Transform2D(new Vector2(5, 5));

            NarrowPhase.TestCollision(circleA, transformA, circleB, transformB).Should().BeFalse();
        }

        [Test]
        public void ScaledCirclesNotOverlapping()
        {
            var circleA = new Circle(2);
            var transformA = new Transform2D(new Vector2(-5, -5), 0, new Vector2(0.2f, 0.2f));
            var circleB = new Circle(2);
            var transformB = new Transform2D(new Vector2(5, 5), 0, new Vector2(0.2f, 0.2f));

            NarrowPhase.TestCollision(circleA, transformA, circleB, transformB).Should().BeFalse();
        }

        [Test]
        public void PolygonPolygonOverlapping()
        {
            var shapeA = new Polygon(ImmutableArray.Create(
                new Position2D(-1, 1), new Position2D(1, 1),
                new Position2D(-1, -1), new Position2D(1, -1)
            ));

            var transformA = Transform2D.DefaultTransform;

            var shapeB = new Polygon(ImmutableArray.Create(
                new Position2D(-1, 1), new Position2D(1, 1),
                new Position2D(-1, -1), new Position2D(1, -1)
            ));

            var transformB = new Transform2D(new Vector2(0.5f, 0.5f));

            NarrowPhase.TestCollision(shapeA, transformA, shapeB, transformB).Should().BeTrue();
        }

        [Test]
        public void ScaledPolygonsOverlapping()
        {
            var shapeA = new Polygon(ImmutableArray.Create(
                new Position2D(-1, 1), new Position2D(1, 1),
                new Position2D(-1, -1), new Position2D(1, -1)
            ));

            var transformA = Transform2D.DefaultTransform;

            var shapeB = new Polygon(ImmutableArray.Create(
                new Position2D(-1, 1), new Position2D(1, 1),
                new Position2D(-1, -1), new Position2D(1, -1)
            ));

            var transformB = new Transform2D(new Vector2(3f, 0f), 0f, new Vector2(3f, 3f));

            NarrowPhase.TestCollision(shapeA, transformA, shapeB, transformB).Should().BeTrue();
        }

        [Test]
        public void PolygonPolygonNotOverlapping()
        {
            var shapeA = new Polygon(ImmutableArray.Create(
                new Position2D(-1, 1), new Position2D(1, 1),
                new Position2D(-1, -1), new Position2D(1, -1)
            ));

            var transformA = Transform2D.DefaultTransform;

            var shapeB = new Polygon(ImmutableArray.Create(
                new Position2D(-1, 1), new Position2D(1, 1),
                new Position2D(-1, -1), new Position2D(1, -1)
            ));

            var transformB = new Transform2D(new Vector2(5, 0));

            NarrowPhase.TestCollision(shapeA, transformA, shapeB, transformB).Should().BeFalse();
        }

        [Test]
        public void ScaledPolygonsNotOverlapping()
        {
            var shapeA = new Polygon(ImmutableArray.Create(
                new Position2D(-1, 1), new Position2D(1, 1),
                new Position2D(-1, -1), new Position2D(1, -1)
            ));

            var transformA = Transform2D.DefaultTransform;

            var shapeB = new Polygon(ImmutableArray.Create(
                new Position2D(-2, 2), new Position2D(2, 2),
                new Position2D(-2, -2), new Position2D(2, -2)
            ));

            var transformB = new Transform2D(new Vector2(3f, 0), 0f, new Vector2(0.5f, 0.5f));

            NarrowPhase.TestCollision(shapeA, transformA, shapeB, transformB).Should().BeFalse();
        }

        [Test]
        public void LinePolygonOverlapping()
        {
            var line = new Line(new Position2D(-1, -1), new Position2D(1, 1));

            var transformA = Transform2D.DefaultTransform;

            var polygon = new Polygon(ImmutableArray.Create(
                new Position2D(-1, -1), new Position2D(1, -1),
                new Position2D(1, 1), new Position2D(-1, 1)
            ));

            var transformB = Transform2D.DefaultTransform;

            NarrowPhase.TestCollision(line, transformA, polygon, transformB).Should().BeTrue();
        }

        [Test]
        public void LinePolygonNotOverlapping()
        {
            var line = new Line(new Position2D(-5, 5), new Position2D(-5, 5));

            var transformA = Transform2D.DefaultTransform;

            var polygon = new Polygon(ImmutableArray.Create(
                new Position2D(-1, -1), new Position2D(1, -1),
                new Position2D(1, 1), new Position2D(-1, 1)
            ));

            var transformB = Transform2D.DefaultTransform;

            NarrowPhase.TestCollision(line, transformA, polygon, transformB).Should().BeFalse();
        }

        [Test]
        public void LineCircleOverlapping()
        {
            var line = new Line(new Position2D(-1, -1), new Position2D(1, 1));
            var transformA = Transform2D.DefaultTransform;
            var circle = new Circle(1);
            var transformB = Transform2D.DefaultTransform;

            NarrowPhase.TestCollision(line, transformA, circle, transformB).Should().BeTrue();
        }

        [Test]
        public void LineCircleNotOverlapping()
        {
            var line = new Line(new Position2D(-5, -5), new Position2D(-4, -4));
            var transformA = Transform2D.DefaultTransform;
            var circle = new Circle(1);
            var transformB = Transform2D.DefaultTransform;

            NarrowPhase.TestCollision(line, transformA, circle, transformB).Should().BeFalse();
        }

        [Test]
        public void CirclePolygonOverlapping()
        {
            var circle = new Circle(1);
            var transformA = new Transform2D(new Vector2(0.25f, 0));

            var square = new Polygon(ImmutableArray.Create(
                new Position2D(-1, -1), new Position2D(1, -1),
                new Position2D(1, 1), new Position2D(-1, 1)
            ));

            var transformB = Transform2D.DefaultTransform;

            NarrowPhase.TestCollision(circle, transformA, square, transformB).Should().BeTrue();
        }

        [Test]
        public void CirclePolygonNotOverlapping()
        {
            var circle = new Circle(1);
            var circleTransform = new Transform2D(new Vector2(5, 0));

            var square = new Polygon(ImmutableArray.Create(
                new Position2D(-1, -1), new Position2D(1, -1),
                new Position2D(1, 1), new Position2D(-1, 1)
            ));
            var squareTransform = Transform2D.DefaultTransform;

            NarrowPhase.TestCollision(circle, circleTransform, square, squareTransform).Should().BeFalse();
        }

        [Test]
        public void RectanglesNotOverlapping()
        {
            var rectangleA = new Rectangle(-6, -6, 12, 12);
            var transformA = new Transform2D(new Position2D(39, 249));

            var rectangleB = new Rectangle(-8, -8, 16, 16);
            var transformB = new Transform2D(new Position2D(16, 240));

            NarrowPhase.TestCollision(rectangleA, transformA, rectangleB, transformB).Should().BeFalse();
        }

        [Test]
        public void RotatedRectanglesOverlapping()
        {
            var rectangleA = new Rectangle(-1, -3, 3, 6);
            var transformA = new Transform2D(new Vector2(4f, 0), (float)System.Math.PI / 2);

            var rectangleB = new Rectangle(-1, -1, 2, 2);
            var transformB = new Transform2D(new Vector2(0, 0));

            NarrowPhase.TestCollision(rectangleA, transformA, rectangleB, transformB).Should().BeTrue();
        }

        [Test]
        public void RectanglesTouchingGJK2D()
        {
            var rectangleA = new Rectangle(-1, -1, 2, 2);
            var transformA = new Transform2D(new Position2D(-1, 0));

            var rectangleB = new Rectangle(-1, -1, 2, 2);
            var transformB = new Transform2D(new Vector2(1, 0));

            NarrowPhase.TestCollision(rectangleA, transformA, rectangleB, transformB).Should().BeTrue();
        }

        [Test]
        public void RectanglesOverlappingGJK2D()
        {
            var rectangleA = new Rectangle(-1, -1, 2, 2);
            var transformA = new Transform2D(new Position2D(0, 0));

            var rectangleB = new Rectangle(-1, -1, 2, 2);
            var transformB = new Transform2D(new Vector2(1, 0));

            NarrowPhase.TestCollision(rectangleA, transformA, rectangleB, transformB).Should().BeTrue();
        }

        [Test]
        public void RectanglesTouchingOverlap()
        {
            var rectangleA = new Rectangle(-1, -1, 2, 2);
            var transformA = new Transform2D(new Position2D(-1, 0));

            var rectangleB = new Rectangle(-1, -1, 2, 2);
            var transformB = new Transform2D(new Vector2(1, 0));

            NarrowPhase.TestRectangleOverlap(rectangleA, transformA, rectangleB, transformB).Should().BeTrue();
        }

        [Test]
        public void RectanglesOverlappingOverlap()
        {
            var rectangleA = new Rectangle(-1, -1, 2, 2);
            var transformA = new Transform2D(new Position2D(0, 0));

            var rectangleB = new Rectangle(-1, -1, 2, 2);
            var transformB = new Transform2D(new Vector2(1, 0), 0, new Vector2(-1, 1));

            NarrowPhase.TestRectangleOverlap(rectangleA, transformA, rectangleB, transformB).Should().BeTrue();
        }

        [Test]
        public void MultiRectanglesOverlapping()
        {
            var multiRectangleA = new MultiShape(
                ImmutableArray.Create<(IShape2D, Transform2D)>(
                    (new Rectangle(-2, 0, 4, 1), new Transform2D(new Position2D(-5, 0))),
                    (new Rectangle(-2, 0, 4, 1), new Transform2D(new Position2D(-5, 1))),
                    (new Rectangle(-2, 0, 4, 1), new Transform2D(new Position2D(-5, 2)))
                )
            );
            var transformA = new Transform2D(new Position2D(5, 0));

            var multiRectangleB = new MultiShape(
                ImmutableArray.Create<(IShape2D, Transform2D)>(
                    (new Rectangle(-2, 0, 4, 1), new Transform2D(new Position2D(4, -1))),
                    (new Rectangle(-2, 0, 4, 1), new Transform2D(new Position2D(4, 0))),
                    (new Rectangle(-2, 0, 4, 1), new Transform2D(new Position2D(4, 1)))
                )
            );
            var transformB = new Transform2D(new Position2D(0, 3));

            NarrowPhase.TestCollision(multiRectangleA, transformA, multiRectangleB, transformB).Should().BeTrue();
        }

        [Test]
        public void MultiRectanglesNotOverlapping()
        {
            var multiRectangleA = new MultiShape(
                ImmutableArray.Create<(IShape2D, Transform2D)>(
                    (new Rectangle(-2, 0, 4, 1), new Transform2D(new Position2D(-5, 0))),
                    (new Rectangle(-2, 0, 4, 1), new Transform2D(new Position2D(-5, 1))),
                    (new Rectangle(-2, 0, 4, 1), new Transform2D(new Position2D(-5, 2)))
                )
            );
            var transformA = new Transform2D(new Position2D(5, 0));

            var multiRectangleB = new MultiShape(
                ImmutableArray.Create<(IShape2D, Transform2D)>(
                    (new Rectangle(-2, 0, 4, 1), new Transform2D(new Position2D(4, -1))),
                    (new Rectangle(-2, 0, 4, 1), new Transform2D(new Position2D(4, 0))),
                    (new Rectangle(-2, 0, 4, 1), new Transform2D(new Position2D(4, 1)))
                )
            );
            var transformB = new Transform2D(new Position2D(0, -3));

            NarrowPhase.TestCollision(multiRectangleA, transformA, multiRectangleB, transformB).Should().BeFalse();
        }
    }
}
