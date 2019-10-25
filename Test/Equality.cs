using NUnit.Framework;
using FluentAssertions;

using MoonTools.Core.Bonk;
using MoonTools.Core.Structs;
using Microsoft.Xna.Framework;

namespace Tests
{
    public class EqualityTests
    {
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
                var a = new MoonTools.Core.Bonk.Rectangle(0, 0, 3, 3);
                var b = new MoonTools.Core.Bonk.Rectangle(0, 0, 3, 3);

                a.Equals(b).Should().BeTrue();
            }

            [Test]
            public void RectangleEqualOperator()
            {
                var a = new MoonTools.Core.Bonk.Rectangle(0, 0, 3, 3);
                var b = new MoonTools.Core.Bonk.Rectangle(0, 0, 3, 3);

                (a == b).Should().BeTrue();
            }

            [Test]
            public void RectangleNotEqual()
            {
                var a = new MoonTools.Core.Bonk.Rectangle(0, 0, 3, 3);
                var b = new MoonTools.Core.Bonk.Rectangle(-1, -1, 5, 5);

                a.Equals(b).Should().BeFalse();
            }

            [Test]
            public void RectangleNotEqualOperator()
            {
                var a = new MoonTools.Core.Bonk.Rectangle(0, 0, 3, 3);
                var b = new MoonTools.Core.Bonk.Rectangle(-1, -1, 5, 5);

                (a != b).Should().BeTrue();
            }
        }

        public class PolygonTests
        {
            [Test]
            public void PolygonEqual()
            {
                var a = new Polygon(
                    new Position2D(0, 1),
                    new Position2D(1, 2),
                    new Position2D(-1, -1)
                );

                var b = new Polygon(
                    new Position2D(0, 1),
                    new Position2D(1, 2),
                    new Position2D(-1, -1)
                );

                a.Equals(b).Should().BeTrue();
            }

            [Test]
            public void PolygonEqualOperator()
            {
                var a = new Polygon(
                    new Position2D(0, 1),
                    new Position2D(1, 2),
                    new Position2D(-1, -1)
                );

                var b = new Polygon(
                    new Position2D(0, 1),
                    new Position2D(1, 2),
                    new Position2D(-1, -1)
                );

                (a == b).Should().BeTrue();
            }

            [Test]
            public void PolygonDifferentOrderEqual()
            {
                var a = new Polygon(
                    new Position2D(0, 1),
                    new Position2D(1, 2),
                    new Position2D(-1, -1)
                );

                var b = new Polygon(
                    new Position2D(1, 2),
                    new Position2D(-1, -1),
                    new Position2D(0, 1)
                );

                a.Equals(b).Should().BeTrue();
            }

            [Test]
            public void PolygonDifferentOrderEqualOperator()
            {
                var a = new Polygon(
                    new Position2D(0, 1),
                    new Position2D(1, 2),
                    new Position2D(-1, -1)
                );

                var b = new Polygon(
                    new Position2D(1, 2),
                    new Position2D(-1, -1),
                    new Position2D(0, 1)
                );

                (a == b).Should().BeTrue();
            }

            [Test]
            public void PolygonNotEqual()
            {
                var a = new Polygon(
                    new Position2D(0, 1),
                    new Position2D(1, 2),
                    new Position2D(-1, -1)
                );

                var b = new Polygon(
                    new Position2D(1, 0),
                    new Position2D(2, 1),
                    new Position2D(-1, -1)
                );

                a.Equals(b).Should().BeFalse();
            }

            [Test]
            public void PolygonNotEqualOperator()
            {
                var a = new Polygon(
                    new Position2D(0, 1),
                    new Position2D(1, 2),
                    new Position2D(-1, -1)
                );

                var b = new Polygon(
                    new Position2D(1, 0),
                    new Position2D(2, 1),
                    new Position2D(-1, -1)
                );

                (a != b).Should().BeTrue();
            }
        }

        public class SimplexTests
        {
            [Test]
            public void SimplexEquals()
            {
                var shapeA = new Circle(3);
                var transformA = new Transform2D(new Position2D(1, 2));

                var shapeB = new Circle(2);
                var transformB = new Transform2D(new Position2D(4, 5));

                var minkowskiDifference = new MinkowskiDifference(shapeA, transformA, shapeB, transformB);

                var directionA = Vector2.UnitX;
                var directionB = Vector2.UnitY;

                var simplexA = new Simplex(minkowskiDifference, directionA, directionB);
                var simplexB = new Simplex(minkowskiDifference, directionA, directionB);

                simplexA.Equals(simplexB).Should().BeTrue();
            }

            [Test]
            public void SimplexEqualsOperator()
            {
                var shapeA = new Circle(3);
                var transformA = new Transform2D(new Position2D(1, 2));

                var shapeB = new Circle(2);
                var transformB = new Transform2D(new Position2D(4, 5));

                var minkowskiDifference = new MinkowskiDifference(shapeA, transformA, shapeB, transformB);

                var directionA = Vector2.UnitX;
                var directionB = Vector2.UnitY;

                var simplexA = new Simplex(minkowskiDifference, directionA, directionB);
                var simplexB = new Simplex(minkowskiDifference, directionA, directionB);

                (simplexA == simplexB).Should().BeTrue();
            }

            [Test]
            public void SimplexDirectionOutOfOrderEqual()
            {
                var shapeA = new Circle(3);
                var transformA = new Transform2D(new Position2D(1, 2));

                var shapeB = new Circle(2);
                var transformB = new Transform2D(new Position2D(4, 5));

                var minkowskiDifference = new MinkowskiDifference(shapeA, transformA, shapeB, transformB);

                var directionA = Vector2.UnitX;
                var directionB = Vector2.UnitY;

                var simplexA = new Simplex(minkowskiDifference, directionA, directionB);
                var simplexB = new Simplex(minkowskiDifference, directionB, directionA);

                simplexA.Equals(simplexB).Should().BeTrue();
            }

            [Test]
            public void SimplexDirectionOutOfOrderEqualOperator()
            {
                var shapeA = new Circle(3);
                var transformA = new Transform2D(new Position2D(1, 2));

                var shapeB = new Circle(2);
                var transformB = new Transform2D(new Position2D(4, 5));

                var minkowskiDifference = new MinkowskiDifference(shapeA, transformA, shapeB, transformB);

                var directionA = Vector2.UnitX;
                var directionB = Vector2.UnitY;

                var simplexA = new Simplex(minkowskiDifference, directionA, directionB);
                var simplexB = new Simplex(minkowskiDifference, directionB, directionA);

                (simplexA == simplexB).Should().BeTrue();
            }

            [Test]
            public void SimplexMinkowskiNotEqual()
            {
                var shapeA = new Circle(3);
                var transformA = new Transform2D(new Position2D(1, 2));

                var shapeB = new Circle(2);
                var transformB = new Transform2D(new Position2D(4, 5));

                var minkowskiDifferenceA = new MinkowskiDifference(shapeA, transformA, shapeB, transformB);
                var minkowskiDifferenceB = new MinkowskiDifference(shapeB, transformB, shapeA, transformA);

                var directionA = Vector2.UnitX;
                var directionB = Vector2.UnitY;

                var simplexA = new Simplex(minkowskiDifferenceA, directionA, directionB);
                var simplexB = new Simplex(minkowskiDifferenceB, directionA, directionB);

                simplexA.Equals(simplexB).Should().BeFalse();
            }

            [Test]
            public void SimplexMinkowskiNotEqualOperator()
            {
                var shapeA = new Circle(3);
                var transformA = new Transform2D(new Position2D(1, 2));

                var shapeB = new Circle(2);
                var transformB = new Transform2D(new Position2D(4, 5));

                var minkowskiDifferenceA = new MinkowskiDifference(shapeA, transformA, shapeB, transformB);
                var minkowskiDifferenceB = new MinkowskiDifference(shapeB, transformB, shapeA, transformA);

                var directionA = Vector2.UnitX;
                var directionB = Vector2.UnitY;

                var simplexA = new Simplex(minkowskiDifferenceA, directionA, directionB);
                var simplexB = new Simplex(minkowskiDifferenceB, directionA, directionB);

                (simplexA != simplexB).Should().BeTrue();
            }

            [Test]
            public void SimplexDirectionsNotEqual()
            {
                var shapeA = new Circle(3);
                var transformA = new Transform2D(new Position2D(1, 2));

                var shapeB = new Circle(2);
                var transformB = new Transform2D(new Position2D(4, 5));

                var minkowskiDifference = new MinkowskiDifference(shapeA, transformA, shapeB, transformB);

                var directionA = Vector2.UnitX;
                var directionB = Vector2.UnitY;
                var directionC = -Vector2.UnitX;
                var directionD = -Vector2.UnitY;

                var simplexA = new Simplex(minkowskiDifference, directionA, directionB);
                var simplexB = new Simplex(minkowskiDifference, directionC, directionD);

                simplexA.Equals(simplexB).Should().BeFalse();
            }

            [Test]
            public void SimplexDirectionsNotEqualOperator()
            {
                var shapeA = new Circle(3);
                var transformA = new Transform2D(new Position2D(1, 2));

                var shapeB = new Circle(2);
                var transformB = new Transform2D(new Position2D(4, 5));

                var minkowskiDifference = new MinkowskiDifference(shapeA, transformA, shapeB, transformB);

                var directionA = Vector2.UnitX;
                var directionB = Vector2.UnitY;
                var directionC = -Vector2.UnitX;
                var directionD = -Vector2.UnitY;

                var simplexA = new Simplex(minkowskiDifference, directionA, directionB);
                var simplexB = new Simplex(minkowskiDifference, directionC, directionD);

                (simplexA != simplexB).Should().BeTrue();
            }
        }
    }
}