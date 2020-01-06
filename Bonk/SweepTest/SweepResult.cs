using System;
using System.Numerics;

namespace MoonTools.Core.Bonk
{
    public struct SweepResult<T> where T : IEquatable<T>
    {
        public static SweepResult<T> False = new SweepResult<T>();

        public bool Hit { get; }
        public Vector2 Motion { get; }
        public T ID { get; }

        public SweepResult(bool hit, Vector2 motion, T id)
        {
            Hit = hit;
            Motion = motion;
            ID = id;
        }
    }
}
