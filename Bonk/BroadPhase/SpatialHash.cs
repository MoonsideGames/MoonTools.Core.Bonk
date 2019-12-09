using System;
using System.Collections.Generic;
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

        private readonly Dictionary<int, Dictionary<int, HashSet<T>>> hashDictionary = new Dictionary<int, Dictionary<int, HashSet<T>>>();
        private readonly Dictionary<T, (IShape2D, Transform2D)> IDLookup = new Dictionary<T, (IShape2D, Transform2D)>();

        public SpatialHash(int cellSize)
        {
            this.cellSize = cellSize;
        }

        private (int, int) Hash(float x, float y)
        {
            return ((int)Math.Floor(x / cellSize), (int)Math.Floor(y / cellSize));
        }

        /// <summary>
        /// Inserts an element into the SpatialHash.
        /// </summary>
        /// <param name="id">A unique ID for the shape-transform pair.</param>
        /// <param name="shape"></param>
        /// <param name="transform2D"></param>
        public void Insert(T id, IShape2D shape, Transform2D transform2D)
        {
            if (shape == null) { throw new ArgumentNullException(nameof(shape)); }

            var box = shape.AABB(transform2D);
            var minHash = Hash(box.MinX, box.MinY);
            var maxHash = Hash(box.MaxX, box.MaxY);

            for (int i = minHash.Item1; i <= maxHash.Item1; i++)
            {
                for (int j = minHash.Item2; j <= maxHash.Item2; j++)
                {
                    if (!hashDictionary.ContainsKey(i))
                    {
                        hashDictionary.Add(i, new Dictionary<int, HashSet<T>>());
                    }

                    if (!hashDictionary[i].ContainsKey(j))
                    {
                        hashDictionary[i].Add(j, new HashSet<T>());
                    }

                    hashDictionary[i][j].Add(id);
                    IDLookup[id] = (shape, transform2D);
                }
            }
        }

        /// <summary>
        /// Retrieves all the potential collisions of a shape-transform pair. Excludes any shape-transforms with the given ID.
        /// </summary>
        public IEnumerable<(T, IShape2D, Transform2D)> Retrieve(T id, IShape2D shape, Transform2D transform2D)
        {
            if (shape == null) { throw new ArgumentNullException(paramName: nameof(shape)); }

            AABB box = shape.AABB(transform2D);
            var minHash = Hash(box.MinX, box.MinY);
            var maxHash = Hash(box.MaxX, box.MaxY);

            for (int i = minHash.Item1; i <= maxHash.Item1; i++)
            {
                for (int j = minHash.Item2; j <= maxHash.Item2; j++)
                {
                    if (hashDictionary.ContainsKey(i) && hashDictionary[i].ContainsKey(j))
                    {
                        foreach (var t in hashDictionary[i][j])
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
            foreach (var innerDict in hashDictionary.Values)
            {
                foreach (var set in innerDict.Values)
                {
                    set.Clear();
                }
            }

            IDLookup.Clear();
        }
    }
}