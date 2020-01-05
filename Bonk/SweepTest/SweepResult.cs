using System;
using System.Numerics;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    public struct SweepResult<T, U> where T : IEquatable<T> where U : struct, IShape2D
    {
        public static SweepResult<T, U> False = new SweepResult<T, U>();

        public bool Hit { get; }
        public Vector2 Motion { get; }
        public T ID { get; }
        public U Shape { get; }
        public Transform2D Transform { get; }

        public SweepResult(bool hit, Vector2 motion, T id, U shape, Transform2D transform)
        {
            Hit = hit;
            Motion = motion;
            ID = id;
            Shape = shape;
            Transform = transform;
        }
    }
}
