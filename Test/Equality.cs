using NUnit.Framework;
using FluentAssertions;

using MoonTools.Core.Bonk;
using MoonTools.Core.Structs;
using System.Numerics;
using System.Collections.Immutable;

namespace Tests
{
    public static class EqualityTests
    {
        public class PointTests
        {
            [Test]
            public void PointEqual()
            {
                var a = new Point();
                var b = new Point();

                a.Equals(b).Should().BeTrue();
            }

            [Test]
            public void PointEqualOperator()
            {
                var a = new Point();
                var b = new Point();
                (a == b).Should().BeTrue();
            }

            [Test]
            public void PointNotEqualOperator()
            {
                var a = new Point();
                var b = new Point();

                (a != b).Should().BeFalse();
            }
        }

        public class CircleTests
        {
            [Test]
            public void CircleEqual()
            {
                var a = new Circle(2);
                var b = new Circle(2);

                (a.Equals(b)).Should().BeTrue();
            }

            [Test]
            public void CircleNotEqual()
            {
                var a = new Circle(2);
                var b = new Circle(3);

                (a.Equals(b)).Should().BeFalse();
            }

            [Test]
            public void CircleEqualOperator()
            {
                var a = new Circle(2);
                var b = new Circle(2);

                (a == b).Should().BeTrue();
            }

            [Test]
            public void CircleNotEqualOperator()
            {
                var a = new Circle(2);
                var b = new Circle(3);

                (a != b).Should().BeTrue();
            }
        }

        public class LineTests
        {
            [Test]
            public void LineEqual()
            {
                var a = new Line(new Position2D(0, 2), new Position2D(2, 4));
                var b = new Line(new Position2D(0, 2), new Position2D(2, 4));

                a.Equals(b).Should().BeTrue();
            }

            [Test]
            public void LineEqualOperator()
            {
                var a = new Line(new Position2D(0, 2), new Position2D(2, 4));
                var b = new Line(new Position2D(0, 2), new Position2D(2, 4));

                (a == b).Should().BeTrue();
            }

            [Test]
            public void LineNotEqual()
            {
                var a = new Line(new Position2D(-2, 4), new Position2D(2, 4));
                var b = new Line(new Position2D(0, 3), new Position2D(5, 1));

                a.Equals(b).Should().BeFalse();
            }

            [Test]
            public void LineNotEqualOperator()
            {
                var a = new Line(new Position2D(-2, 4), new Position2D(2, 4));
                var b = new Line(new Position2D(0, 3), new Position2D(5, 1));

                (a != b).Should().BeTrue();
            }

            [Test]
            public void LineReversedEqual()
            {
                var a = new Line(new Position2D(0, 2), new Position2D(2, 4));
                var b = new Line(new Position2D(2, 4), new Position2D(0, 2));

                a.Equals(b).Should().BeTrue();
            }

            [Test]
            public void LineReversedEqualOperator()
            {
                var a = new Line(new Position2D(0, 2), new Position2D(2, 4));
                var b = new Line(new Position2D(2, 4), new Position2D(0, 2));

                (a == b).Should().BeTrue();
            }
        }

        public class RectangleTests
        {
            [Test]
            public void RectangleEqual()
            {
                var a = new Rectangle(0, 0, 3, 3);
                var b = new Rectangle(0, 0, 3, 3);

                a.Equals(b).Should().BeTrue();
            }

            [Test]
            public void RectangleEqualOperator()
            {
                var a = new Rectangle(0, 0, 3, 3);
                var b = new Rectangle(0, 0, 3, 3);

                (a == b).Should().BeTrue();
            }

            [Test]
            public void RectangleNotEqual()
            {
                var a = new Rectangle(0, 0, 3, 3);
                var b = new Rectangle(-1, -1, 5, 5);

                a.Equals(b).Should().BeFalse();
            }

