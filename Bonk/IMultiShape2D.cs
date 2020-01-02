using System.Collections.Generic;
using MoonTools.Core.Structs;

namespace MoonTools.Core.Bonk
{
    public interface IMultiShape2D : IHasAABB2D
    {
        IEnumerable<(IShape2D, Transform2D)> ShapeTransformPairs { get; }
    }
}
