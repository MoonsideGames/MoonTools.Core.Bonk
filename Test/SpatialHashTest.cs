﻿using FluentAssertions;
using NUnit.Framework;
using MoonTools.Core.Structs;
using MoonTools.Core.Bonk;
using System.Numerics;

namespace Tests
{
    public class SpatialHashTest
    {
        [Test]
        public void InsertAndRetrieve()
        {
            var spatialHash = new SpatialHash<int>(16);

            var rectA = new Rectangle(-2, -2, 2, 2);
            var rectATransform = new Transform2D(new Vector2(-8, -8));

            var rectB = new Rectangle(-2, -2, 2, 2);
            var rectBTransform = new Transform2D(new Vector2(8, 8));

            var rectC = new Rectangle(-2, -2, 2, 2);
            var rectCTransform = new Transform2D(new Vector2(24, -4));

            var rectD = new Rectangle(-2, -2, 2, 2);
            var rectDTransform = new Transform2D(new Vector2(24, 24));

            var circleA = new Circle(2);
            var circleATransform = new Transform2D(new Vector2(24, -8));

            var circleB = new Circle(8);
            var circleBTransform = new Transform2D(new Vector2(16, 16));

            var line = new Line(new Position2D(20, -4), new Position2D(22, -12));
            var lineTransform = new Transform2D(new Vector2(0, 0));

            var point = new Point();
            var pointTransform = new Transform2D(new Position2D(8, 8));

            spatialHash.Insert(0, rectA, rectATransform);
            spatialHash.Insert(1, rectB, rectBTransform);
            spatialHash.Insert(2, rectC, rectCTransform);
            spatialHash.Insert(3, rectD, rectDTransform);
            spatialHash.Insert(4, circleA, circleATransform);
            spatialHash.Insert(1, circleB, circleBTransform);
            spatialHash.Insert(6, line, lineTransform);
            spatialHash.Insert(7, point, pointTransform);

            spatialHash.Retrieve(0, rectA, rectATransform).Should().BeEmpty();
            spatialHash.Retrieve(1, rectB, rectBTransform).Should().NotContain((1, circleB, circleBTransform));
            spatialHash.Retrieve(1, rectB, rectBTransform).Should().Contain((7, point, pointTransform));
            spatialHash.Retrieve(2, rectC, rectCTransform).Should().Contain((6, line, lineTransform)).And.Contain((4, circleA, circleATransform));
            spatialHash.Retrieve(3, rectD, rectDTransform).Should().Contain((1, circleB, circleBTransform));

            spatialHash.Retrieve(4, circleA, circleATransform).Should().Contain((6, line, lineTransform)).And.Contain((2, rectC, rectCTransform));
            spatialHash.Retrieve(1, circleB, circleBTransform).Should().NotContain((1, rectB, rectBTransform)).And.Contain((3, rectD, rectDTransform));

            spatialHash.Retrieve(6, line, lineTransform).Should().Contain((4, circleA, circleATransform)).And.Contain((2, rectC, rectCTransform));
        }

        [Test]
        public void InsertAndRetrieveSameValues()
        {
            var spatialHash = new SpatialHash<int>(16);

            var rectA = new Rectangle(-2, -2, 2, 2);
            var rectATransform = new Transform2D(new Vector2(-8, -8));

            var rectB = new Rectangle(-2, -2, 2, 2);
            var rectBTransform = new Transform2D(new Vector2(-8, -8));

            var rectC = new Rectangle(-1, -1, 1, 1);
            var rectCTransform = new Transform2D(new Vector2(-8, -8));

            spatialHash.Insert(0, rectA, rectATransform);
            spatialHash.Insert(1, rectB, rectBTransform);
            spatialHash.Insert(2, rectC, rectCTransform);

            spatialHash.Retrieve(2, rectC, rectCTransform).Should().HaveCount(2);
        }

        [Test]
        public void Clear()
        {
            var spatialHash = new SpatialHash<int>(16);

            var rectA = new Rectangle(-2, -2, 2, 2);
            var rectATransform = new Transform2D(new Vector2(-8, -8));

            var rectB = new Rectangle(-2, -2, 2, 2);
            var rectBTransform = new Transform2D(new Vector2(8, 8));

            spatialHash.Insert(0, rectA, rectATransform);
            spatialHash.Insert(1, rectB, rectBTransform);

            spatialHash.Clear();

            spatialHash.Retrieve(0, rectA, rectATransform).Should().HaveCount(0);
        }
    }
}
