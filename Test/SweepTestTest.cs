﻿using System.Numerics;
using FluentAssertions;
using MoonTools.Core.Bonk;
using MoonTools.Core.Structs;
using NUnit.Framework;

namespace Tests
{
    class SweepTestTest
    {
        [Test]
        public void SweepsThrough()
        {
            var rectangle = new Rectangle(4, 4);
            var transform = new Transform2D(new Position2D(-6, 0));

            var otherRectangle = new Rectangle(4, 4);
            var otherTransform = new Transform2D(new Position2D(6, 0));

            var farthestRectangle = new Rectangle(4, 4);
            var farthestTransform = new Transform2D(new Position2D(12, 0));

            var downRectangle = new Rectangle(12, 4);
            var downTransform = new Transform2D(new Position2D(-6, 20));

            var spatialHash = new SpatialHash<int>(16);
            spatialHash.Insert(1, otherRectangle, otherTransform);
            spatialHash.Insert(2, farthestRectangle, farthestTransform);
            spatialHash.Insert(3, downRectangle, downTransform);

            SweepTest.Rectangle(spatialHash, rectangle, transform, new Vector2(12, 0)).Should().Be(
                new SweepResult<int, Rectangle>(true, new Vector2(7, 0), 1, otherRectangle, otherTransform)
            );

            SweepTest.Rectangle(spatialHash, rectangle, transform, new Vector2(-12, 0)).Hit.Should().BeFalse();

            SweepTest.Rectangle(spatialHash, rectangle, transform, new Vector2(0, 20)).Should().Be(
                new SweepResult<int, Rectangle>(true, new Vector2(0, 15), 3, downRectangle, downTransform)
            );
        }
    }
}
