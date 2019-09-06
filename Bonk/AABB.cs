using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    public struct AABB
    {
        public int MinX { get; private set; }
        public int MinY { get; private set; }
        public int MaxX { get; private set; }
        public int MaxY { get; private set; }

        public int Width { get { return MaxX - MinX; } }
        public int Height { get { return MaxY - MinY; } }

        public static AABB FromTransformedVertices(IEnumerable<Position2D> vertices, Transform2D transform)
        {
            var TransformedVertices = vertices.Select(vertex => Vector2.Transform(vertex, transform.TransformMatrix));

            return new AABB
            {
                MinX = (int)Math.Round(TransformedVertices.Min(vertex => vertex.X)),
                MinY = (int)Math.Round(TransformedVertices.Min(vertex => vertex.Y)),
                MaxX = (int)Math.Round(TransformedVertices.Max(vertex => vertex.X)),
                MaxY = (int)Math.Round(TransformedVertices.Max(vertex => vertex.Y))
            };
        }

        public AABB(int minX, int minY, int maxX, int maxY)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
        }
    }
}