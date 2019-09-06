using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MoonTools.Core.Bonk
{
    public struct SimplexVertices : IEnumerable<Vector2>
    {
        public Vector2?[] vertices;

        /// <summary>
        /// Make sure to pass in all nulls
        /// </summary>
        public SimplexVertices(Vector2?[] vertices)
        {
            this.vertices = vertices;
        }

        public Vector2 this[int key]
        {
            get
            {
                if (!vertices[key].HasValue) { throw new IndexOutOfRangeException(); }
                return vertices[key].Value;
            }
            set
            {
                vertices[key] = value;
            }
        }

        public int Count {
            get
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    if (!vertices[i].HasValue) { return i; }
                }
                return vertices.Length;
            }
        }

        public void Add(Vector2 vertex)
        {
            if (Count > vertices.Length - 1) { throw new IndexOutOfRangeException(); }

            vertices[Count] = vertex;
        }

        public void Insert(int index, Vector2 vertex)
        {
            if (Count >= vertices.Length || index > vertices.Length - 1) { throw new IndexOutOfRangeException(); }

            var currentCount = Count;

            for (int i = currentCount - 1; i >= index; i--)
            {
                vertices[i + 1] = vertices[i];
            }

            vertices[index] = vertex;
        }

        public IEnumerator<Vector2> GetEnumerator()
        {
            foreach (Vector2? vec in vertices)
            {
                if (!vec.HasValue) { yield break; }
                yield return vec.Value;
            }
        }

        public void RemoveAt(int index)
        {
            if (index > vertices.Length - 1 || index > Count) { throw new ArgumentOutOfRangeException(); }

            for (int i = vertices.Length - 2; i >= index; i--)
            {
                vertices[i] = vertices[i + 1];
            }

            vertices[vertices.Length - 1] = null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}