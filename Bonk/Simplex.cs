﻿using System.Collections.Generic;
using System.Numerics;
using MoonTools.Core.Structs;
using System;

namespace MoonTools.Core.Bonk
{
    /// <summary>
    /// A simplex is a shape with up to n - 2 vertices in the nth dimension.
    /// </summary>
    public struct Simplex2D : IEquatable<Simplex2D>
    {
        private Vector2 a;
        private Vector2? b;
        private Vector2? c;

        public Vector2 A => a;
        public Vector2? B => b;
        public Vector2? C => c;

        public bool ZeroSimplex { get { return !b.HasValue && !c.HasValue; } }
        public bool OneSimplex { get { return b.HasValue && !c.HasValue; } }
        public bool TwoSimplex { get { return b.HasValue && c.HasValue; } }

        public int Count => TwoSimplex ? 3 : (OneSimplex ? 2 : 1);

        public Simplex2D(Vector2 a)
        {
            this.a = a;
            b = null;
            c = null;
        }

        public Simplex2D(Vector2 a, Vector2 b)
        {
            this.a = a;
            this.b = b;
            c = null;
        }

        public Simplex2D(Vector2 a, Vector2 b, Vector2 c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public IEnumerable<Position2D> Vertices
        {
            get
            {
                yield return (Position2D)a;
                if (b.HasValue) { yield return (Position2D)b; }
                if (c.HasValue) { yield return (Position2D)c; }
            }
        }

        public Vector2 Support(Vector2 direction, Transform2D transform)
        {
            var maxDotProduct = float.NegativeInfinity;
            var maxVertex = a;
            foreach (var vertex in Vertices)
            {
                var transformed = Vector2.Transform(vertex, transform.TransformMatrix);
                var dot = Vector2.Dot(transformed, direction);
                if (dot > maxDotProduct)
                {
                    maxVertex = transformed;
                    maxDotProduct = dot;
                }
            }
            return maxVertex;
        }

        public override bool Equals(object obj)
        {
            return obj is Simplex2D other && Equals(other);
        }

        public bool Equals(Simplex2D other)
        {
            if (Count != other.Count) { return false; }

            return
                (A == other.A && B == other.B && C == other.C) ||
                (A == other.A && B == other.C && C == other.B) ||
                (A == other.B && B == other.A && C == other.C) ||
                (A == other.B && B == other.C && C == other.A) ||
                (A == other.C && B == other.A && C == other.B) ||
                (A == other.C && B == other.B && C == other.A);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Vertices);
        }

        public static bool operator ==(Simplex2D a, Simplex2D b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Simplex2D a, Simplex2D b)
        {
            return !(a == b);
        }
    }
}
