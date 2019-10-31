using System.Numerics;

namespace MoonTools.Core.Bonk
{
    internal struct Edge
    {
        public float distance;
        public Vector2 normal;
        public int index;

        public Edge(float distance, Vector2 normal, int index)
        {
            this.distance = distance;
            this.normal = normal;
            this.index = index;
        }
    }
}
