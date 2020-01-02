using System.Collections.Generic;
using System.Numerics;
using MoonTools.Core.Structs;

internal unsafe struct SimplexVertexBuffer
{
    private const int Size = 35;

    public int Length { get; private set; }

    public SimplexVertexBuffer(IEnumerable<Position2D> positions)
    {
        var i = 0;
        foreach (var position in positions)
        {
            if (i == Size) { break; }
            var vertex = position.ToVector2();
            _simplexXBuffer[i] = vertex.X;
            _simplexYBuffer[i] = vertex.Y;
            i++;
        }
        Length = i;
    }

    public Vector2 this[int key]
    {
        get => new Vector2(_simplexXBuffer[key], _simplexYBuffer[key]);
        private set
        {
            _simplexXBuffer[key] = value.X;
            _simplexYBuffer[key] = value.Y;
        }
    }

    public void Insert(int index, Vector2 value)
    {
        for (var i = Length; i > index; i--)
        {
            this[i] = this[i - 1];
        }
        this[index] = value;
        Length++;
    }

    private fixed float _simplexXBuffer[Size];
    private fixed float _simplexYBuffer[Size];
}
