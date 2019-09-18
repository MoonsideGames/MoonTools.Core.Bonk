using System;
using System.Collections.Generic;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    public class SpatialHash<T> where T : IEquatable<T>
    {
        private readonly int cellSize;

        private readonly Dictionary<int, Dictionary<int, HashSet<(IShape2D, Transform2D)>>> hashDictionary = new Dictionary<int, Dictionary<int, HashSet<(IShape2D, Transform2D)>>>();
        private readonly Dictionary<(IShape2D, Transform2D), T> IDLookup = new Dictionary<(IShape2D, Transform2D), T>();

        public SpatialHash(int cellSize)
        {
            this.cellSize = cellSize;
        }

        private (int, int) Hash(int x, int y)
        {
            return ((int)Math.Floor((float)x / cellSize), (int)Math.Floor((float)y / cellSize));
        }

        public void Insert(T id, IShape2D shape, Transform2D Transform2D)
        {
            var box = shape.AABB(Transform2D);
            var minHash = Hash(box.MinX, box.MinY);
            var maxHash = Hash(box.MaxX, box.MaxY);

            for (int i = minHash.Item1; i <= maxHash.Item1; i++)
            {
                for (int j = minHash.Item2; j <= maxHash.Item2; j++)
                {
                    if (!hashDictionary.ContainsKey(i))
                    {
                        hashDictionary.Add(i, new Dictionary<int, HashSet<(IShape2D, Transform2D)>>());
                    }

                    if (!hashDictionary[i].ContainsKey(j))
                    {
                        hashDictionary[i].Add(j, new HashSet<(IShape2D, Transform2D)>());
                    }

                    hashDictionary[i][j].Add((shape, Transform2D));
                    IDLookup[(shape, Transform2D)] = id;
                }
            }
        }

        public IEnumerable<(T, IShape2D, Transform2D)> Retrieve(T id, IShape2D shape, Transform2D Transform2D)
        {
            var box = shape.AABB(Transform2D);
            var minHash = Hash(box.MinX, box.MinY);
            var maxHash = Hash(box.MaxX, box.MaxY);

            for (int i = minHash.Item1; i <= maxHash.Item1; i++)
            {
                for (int j = minHash.Item2; j <= maxHash.Item2; j++)
                {
                    if (hashDictionary.ContainsKey(i) && hashDictionary[i].ContainsKey(j))
                    {
                        foreach (var (otherShape, otherTransform2D) in hashDictionary[i][j])
                        {
                            var otherID = IDLookup[(otherShape, otherTransform2D)];
                            if (!id.Equals(otherID)) { yield return (otherID, otherShape, otherTransform2D); }
                        }
                    }
                }
            }
        }

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