            [Test]
            public void RectangleNotEqualOperator()
            {
                var a = new Rectangle(0, 0, 3, 3);
                var b = new Rectangle(-1, -1, 5, 5);

                (a != b).Should().BeTrue();
            }
        }

        public class PolygonTests
        {
            [Test]
            public void PolygonEqual()
            {
                var a = new Polygon(ImmutableArray.Create(
                    new Position2D(0, 1),
                    new Position2D(1, 2),
                    new Position2D(-1, -1)
                ));

                var b = new Polygon(ImmutableArray.Create(
                    new Position2D(0, 1),
                    new Position2D(1, 2),
                    new Position2D(-1, -1)
                ));

                a.Equals(b).Should().BeTrue();
            }

            [Test]
            public void PolygonEqualOperator()
            {
                var a = new Polygon(ImmutableArray.Create(
                    new Position2D(0, 1),
                    new Position2D(1, 2),
                    new Position2D(-1, -1)
                ));

                var b = new Polygon(ImmutableArray.Create(
                    new Position2D(0, 1),
                    new Position2D(1, 2),
                    new Position2D(-1, -1)
                ));

                (a == b).Should().BeTrue();
            }

            [Test]
            public void PolygonDifferentOrderEqual()
            {
                var a = new Polygon(ImmutableArray.Create(
                    new Position2D(0, 1),
                    new Position2D(1, 2),
                    new Position2D(-1, -1)
                ));

                var b = new Polygon(ImmutableArray.Create(
                    new Position2D(1, 2),
                    new Position2D(-1, -1),
                    new Position2D(0, 1)
                ));

                a.Equals(b).Should().BeTrue();
            }

            [Test]
            public void PolygonDifferentOrderEqualOperator()
            {
                var a = new Polygon(ImmutableArray.Create(
                    new Position2D(0, 1),
                    new Position2D(1, 2),
                    new Position2D(-1, -1)
                ));

                var b = new Polygon(ImmutableArray.Create(
                    new Position2D(1, 2),
                    new Position2D(-1, -1),
                    new Position2D(0, 1)
                ));

                (a == b).Should().BeTrue();
            }

            [Test]
            public void PolygonNotEqual()
            {
                var a = new Polygon(ImmutableArray.Create(
                    new Position2D(0, 1),
                    new Position2D(1, 2),
                    new Position2D(-1, -1)
                ));

                var b = new Polygon(ImmutableArray.Create(
                    new Position2D(1, 0),
                    new Position2D(2, 1),
                    new Position2D(-1, -1)
                ));

                a.Equals(b).Should().BeFalse();
            }

            [Test]
            public void PolygonNotEqualOperator()
            {
                var a = new Polygon(ImmutableArray.Create(
                    new Position2D(0, 1),
                    new Position2D(1, 2),
                    new Position2D(-1, -1)
                ));

                var b = new Polygon(ImmutableArray.Create(
                    new Position2D(1, 0),
                    new Position2D(2, 1),
                    new Position2D(-1, -1)
                ));

                (a != b).Should().BeTrue();
            }

            [Test]
            public void PolygonRectangleEqual()
            {
                var a = new Polygon(ImmutableArray.Create(
                    new Position2D(-1, -1),
                    new Position2D(1, -1),
                    new Position2D(1, 1),
                    new Position2D(-1, 1)
                ));

                var b = new Rectangle(-1, -1, 1, 1);

                a.Equals(b).Should().BeTrue();
                b.Equals(a).Should().BeTrue();
            }

            [Test]
            public void PolygonRectangleNotEqual()
            {
                var a = new Polygon(ImmutableArray.Create(
                    new Position2D(-2, -1),
                    new Position2D(1, -1),
                    new Position2D(1, 1),
                    new Position2D(-2, 1)
                ));

                var b = new Rectangle(-1, -1, 1, 1);

                a.Equals(b).Should().BeFalse();
                b.Equals(a).Should().BeFalse();
            }

