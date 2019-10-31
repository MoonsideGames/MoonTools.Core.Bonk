using System.Numerics;

namespace MoonTools.Core.Bonk.Extensions
{
    internal static class Vector2Extensions
    {
        internal static float Cross(this Vector2 a, Vector2 b)
        {
            return Vector3.Cross(new Vector3(a.X, a.Y, 0), new Vector3(b.X, b.Y, 0)).Z;
        }

        internal static Vector2 Perpendicular(this Vector2 a, Vector2 b)
        {
            var ab = b - a;
            return a.Cross(b) > 0 ? Vector2.Normalize(new Vector2(ab.Y, ab.X)) : Vector2.Normalize(new Vector2(ab.Y, -ab.X));
        }
    }
}