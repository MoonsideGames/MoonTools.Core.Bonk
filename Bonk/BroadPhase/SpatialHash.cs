using System;
using System.Collections.Generic;
using System.Numerics;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    /// <summary>
    /// Used to quickly check if two shapes are potentially overlapping.
    /// </summary>
    /// <typeparam name="T">The type that will be used to uniquely identify shape-transform pairs.</typeparam>
    public class SpatialHash<T> where T : IEquatable<T>
    {
        private readonly int cellSize;

        private readonly Dictionary<long, HashSet<T>> hashDictionary = new Dictionary<long, HashSet<T>>();
        private readonly Dictionary<T, (IHasAABB2D, Transform2D)> IDLookup = new Dictionary<T, (IHasAABB2D, Transform2D)>();

        public int MinX { get; private set; } = 0;
        public int MaxX { get; private set; } = 0;
        public int MinY { get; private set; } = 0;
        public int MaxY { get; private set; } = 0;


        public SpatialHash(int cellSize)
        {
            this.cellSize = cellSize;
        }

        private (int, int) Hash(Vector2 position)
        {
            return ((int)Math.Floor(position.X / cellSize), (int)Math.Floor(position.Y / cellSize));
        }

        /// <summary>
        /// Inserts an element into the SpatialHash.
        /// </summary>
        /// <param name="id">A unique ID for the shape-transform pair.</param>
        /// <param name="shape"></param>
        /// <param name="transform2D"></param>
        public void Insert(T id, IHasAABB2D shape, Transform2D transform2D)
        {
            var box = shape.TransformedAABB(transform2D);
            var minHash = Hash(box.Min);
            var maxHash = Hash(box.Max);

            for (var i = minHash.Item1; i <= maxHash.Item1; i++)
            {
                for (var j = minHash.Item2; j <= maxHash.Item2; j++)
                {
                    var key = MakeLong(i, j);
                    if (!hashDictionary.ContainsKey(key))
                    {
                        hashDictionary.Add(key, new HashSet<T>());
                    }

                    hashDictionary[key].Add(id);
                    IDLookup[id] = (shape, transform2D);
                }
            }

            MinX = Math.Min(MinX, minHash.Item1);
            MinY = Math.Min(MinY, minHash.Item2);
            MaxX = Math.Max(MaxX, maxHash.Item1);
            MaxY = Math.Max(MaxY, maxHash.Item2);
        }

        /// <summary>
        /// Retrieves all the potential collisions of a shape-transform pair. Excludes any shape-transforms with the given ID.
        /// </summary>
        public IEnumerable<(T, IHasAABB2D, Transform2D)> Retrieve(T id, IHasAABB2D shape, Transform2D transform2D)
        {
            var box = shape.TransformedAABB(transform2D);
            var (minX, minY) = Hash(box.Min);
            var (maxX, maxY) = Hash(box.Max);

            if (minX < MinX) { minX = MinX; }
            if (maxX > MaxX) { maxX = MaxX; }
            if (minY < MinY) { minY = MinY; }
            if (maxY > MaxY) { maxY = MaxY; }

            for (var i = minX; i <= maxX; i++)
            {
                for (var j = minY; j <= maxY; j++)
                {
                    var key = MakeLong(i, j);
                    if (hashDictionary.ContainsKey(key))
                    {
                        foreach (var t in hashDictionary[key])
                        {
                            var (otherShape, otherTransform) = IDLookup[t];
                            if (!id.Equals(t) && AABB.TestOverlap(box, otherShape.TransformedAABB(otherTransform)))
                            {
                                yield return (t, otherShape, otherTransform);
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Retrieves objects based on a pre-transformed AABB.
        /// </summary>
        /// <param name="aabb">A transformed AABB.</param>
        /// <returns></returns>
        public IEnumerable<(T, IHasAABB2D, Transform2D)> Retrieve(AABB aabb)
        {
            var (minX, minY) = Hash(aabb.Min);
            var (maxX, maxY) = Hash(aabb.Max);

            if (minX < MinX) { minX = MinX; }
            if (maxX > MaxX) { maxX = MaxX; }
            if (minY < MinY) { minY = MinY; }
            if (maxY > MaxY) { maxY = MaxY; }

            for (var i = minX; i <= maxX; i++)
            {
                for (var j = minY; j <= maxY; j++)
                {
                    var key = MakeLong(i, j);
                    if (hashDictionary.ContainsKey(key))
                    {
                        foreach (var t in hashDictionary[key])
                        {
                            var (otherShape, otherTransform) = IDLookup[t];
                            if (AABB.TestOverlap(aabb, otherShape.TransformedAABB(otherTransform)))
                            {
                                yield return (t, otherShape, otherTransform);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Removes everything that has been inserted into the SpatialHash.
        /// </summary>
        public void Clear()
        {
            foreach (var hash in hashDictionary.Values)
            {
                hash.Clear();
            }

            IDLookup.Clear();
        }

        private static long MakeLong(int left, int right)
        {
            return ((long)left << 32) | ((uint)right);
        }
    }
}