            [Test]
            public void PolygonRectangleEqualOperator()
            {
                var a = new Polygon(ImmutableArray.Create(
                    new Position2D(-1, -1),
                    new Position2D(1, -1),
                    new Position2D(1, 1),
                    new Position2D(-1, 1)
                ));

                var b = new Rectangle(-1, -1, 1, 1);

                (a == b).Should().BeTrue();
                (b == a).Should().BeTrue();
            }

            [Test]
            public void PolygonRectangleNotEqualOperator()
            {
                var a = new Polygon(ImmutableArray.Create(
                    new Position2D(2, 1),
                    new Position2D(1, -1),
                    new Position2D(-1, -1),
                    new Position2D(-2, 1)
                ));

                var b = new Rectangle(-1, -1, 1, 1);

                (a != b).Should().BeTrue();
                (b != a).Should().BeTrue();
            }
        }

        public class SimplexTests
        {
            [Test]
            public void ZeroSimplexEquals()
            {
                var simplexA = new Simplex2D(Vector2.One);
                var simplexB = new Simplex2D(Vector2.One);

                simplexA.Equals(simplexB).Should().BeTrue();
            }

            [Test]
            public void ZeroSimplexEqualsOperator()
            {
                var simplexA = new Simplex2D(Vector2.One);
                var simplexB = new Simplex2D(Vector2.One);

                (simplexA == simplexB).Should().BeTrue();
            }

            [Test]
            public void ZeroSimplexNotEquals()
            {
                var simplexA = new Simplex2D(Vector2.Zero);
                var simplexB = new Simplex2D(Vector2.One);

                simplexA.Equals(simplexB).Should().BeFalse();

                var simplexC = new Simplex2D(Vector2.Zero, Vector2.One);

                simplexA.Equals(simplexC).Should().BeFalse();
            }

            [Test]
            public void ZeroSimplexNotEqualsOperator()
            {
                var simplexA = new Simplex2D(Vector2.Zero);
                var simplexB = new Simplex2D(Vector2.One);

                (simplexA != simplexB).Should().BeTrue();
            }

            [Test]
            public void OneSimplexEquals()
            {
                var simplexA = new Simplex2D(Vector2.One, Vector2.Zero);
                var simplexB = new Simplex2D(Vector2.One, Vector2.Zero);

                simplexA.Equals(simplexB).Should().BeTrue();

                var simplexC = new Simplex2D(Vector2.One, Vector2.Zero);
                var simplexD = new Simplex2D(Vector2.Zero, Vector2.One);

                simplexC.Equals(simplexD).Should().BeTrue();
            }

            [Test]
            public void OneSimplexEqualsOperator()
            {
                var simplexA = new Simplex2D(Vector2.One, Vector2.Zero);
                var simplexB = new Simplex2D(Vector2.One, Vector2.Zero);

                (simplexA == simplexB).Should().BeTrue();

                var simplexC = new Simplex2D(Vector2.One, Vector2.Zero);
                var simplexD = new Simplex2D(Vector2.Zero, Vector2.One);

                (simplexC == simplexD).Should().BeTrue();
            }

            [Test]
            public void OneSimplexNotEquals()
            {
                var simplexA = new Simplex2D(Vector2.One, Vector2.Zero);
                var simplexB = new Simplex2D(Vector2.One, Vector2.UnitX);

                simplexA.Equals(simplexB).Should().BeFalse();

                var simplexC = new Simplex2D(Vector2.One, Vector2.Zero);
                var simplexD = new Simplex2D(Vector2.Zero, Vector2.UnitX);

                simplexC.Equals(simplexD).Should().BeFalse();

                var simplexE = new Simplex2D(Vector2.Zero);

                simplexA.Equals(simplexE).Should().BeFalse();
            }

