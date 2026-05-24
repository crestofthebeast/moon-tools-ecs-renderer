using System;
using FixMath.NET;

public readonly record struct FixPosition
{
    public readonly FixVector2 Pos;
    public readonly Fix64 X { get => Pos.x; }
    public readonly Fix64 Y { get => Pos.y; }
    public readonly int IntX { get => (int)Pos.x; }
    public readonly int IntY { get => (int)Pos.y; }

    public FixPosition(FixVector2 pos)
    {
        Pos = pos;
    }
    public FixPosition(int x, int y)
    {
        Pos = new(new Fix64(x), new Fix64(y));
    }

}