// adapted from https://github.com/prophetgoddess/FNAECSTemplate/blob/main/FNAECSTemplate/Components/AABB.cs
using System;
using FixMath.NET;

// TODO theres a lot of yucky casts here that have gross perf implications

public readonly struct AABB
{
    public FixVector2 Center { get; }

    public Fix64 X
    {
        get
        {
            return Center.X;
        }
    }

    public Fix64 Y
    {
        get
        {
            return Center.Y;
        }
    }

    public Fix64 Width { get; }
    public Fix64 Height { get; }
    public FixVector2 TopLeft
    {
        get
        {
            return new FixVector2(Center.X - Width * (Fix64)0.5f, Center.Y - Height * (Fix64)0.5f);
        }
    }

    public FixVector2 TopRight
    {
        get
        {
            return new FixVector2(Center.X + Width * (Fix64)0.5f, Center.Y - Height * (Fix64)0.5f);
        }
    }

    public FixVector2 BottomLeft
    {
        get
        {
            return new FixVector2(Center.X - Width * (Fix64)0.5f, Center.Y + Height * (Fix64)0.5f);
        }
    }

    public FixVector2 BottomRight
    {
        get
        {
            return new FixVector2(Center.X + Width * (Fix64)0.5f, Center.Y + Height * (Fix64)0.5f);
        }
    }

    public FixVector2 Left
    {
        get
        {
            return new FixVector2(Center.X - Width * (Fix64)0.5f, Center.Y);
        }
    }

    public FixVector2 Right
    {
        get
        {
            return new FixVector2(Center.X + Width * (Fix64)0.5f, Center.Y);
        }
    }

    public FixVector2 Top
    {
        get
        {
            return new FixVector2(Center.X, Center.Y - Height * (Fix64)0.5f);
        }
    }

    public FixVector2 Bottom
    {
        get
        {
            return new FixVector2(Center.X, Center.Y + Height * (Fix64)0.5f);
        }
    }

    public AABB(Fix64 x, Fix64 y, Fix64 w, Fix64 h)
    {
        Center = new FixVector2(x, y);
        Width = w;
        Height = h;
    }

    public AABB(FixVector2 position, Fix64 w, Fix64 h)
    {
        Center = position;
        Width = w;
        Height = h;
    }

    public bool Contains(FixVector2 point)
    {
        return (this.Left.X <= point.X) &&
            (point.X < this.Right.X) &&
            (this.Top.Y <= point.Y) &&
            (point.Y <= this.Bottom.Y);
    }

    public AABB MinkowskiDifference(AABB other)
    {
        var mdLeft = Left - other.Right;
        var mdTop = Top - other.Bottom;
        var mdWidth = Width + other.Width;
        var mdHeight = Height + other.Height;

        var mdX = mdLeft.X + mdWidth * (Fix64)0.5f;
        var mdY = mdTop.Y + mdHeight * (Fix64)0.5f;

        return new AABB(mdX, mdY, mdWidth, mdHeight);
    }

    public AABB ToWorld(FixVector2 position)
    {
        return new AABB(X + position.X, Y + position.Y, Width, Height);
    }

    public (FixVector2 penetrationVector, bool overlaps) Overlaps(AABB other)
    {
        var md = MinkowskiDifference(other);
        var min = md.TopLeft;
        var max = md.BottomRight;

        if (min.X <= Fix64.Zero && max.X >= Fix64.Zero &&
            min.Y <= Fix64.Zero && max.Y >= Fix64.Zero)
        {
            return (PenetrationVector(md), true);
        }

        return (FixVector2.Zero, false);
    }

    FixVector2 PenetrationVector(AABB md)
    {
        var min = md.TopLeft;
        var max = md.BottomRight;

        var minDist = Fix64.Abs(min.X);
        var boundsPoint = new FixVector2(min.X, Fix64.Zero);

        if (Fix64.Abs(max.X) < minDist)
        {
            minDist = Fix64.Abs(max.X);
            boundsPoint.X = max.X;
        }

        if (Fix64.Abs(max.Y) < minDist)
        {
            minDist = Fix64.Abs(max.Y);
            boundsPoint.X = Fix64.Zero;
            boundsPoint.Y = max.Y;
        }

        if (Fix64.Abs(min.Y) < minDist)
        {
            boundsPoint.X = Fix64.Zero;
            boundsPoint.Y = min.Y;
        }

        return boundsPoint;
    }
}