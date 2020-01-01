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
        private readonly Dictionary<T, (IShape2D, Transform2D)> IDLookup = new Dictionary<T, (IShape2D, Transform2D)>();

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
        public void Insert(T id, IShape2D shape, Transform2D transform2D)
        {
            var box = shape.TransformedAABB(transform2D);
            var minHash = Hash(box.Min);
            var maxHash = Hash(box.Max);

            for (var i = minHash.Item1; i <= maxHash.Item1; i++)
            {
                for (var j = minHash.Item2; j <= maxHash.Item2; j++)
                {
                    var key = LongHelper.MakeLong(i, j);
                    if (!hashDictionary.ContainsKey(key))
                    {
                        hashDictionary.Add(key, new HashSet<T>());
                    }

                    hashDictionary[key].Add(id);
                    IDLookup[id] = (shape, transform2D);
                }
            }
        }

        /// <summary>
        /// Retrieves all the potential collisions of a shape-transform pair. Excludes any shape-transforms with the given ID.
        /// </summary>
        public IEnumerable<(T, IShape2D, Transform2D)> Retrieve(T id, IShape2D shape, Transform2D transform2D)
        {
            AABB box = shape.TransformedAABB(transform2D);
            var minHash = Hash(box.Min);
            var maxHash = Hash(box.Max);

            for (int i = minHash.Item1; i <= maxHash.Item1; i++)
            {
                for (int j = minHash.Item2; j <= maxHash.Item2; j++)
                {
                    var key = LongHelper.MakeLong(i, j);
                    if (hashDictionary.ContainsKey(key))
                    {
                        foreach (var t in hashDictionary[key])
                        {
                            var (otherShape, otherTransform) = IDLookup[t];
                            if (!id.Equals(t)) { yield return (t, otherShape, otherTransform); }
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
    }
}
