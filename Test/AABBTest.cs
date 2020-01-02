using NUnit.Framework;
using FluentAssertions;
using MoonTools.Core.Bonk;
using System.Numerics;

namespace Tests
{
    public class AABBTest
    {
        [Test]
        public void Overlapping()
        {
            var a = new AABB(new Vector2(-1, -1), new Vector2(1, 1));
            var b = new AABB(new Vector2(0, 0), new Vector2(2, 2));

            AABB.TestOverlap(a, b).Should().BeTrue();

            var c = new AABB(-2, -2, 2, 1);
            var d = new AABB(-2, -2, 2, 2);

            AABB.TestOverlap(c, d).Should().BeTrue();
        }

        [Test]
        public void NotOverlapping()
        {
            var a = new AABB(new Vector2(-1, -1), new Vector2(1, 1));
            var b = new AABB(new Vector2(-3, -3), new Vector2(-2, -2));

            AABB.TestOverlap(a, b).Should().BeFalse();
        }
    }
}
