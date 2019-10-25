using Microsoft.Xna.Framework;

namespace MoonTools.Core.Bonk.Extensions
{
    public static class Vector2Extensions
    {
        public static Vector2 Cross(this Vector2 a, Vector2 b)
        {
            var vec3 = Vector3.Cross(new Vector3(a.X, a.Y, 0), new Vector3(b.X, b.Y, 0));
            return new Vector2(vec3.X, vec3.Y);
        }

        public static Vector2 Perpendicular(this Vector2 v)
        {
            return new Vector2(v.Y, -v.X);
        }
    }
}