            [Test]
            public void OneSimplexNotEqualsOperator()
            {
                var simplexA = new Simplex2D(Vector2.One, Vector2.Zero);
                var simplexB = new Simplex2D(Vector2.One, Vector2.UnitX);

                (simplexA == simplexB).Should().BeFalse();

                var simplexC = new Simplex2D(Vector2.One, Vector2.Zero);
                var simplexD = new Simplex2D(Vector2.Zero, Vector2.UnitX);

                (simplexC == simplexD).Should().BeFalse();
            }

            [Test]
            public void TwoSimplexEquals()
            {
                var simplexA = new Simplex2D(Vector2.One, Vector2.Zero, Vector2.UnitX);

                var simplexB = new Simplex2D(Vector2.One, Vector2.Zero, Vector2.UnitX);

                simplexA.Equals(simplexB).Should().BeTrue();

                var simplexC = new Simplex2D(Vector2.Zero, Vector2.One, Vector2.UnitX);

                simplexA.Equals(simplexC).Should().BeTrue();

                var simplexD = new Simplex2D(Vector2.UnitX, Vector2.Zero, Vector2.One);

                simplexA.Equals(simplexD).Should().BeTrue();

                var simplexE = new Simplex2D(Vector2.One, Vector2.UnitX, Vector2.Zero);

                simplexA.Equals(simplexE).Should().BeTrue();

                var simplexF = new Simplex2D(Vector2.Zero, Vector2.UnitX, Vector2.One);

                simplexA.Equals(simplexF).Should().BeTrue();
            }

            [Test]
            public void TwoSimplexEqualsOperator()
            {
                var simplexA = new Simplex2D(Vector2.One, Vector2.Zero, Vector2.UnitX);
                var simplexB = new Simplex2D(Vector2.One, Vector2.Zero, Vector2.UnitX);

                (simplexA == simplexB).Should().BeTrue();

                var simplexC = new Simplex2D(Vector2.One, Vector2.Zero, Vector2.UnitX);
                var simplexD = new Simplex2D(Vector2.Zero, Vector2.One, Vector2.UnitX);

                (simplexC == simplexD).Should().BeTrue();
            }

            [Test]
            public void TwoSimplexNotEquals()
            {
                var simplexA = new Simplex2D(Vector2.One, Vector2.UnitY, Vector2.UnitX);
                var simplexB = new Simplex2D(Vector2.One, Vector2.Zero, Vector2.UnitX);

                simplexA.Equals(simplexB).Should().BeFalse();

                var simplexC = new Simplex2D(Vector2.One, Vector2.Zero, Vector2.UnitX);
                var simplexD = new Simplex2D(Vector2.Zero, Vector2.UnitY, Vector2.UnitX);

                simplexC.Equals(simplexD).Should().BeFalse();

                var simplexE = new Simplex2D(Vector2.Zero);

                simplexA.Equals(simplexE).Should().BeFalse();
            }

            [Test]
            public void TwoSimplexNotEqualsOperator()
            {
                var simplexA = new Simplex2D(Vector2.One, Vector2.UnitY, Vector2.UnitX);
                var simplexB = new Simplex2D(Vector2.One, Vector2.Zero, Vector2.UnitX);

                (simplexA == simplexB).Should().BeFalse();

                var simplexC = new Simplex2D(Vector2.One, Vector2.Zero, Vector2.UnitX);
                var simplexD = new Simplex2D(Vector2.Zero, Vector2.UnitY, Vector2.UnitX);

                (simplexC == simplexD).Should().BeFalse();
            }
        }

        public class AABBTests
        {
            [Test]
            public void Equal()
            {
                var aabb = new AABB(0, 0, 3, 3);
                var other = new AABB(0, 0, 3, 3);

                (aabb == other).Should().BeTrue();
            }

            [Test]
            public void NotEqual()
            {
                var aabb = new AABB(0, 0, 3, 3);
                var other = new AABB(0, 0, 6, 6);

                (aabb != other).Should().BeTrue();
            }
        }
    }
}
