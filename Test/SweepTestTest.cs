using System.Numerics;
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
            var rectangle = new Rectangle(-2, -2, 4, 4);
            var transform = new Transform2D(new Position2D(-6, 0));

            var otherRectangle = new Rectangle(-2, -2, 4, 4);
            var otherTransform = new Transform2D(new Position2D(6, 0));

            var farthestRectangle = new Rectangle(-2, -2, 4, 4);
            var farthestTransform = new Transform2D(new Position2D(12, 0));

            var downRectangle = new Rectangle(-6, -2, 12, 4);
            var downTransform = new Transform2D(new Position2D(-6, 20));

            var spatialHash = new SpatialHash<int>(16);
            spatialHash.Insert(1, otherRectangle, otherTransform);
            spatialHash.Insert(2, farthestRectangle, farthestTransform);
            spatialHash.Insert(3, downRectangle, downTransform);

            SweepTest.Test(spatialHash, rectangle, transform, new Vector2(12, 0)).Should().Be(
                new SweepResult<int>(true, new Vector2(7, 0), 1)
            );

            SweepTest.Test(spatialHash, rectangle, transform, new Vector2(-12, 0)).Hit.Should().BeFalse();

            SweepTest.Test(spatialHash, rectangle, transform, new Vector2(0, 20)).Should().Be(
                new SweepResult<int>(true, new Vector2(0, 15), 3)
            );
        }
    }
}
